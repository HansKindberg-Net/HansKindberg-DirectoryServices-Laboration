using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using HansKindberg.DirectoryServices.Protocols.Connections;

namespace HansKindberg.DirectoryServices.Protocols
{
	public class AbstractDirectory : Directory, IAbstractDirectory
	{
		#region Constructors

		public AbstractDirectory(ILdapConnectionFactory ldapConnectionFactory, ILdapConnectionSettings ldapConnectionSettings, IDirectorySettings directorySettings, IDistinguishedNameParser distinguishedNameParser) : base(ldapConnectionFactory, ldapConnectionSettings, directorySettings, distinguishedNameParser) {}

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

		ISearchResultEntry IAbstractDirectory.Get(string identity)
		{
			return (SearchResultEntryWrapper) this.Get(identity);
		}

		ISearchResultEntry IAbstractDirectory.Get(string identity, ISearchOptions searchOptions)
		{
			return (SearchResultEntryWrapper) this.Get(identity, searchOptions);
		}

		IEnumerable<ISearchResultEntry> IAbstractDirectory.GetAncestors(string identity)
		{
			return this.CastCollection(this.GetAncestors(identity));
		}

		IEnumerable<ISearchResultEntry> IAbstractDirectory.GetAncestors(string identity, ISearchOptions searchOptions)
		{
			return this.CastCollection(this.GetAncestors(identity, searchOptions));
		}

		IEnumerable<ISearchResultEntry> IAbstractDirectory.GetChildren(string identity)
		{
			return this.CastCollection(this.GetChildren(identity));
		}

		IEnumerable<ISearchResultEntry> IAbstractDirectory.GetChildren(string identity, ISearchOptions searchOptions)
		{
			return this.CastCollection(this.GetChildren(identity, searchOptions));
		}

		ISearchResultEntry IAbstractDirectory.GetParent(string identity)
		{
			return (SearchResultEntryWrapper) this.GetParent(identity);
		}

		ISearchResultEntry IAbstractDirectory.GetParent(string identity, ISearchOptions searchOptions)
		{
			return (SearchResultEntryWrapper) this.GetParent(identity, searchOptions);
		}

		IEnumerable<ISearchResultEntry> IAbstractDirectory.GetTree()
		{
			return this.CastCollection(this.GetTree());
		}

		IEnumerable<ISearchResultEntry> IAbstractDirectory.GetTree(string identity)
		{
			return this.CastCollection(this.GetTree(identity));
		}

		IEnumerable<ISearchResultEntry> IAbstractDirectory.GetTree(ISearchOptions searchOptions)
		{
			return this.CastCollection(this.GetTree(searchOptions));
		}

		IEnumerable<ISearchResultEntry> IAbstractDirectory.GetTree(string identity, ISearchOptions searchOptions)
		{
			return this.CastCollection(this.GetTree(identity, searchOptions));
		}

		#endregion
	}
}