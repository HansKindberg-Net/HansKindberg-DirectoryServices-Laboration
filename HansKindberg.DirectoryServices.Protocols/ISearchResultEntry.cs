using System.Collections.Generic;

namespace HansKindberg.DirectoryServices.Protocols
{
	public interface ISearchResultEntry
	{
		#region Properties

		IEnumerable<IDirectoryAttribute> Attributes { get; }
		string DistinguishedName { get; }

		#endregion
	}
}