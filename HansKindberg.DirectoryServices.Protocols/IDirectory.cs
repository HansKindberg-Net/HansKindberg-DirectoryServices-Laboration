using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.DirectoryServices.Protocols;

namespace HansKindberg.DirectoryServices.Protocols
{
	public interface IDirectory
	{
		#region Methods

		IEnumerable<SearchResultEntry> Find(string distinguishedName);
		IEnumerable<SearchResultEntry> Find(string distinguishedName, ISearchOptions searchOptions);

		[SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get")]
		SearchResultEntry Get(string distinguishedName);

		[SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get")]
		SearchResultEntry Get(string distinguishedName, ISearchOptions searchOptions);

		IEnumerable<SearchResultEntry> GetAncestors(string distinguishedName);
		IEnumerable<SearchResultEntry> GetAncestors(string distinguishedName, ISearchOptions searchOptions);
		IEnumerable<SearchResultEntry> GetChildren(string distinguishedName);
		IEnumerable<SearchResultEntry> GetChildren(string distinguishedName, ISearchOptions searchOptions);
		SearchResultEntry GetParent(string distinguishedName);
		SearchResultEntry GetParent(string distinguishedName, ISearchOptions searchOptions);

		#endregion
	}
}