using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using HansKindberg.DirectoryServices.Protocols.Connections;

namespace HansKindberg.DirectoryServices.Protocols
{
	public class Directory : Directory<SearchResultEntry>, IDirectory
	{
		#region Fields

		private readonly ILdapConnectionFactory _ldapConnectionFactory;
		private readonly ILdapConnectionSettings _ldapConnectionSettings;

		#endregion

		#region Constructors

		public Directory(ILdapConnectionFactory ldapConnectionFactory, ILdapConnectionSettings ldapConnectionSettings, IDirectorySettings directorySettings, IDistinguishedNameParser distinguishedNameParser) : base(directorySettings, distinguishedNameParser)
		{
			if(ldapConnectionFactory == null)
				throw new ArgumentNullException("ldapConnectionFactory");

			if(ldapConnectionSettings == null)
				throw new ArgumentNullException("ldapConnectionSettings");

			this._ldapConnectionFactory = ldapConnectionFactory;
			this._ldapConnectionSettings = ldapConnectionSettings;
		}

		#endregion

		#region Properties

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

		protected internal virtual SearchRequest CreateSearchRequest(string distinguishedName, string filter, SearchScope searchScope, IEnumerable<string> attributes)
		{
			var searchRequest = new SearchRequest(distinguishedName, filter, searchScope, attributes != null ? attributes.ToArray() : null);

			if(this.DirectorySettings.SizeLimit != null)
				searchRequest.SizeLimit = this.DirectorySettings.SizeLimit.Value;

			return searchRequest;
		}

		public override IEnumerable<SearchResultEntry> Find(string distinguishedName, ISearchOptions searchOptions)
		{
			return this.GetSearchResult(distinguishedName, SearchScope.Subtree, searchOptions, this.DirectorySettings.PageSize);
		}

		public override SearchResultEntry Get(string distinguishedName, ISearchOptions searchOptions)
		{
			return this.GetSearchResult(distinguishedName, SearchScope.Base, searchOptions, null).FirstOrDefault();
		}

		public override IEnumerable<SearchResultEntry> GetAncestors(string distinguishedName, ISearchOptions searchOptions)
		{
			var ancestors = new List<SearchResultEntry>();

			var parent = this.GetParent(distinguishedName, searchOptions);

			while(parent != null)
			{
				ancestors.Add(parent);

				parent = this.GetParent(parent.DistinguishedName, searchOptions);
			}

			return ancestors.ToArray();
		}

		public override IEnumerable<SearchResultEntry> GetChildren(string distinguishedName, ISearchOptions searchOptions)
		{
			return this.GetSearchResult(distinguishedName, SearchScope.OneLevel, searchOptions, this.DirectorySettings.PageSize);
		}

		public override SearchResultEntry GetParent(string distinguishedName, ISearchOptions searchOptions)
		{
			var item = this.Get(distinguishedName, searchOptions);

			if(item == null)
				return null;

			var parentDistinguishedName = this.DistinguishedNameParser.Parse(item.DistinguishedName).Parent;

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

		protected internal virtual IEnumerable<SearchResultEntry> GetSearchResult(string distinguishedName, SearchScope searchScope, ISearchOptions searchOptions, int? pageSize)
		{
			if(searchOptions == null)
				throw new ArgumentNullException("searchOptions");

			using(var ldapConnection = this.LdapConnectionFactory.Create(this.LdapConnectionSettings))
			{
				var searchRequest = this.CreateSearchRequest(distinguishedName, this.ConcatenateFilters(this.DirectorySettings.Filter, searchOptions.Filter), searchScope, this.GetAttributes(searchOptions));

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

		#endregion
	}
}