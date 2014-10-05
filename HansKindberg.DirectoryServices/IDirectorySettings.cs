using System.Collections.Generic;

namespace HansKindberg.DirectoryServices
{
	public interface IDirectorySettings
	{
		#region Properties

		string Filter { get; }
		string HiddenObjectClass { get; }
		string IdentityAttributeName { get; }
		IEnumerable<string> IdentityAttributes { get; }
		IEnumerable<string> MinimumNumberOfAttributes { get; }
		IEnumerable<string> NoExistingAttributes { get; }
		string ObjectClassAttributeName { get; }
		int? PageSize { get; }
		int? SizeLimit { get; }

		#endregion
	}
}