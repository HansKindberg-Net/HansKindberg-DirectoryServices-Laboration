using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;

namespace HansKindberg.DirectoryServices.Protocols
{
	public class SearchResultEntryWrapper : ISearchResultEntry
	{
		#region Fields

		private readonly SearchResultEntry _searchResultEntry;

		#endregion

		#region Constructors

		public SearchResultEntryWrapper(SearchResultEntry searchResultEntry)
		{
			if(searchResultEntry == null)
				throw new ArgumentNullException("searchResultEntry");

			this._searchResultEntry = searchResultEntry;
		}

		#endregion

		#region Properties

		public virtual IEnumerable<IDirectoryAttribute> Attributes
		{
			get { return (SearchResultAttributeCollectionWrapper) this.SearchResultEntry.Attributes; }
		}

		public virtual string DistinguishedName
		{
			get { return this.SearchResultEntry.DistinguishedName; }
		}

		protected internal virtual SearchResultEntry SearchResultEntry
		{
			get { return this._searchResultEntry; }
		}

		#endregion

		#region Methods

		public static SearchResultEntryWrapper FromSearchResultEntry(SearchResultEntry searchResultEntry)
		{
			return searchResultEntry;
		}

		#endregion

		#region Implicit operator

		public static implicit operator SearchResultEntryWrapper(SearchResultEntry searchResultEntry)
		{
			return searchResultEntry != null ? new SearchResultEntryWrapper(searchResultEntry) : null;
		}

		#endregion
	}
}