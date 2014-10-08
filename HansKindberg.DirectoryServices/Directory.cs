using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace HansKindberg.DirectoryServices
{
	public abstract class Directory<T>
	{
		#region Fields

		private readonly IDirectorySettings _directorySettings;
		private readonly IDistinguishedNameParser _distinguishedNameParser;

		#endregion

		#region Constructors

		protected Directory(IDirectorySettings directorySettings, IDistinguishedNameParser distinguishedNameParser)
		{
			if(directorySettings == null)
				throw new ArgumentNullException("directorySettings");

			if(distinguishedNameParser == null)
				throw new ArgumentNullException("distinguishedNameParser");

			this._directorySettings = directorySettings;
			this._distinguishedNameParser = distinguishedNameParser;
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

		protected internal virtual ISearchOptions CreateDefaultSearchOptions()
		{
			return new SearchOptions();
		}

		public virtual IEnumerable<T> Find(string distinguishedName)
		{
			return this.Find(distinguishedName, this.CreateDefaultSearchOptions());
		}

		public abstract IEnumerable<T> Find(string distinguishedName, ISearchOptions searchOptions);

		[SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get")]
		public virtual T Get(string distinguishedName)
		{
			return this.Get(distinguishedName, this.CreateDefaultSearchOptions());
		}

		[SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get")]
		public abstract T Get(string distinguishedName, ISearchOptions searchOptions);

		public virtual IEnumerable<T> GetAncestors(string distinguishedName)
		{
			return this.GetAncestors(distinguishedName, this.CreateDefaultSearchOptions());
		}

		public abstract IEnumerable<T> GetAncestors(string distinguishedName, ISearchOptions searchOptions);

		protected internal virtual IEnumerable<string> GetAttributes(ISearchOptions searchOptions)
		{
			if(searchOptions == null)
				throw new ArgumentNullException("searchOptions");

			return this.DirectorySettings.GetAttributes(searchOptions.Attributes, searchOptions.AttributesSetting);
		}

		public virtual IEnumerable<T> GetChildren(string distinguishedName)
		{
			return this.GetChildren(distinguishedName, this.CreateDefaultSearchOptions());
		}

		public abstract IEnumerable<T> GetChildren(string distinguishedName, ISearchOptions searchOptions);

		public virtual T GetParent(string distinguishedName)
		{
			return this.GetParent(distinguishedName, this.CreateDefaultSearchOptions());
		}

		public abstract T GetParent(string distinguishedName, ISearchOptions searchOptions);

		#endregion
	}
}