using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace HansKindberg.DirectoryServices.Protocols
{
	public interface IAbstractDirectory
	{
		#region Methods

		[SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get")]
		ISearchResultEntry Get(string identity);

		[SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get")]
		ISearchResultEntry Get(string identity, ISearchOptions searchOptions);

		IEnumerable<ISearchResultEntry> GetAncestors(string identity);
		IEnumerable<ISearchResultEntry> GetAncestors(string identity, ISearchOptions searchOptions);
		IEnumerable<ISearchResultEntry> GetChildren(string identity);
		IEnumerable<ISearchResultEntry> GetChildren(string identity, ISearchOptions searchOptions);
		ISearchResultEntry GetParent(string identity);
		ISearchResultEntry GetParent(string identity, ISearchOptions searchOptions);
		IEnumerable<ISearchResultEntry> GetTree();
		IEnumerable<ISearchResultEntry> GetTree(string identity);
		IEnumerable<ISearchResultEntry> GetTree(ISearchOptions searchOptions);
		IEnumerable<ISearchResultEntry> GetTree(string identity, ISearchOptions searchOptions);

		#endregion
	}
}