using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.DirectoryServices.Protocols;
using HansKindberg.DirectoryServices.Protocols.Connections;

namespace HansKindberg.DirectoryServices.Protocols
{
	public interface IDirectory
	{
		#region Properties

		IDirectorySettings DirectorySettings { get; }
		ILdapConnectionSettings LdapConnectionSettings { get; }

		#endregion

		#region Methods

		[SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get")]
		SearchResultEntry Get(string identity);

		[SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get")]
		SearchResultEntry Get(string identity, ISearchOptions searchOptions);

		IEnumerable<SearchResultEntry> GetAncestors(string identity);
		IEnumerable<SearchResultEntry> GetAncestors(string identity, ISearchOptions searchOptions);
		IEnumerable<SearchResultEntry> GetChildren(string identity);
		IEnumerable<SearchResultEntry> GetChildren(string identity, ISearchOptions searchOptions);
		SearchResultEntry GetParent(string identity);
		SearchResultEntry GetParent(string identity, ISearchOptions searchOptions);
		IEnumerable<SearchResultEntry> GetTree();
		IEnumerable<SearchResultEntry> GetTree(string identity);
		IEnumerable<SearchResultEntry> GetTree(ISearchOptions searchOptions);
		IEnumerable<SearchResultEntry> GetTree(string identity, ISearchOptions searchOptions);

		#endregion
	}
}