using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Globalization;
using System.Linq;
using HansKindberg.DirectoryServices.Protocols.Connections;

namespace HansKindberg.DirectoryServices.Protocols
{
	public class Directory : IDirectory
	{
		#region Fields

		private readonly IDirectorySettings _directorySettings;
		private readonly IDistinguishedNameParser _distinguishedNameParser;
		private readonly ILdapConnectionFactory _ldapConnectionFactory;
		private readonly ILdapConnectionSettings _ldapConnectionSettings;

		#endregion

		#region Constructors

		public Directory(ILdapConnectionFactory ldapConnectionFactory, ILdapConnectionSettings ldapConnectionSettings, IDirectorySettings directorySettings, IDistinguishedNameParser distinguishedNameParser)
		{
			if(ldapConnectionFactory == null)
				throw new ArgumentNullException("ldapConnectionFactory");

			if(ldapConnectionSettings == null)
				throw new ArgumentNullException("ldapConnectionSettings");

			if(directorySettings == null)
				throw new ArgumentNullException("directorySettings");

			if(distinguishedNameParser == null)
				throw new ArgumentNullException("distinguishedNameParser");

			this._directorySettings = directorySettings;
			this._distinguishedNameParser = distinguishedNameParser;
			this._ldapConnectionFactory = ldapConnectionFactory;
			this._ldapConnectionSettings = ldapConnectionSettings;
		}

		#endregion

		#region Properties

		public virtual IDirectorySettings DirectorySettings
		{
			get { return this._directorySettings; }
		}

		protected internal virtual IDistinguishedNameParser DistinguishedNameParser
		{
			get { return this._distinguishedNameParser; }
		}

		protected internal virtual ILdapConnectionFactory LdapConnectionFactory
		{
			get { return this._ldapConnectionFactory; }
		}

		public virtual ILdapConnectionSettings LdapConnectionSettings
		{
			get { return this._ldapConnectionSettings; }
		}

		#endregion

		#region Methods

		protected internal virtual string ConcatenateFilters(string firstFilter, string secondFilter)
		{
			return this.ConcatenateFilters(firstFilter, secondFilter, false);
		}

		protected internal virtual string ConcatenateFilters(string firstFilter, string secondFilter, bool concatenateAsOr)
		{
			if(secondFilter == null)
				return firstFilter;

			if(firstFilter == null)
				return secondFilter;

			if(!firstFilter.StartsWith("(", StringComparison.OrdinalIgnoreCase))
				firstFilter = "(" + firstFilter + ")";

			if(!secondFilter.StartsWith("(", StringComparison.OrdinalIgnoreCase))
				secondFilter = "(" + secondFilter + ")";

			return string.Format(CultureInfo.InvariantCulture, "({0}{1}{2})", concatenateAsOr ? "|" : "&", firstFilter, secondFilter);
		}

		protected internal virtual ISearchOptions CreateDefaultGetAncestorsSettings()
		{
			return new SearchOptions();
		}

		protected internal virtual ISearchOptions CreateDefaultGetChildrenSettings()
		{
			return new SearchOptions();
		}

		protected internal virtual ISearchOptions CreateDefaultGetParentSettings()
		{
			return new SearchOptions();
		}

		protected internal virtual ISearchOptions CreateDefaultGetSettings()
		{
			return new SearchOptions();
		}

		protected internal virtual ISearchOptions CreateDefaultGetTreeSettings()
		{
			return new SearchOptions();
		}

		protected internal virtual SearchRequest CreateSearchRequest(string identity, string filter, SearchScope searchScope, IEnumerable<string> attributes)
		{
			var searchRequest = new SearchRequest(identity, filter, searchScope, attributes != null ? attributes.ToArray() : null);

			if(this.DirectorySettings.SizeLimit != null)
				searchRequest.SizeLimit = this.DirectorySettings.SizeLimit.Value;

			return searchRequest;
		}

		public virtual SearchResultEntry Get(string identity)
		{
			return this.Get(identity, this.CreateDefaultGetSettings());
		}

		public virtual SearchResultEntry Get(string identity, ISearchOptions searchOptions)
		{
			return this.GetSearchResult(identity, searchOptions, SearchScope.Base, null).FirstOrDefault();
		}

		public virtual IEnumerable<SearchResultEntry> GetAncestors(string identity)
		{
			return this.GetAncestors(identity, this.CreateDefaultGetAncestorsSettings());
		}

		public virtual IEnumerable<SearchResultEntry> GetAncestors(string identity, ISearchOptions searchOptions)
		{
			var ancestors = new List<SearchResultEntry>();

			var parent = this.GetParent(identity, searchOptions);

			while(parent != null)
			{
				ancestors.Add(parent);

				parent = this.GetParent(parent.DistinguishedName, searchOptions);
			}

			return ancestors.ToArray();
		}

