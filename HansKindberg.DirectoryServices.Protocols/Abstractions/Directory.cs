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

		ISearchResultEntry IDirectory.Get(string identity)
		{
			return (SearchResultEntryWrapper) this.Get(identity);
		}

		ISearchResultEntry IDirectory.Get(string identity, ISearchOptions searchOptions)
		{
			return (SearchResultEntryWrapper) this.Get(identity, searchOptions);
		}

		IEnumerable<ISearchResultEntry> IDirectory.GetAncestors(string identity)
		{
			return this.CastCollection(this.GetAncestors(identity));
		}

		IEnumerable<ISearchResultEntry> IDirectory.GetAncestors(string identity, ISearchOptions searchOptions)
		{
			return this.CastCollection(this.GetAncestors(identity, searchOptions));
		}

		IEnumerable<ISearchResultEntry> IDirectory.GetChildren(string identity)
		{
			return this.CastCollection(this.GetChildren(identity));
		}

		IEnumerable<ISearchResultEntry> IDirectory.GetChildren(string identity, ISearchOptions searchOptions)
		{
			return this.CastCollection(this.GetChildren(identity, searchOptions));
		}

		ISearchResultEntry IDirectory.GetParent(string identity)
		{
			return (SearchResultEntryWrapper) this.GetParent(identity);
		}

		ISearchResultEntry IDirectory.GetParent(string identity, ISearchOptions searchOptions)
		{
			return (SearchResultEntryWrapper) this.GetParent(identity, searchOptions);
		}

		IEnumerable<ISearchResultEntry> IDirectory.GetTree()
		{
			return this.CastCollection(this.GetTree());
		}

		IEnumerable<ISearchResultEntry> IDirectory.GetTree(string identity)
		{
			return this.CastCollection(this.GetTree(identity));
		}

		IEnumerable<ISearchResultEntry> IDirectory.GetTree(ISearchOptions searchOptions)
		{
			return this.CastCollection(this.GetTree(searchOptions));
		}

		IEnumerable<ISearchResultEntry> IDirectory.GetTree(string identity, ISearchOptions searchOptions)
		{
			return this.CastCollection(this.GetTree(identity, searchOptions));
		}

		#endregion
	}
}