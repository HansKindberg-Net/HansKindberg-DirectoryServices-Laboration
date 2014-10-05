using System.Collections.Generic;

namespace HansKindberg.DirectoryServices.Protocols
{
	public interface ISearchOptions
	{
		#region Properties

		IEnumerable<string> Attributes { get; }
		AttributesSetting AttributesSetting { get; }
		string Filter { get; }

		#endregion
	}
}