		protected internal virtual IEnumerable<string> GetAttributes(ISearchOptions searchOptions)
		{
			if(searchOptions == null)
				throw new ArgumentNullException("searchOptions");

			switch(searchOptions.AttributesSetting)
			{
				case AttributesSetting.Identity:
					return this.DirectorySettings.IdentityAttributes;
				case AttributesSetting.Minimum:
					return this.DirectorySettings.MinimumNumberOfAttributes;
				case AttributesSetting.None:
					return this.DirectorySettings.NoExistingAttributes;
				default:
					return searchOptions.Attributes;
			}
		}

		public virtual IEnumerable<SearchResultEntry> GetChildren(string identity)
		{
			return this.GetChildren(identity, this.CreateDefaultGetChildrenSettings());
		}

		public virtual IEnumerable<SearchResultEntry> GetChildren(string identity, ISearchOptions searchOptions)
		{
			return this.GetSearchResult(identity, searchOptions, SearchScope.OneLevel, this.DirectorySettings.PageSize);
		}

		public virtual SearchResultEntry GetParent(string identity)
		{
			return this.GetParent(identity, this.CreateDefaultGetParentSettings());
		}

		public virtual SearchResultEntry GetParent(string identity, ISearchOptions searchOptions)
		{
			var item = this.Get(identity, searchOptions);

			if(item == null)
				return null;

			var distinguishedName = this.DistinguishedNameParser.Parse(item.DistinguishedName);

			var parentDistinguishedName = distinguishedName.Parent;

			if(parentDistinguishedName == null)
				return null;

			return this.Get(parentDistinguishedName.ToString(), searchOptions);
		}

		protected internal virtual SearchResponse GetSearchResponse(LdapConnection ldapConnection, SearchRequest searchRequest)
		{
			if(ldapConnection == null)
				throw new ArgumentNullException("ldapConnection");

			var searchResponse = (SearchResponse) ldapConnection.SendRequest(searchRequest);

			if(searchResponse == null)
				throw new InvalidOperationException("The search-response is null.");

			return searchResponse;
		}

		protected internal virtual IEnumerable<SearchResultEntry> GetSearchResult(string identity, ISearchOptions searchOptions, SearchScope searchScope, int? pageSize)
		{
			if(searchOptions == null)
				throw new ArgumentNullException("searchOptions");

			using(var ldapConnection = this.LdapConnectionFactory.Create(this.LdapConnectionSettings))
			{
				var searchRequest = this.CreateSearchRequest(identity, this.ConcatenateFilters(this.DirectorySettings.Filter, searchOptions.Filter), searchScope, this.GetAttributes(searchOptions));

				if(pageSize != null)
				{
					var pageResultRequestControl = new PageResultRequestControl(pageSize.Value);

					searchRequest.Controls.Add(pageResultRequestControl);

					var searchOptionsControl = new SearchOptionsControl(SearchOption.DomainScope);

					searchRequest.Controls.Add(searchOptionsControl);

					var searchResult = new List<SearchResultEntry>();

					while(true)
					{
						var searchResponse = this.GetSearchResponse(ldapConnection, searchRequest);

						if(searchResponse.Controls.Length != 1 || !(searchResponse.Controls[0] is PageResultResponseControl))
							throw new InvalidOperationException("The server cannot page the result.");

						var pageResultResponseControl = (PageResultResponseControl) searchResponse.Controls[0];

						searchResult.AddRange(searchResponse.Entries.Cast<SearchResultEntry>());

						if(pageResultResponseControl.Cookie.Length == 0)
							break;

						pageResultRequestControl.Cookie = pageResultResponseControl.Cookie;
					}
				}

				return this.GetSearchResponse(ldapConnection, searchRequest).Entries.Cast<SearchResultEntry>();
			}
		}

		public virtual IEnumerable<SearchResultEntry> GetTree()
		{
			return this.GetTree(this.LdapConnectionSettings.DistinguishedName);
		}

		public virtual IEnumerable<SearchResultEntry> GetTree(string identity)
		{
			return this.GetTree(identity, this.CreateDefaultGetTreeSettings());
		}

		public virtual IEnumerable<SearchResultEntry> GetTree(ISearchOptions searchOptions)
		{
			return this.GetTree(this.LdapConnectionSettings.DistinguishedName, searchOptions);
		}

		public virtual IEnumerable<SearchResultEntry> GetTree(string identity, ISearchOptions searchOptions)
		{
			return this.GetSearchResult(identity, searchOptions, SearchScope.Subtree, this.DirectorySettings.PageSize);
		}

		#endregion
	}
}