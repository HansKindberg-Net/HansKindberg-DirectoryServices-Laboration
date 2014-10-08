using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace HansKindberg.DirectoryServices.Protocols.Abstractions
{
	public interface IDirectory
	{
		#region Methods

		IEnumerable<ISearchResultEntry> Find(string distinguishedName);
		IEnumerable<ISearchResultEntry> Find(string distinguishedName, ISearchOptions searchOptions);

		[SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get")]
		ISearchResultEntry Get(string distinguishedName);

		[SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get")]
		ISearchResultEntry Get(string distinguishedName, ISearchOptions searchOptions);

		IEnumerable<ISearchResultEntry> GetAncestors(string distinguishedName);
		IEnumerable<ISearchResultEntry> GetAncestors(string distinguishedName, ISearchOptions searchOptions);
		IEnumerable<ISearchResultEntry> GetChildren(string distinguishedName);
		IEnumerable<ISearchResultEntry> GetChildren(string distinguishedName, ISearchOptions searchOptions);
		ISearchResultEntry GetParent(string distinguishedName);
		ISearchResultEntry GetParent(string distinguishedName, ISearchOptions searchOptions);

		#endregion
	}
}