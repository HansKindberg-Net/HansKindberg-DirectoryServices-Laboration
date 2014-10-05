using System.Collections.Generic;

namespace HansKindberg.DirectoryServices.Protocols.Abstractions
{
	public interface ISearchResultEntry
	{
		#region Properties

		IEnumerable<IDirectoryAttribute> Attributes { get; }
		string DistinguishedName { get; }

		#endregion
	}
}