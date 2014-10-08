using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using HansKindberg.DirectoryServices.Protocols.Connections;

namespace HansKindberg.DirectoryServices.Protocols.Abstractions
{
	public class Directory : HansKindberg.DirectoryServices.Protocols.Directory, IDirectory
	{
		#region Constructors

		public Directory(ILdapConnectionFactory ldapConnectionFactory, ILdapConnectionSettings ldapConnectionSettings, IDirectorySettings directorySettings, IDistinguishedNameParser distinguishedNameParser) : base(ldapConnectionFactory, ldapConnectionSettings, directorySettings, distinguishedNameParser) {}

		#endregion

		#region Methods

		protected internal virtual IEnumerable<ISearchResultEntry> CastCollection(SearchResultEntryCollection searchResultEntries)
		{
			if(searchResultEntries == null)
				return null;

			return this.CastCollection(searchResultEntries.Cast<SearchResultEntry>());
		}

		protected internal virtual IEnumerable<ISearchResultEntry> CastCollection(IEnumerable<SearchResultEntry> searchResultEntries)
		{
			if(searchResultEntries == null)
				return null;

			return searchResultEntries.Select(searchResultEntry => (ISearchResultEntry) (SearchResultEntryWrapper) searchResultEntry);
		}

		IEnumerable<ISearchResultEntry> IDirectory.Find(string distinguishedName)
		{
			return this.CastCollection(this.Find(distinguishedName));
		}

		IEnumerable<ISearchResultEntry> IDirectory.Find(string distinguishedName, ISearchOptions searchOptions)
		{
			return this.CastCollection(this.Find(distinguishedName, searchOptions));
		}

		ISearchResultEntry IDirectory.Get(string distinguishedName)
		{
			return (SearchResultEntryWrapper) this.Get(distinguishedName);
		}

		ISearchResultEntry IDirectory.Get(string distinguishedName, ISearchOptions searchOptions)
		{
			return (SearchResultEntryWrapper) this.Get(distinguishedName, searchOptions);
		}

		IEnumerable<ISearchResultEntry> IDirectory.GetAncestors(string distinguishedName)
		{
			return this.CastCollection(this.GetAncestors(distinguishedName));
		}

		IEnumerable<ISearchResultEntry> IDirectory.GetAncestors(string distinguishedName, ISearchOptions searchOptions)
		{
			return this.CastCollection(this.GetAncestors(distinguishedName, searchOptions));
		}

		IEnumerable<ISearchResultEntry> IDirectory.GetChildren(string distinguishedName)
		{
			return this.CastCollection(this.GetChildren(distinguishedName));
		}

		IEnumerable<ISearchResultEntry> IDirectory.GetChildren(string distinguishedName, ISearchOptions searchOptions)
		{
			return this.CastCollection(this.GetChildren(distinguishedName, searchOptions));
		}

		ISearchResultEntry IDirectory.GetParent(string distinguishedName)
		{
			return (SearchResultEntryWrapper) this.GetParent(distinguishedName);
		}

		ISearchResultEntry IDirectory.GetParent(string distinguishedName, ISearchOptions searchOptions)
		{
			return (SearchResultEntryWrapper) this.GetParent(distinguishedName, searchOptions);
		}

		#endregion
	}
}