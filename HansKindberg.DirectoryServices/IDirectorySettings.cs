using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace HansKindberg.DirectoryServices
{
	public interface IDirectorySettings
	{
		#region Properties

		[SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
		IEnumerable<string> Attributes { get; }

		string DefaultDistinguishedName { get; }
		string Filter { get; }
		string HiddenObjectClass { get; }
		string IdentityAttribute { get; }
		string ObjectClassAttribute { get; }
		int? PageSize { get; }
		int? SizeLimit { get; }

		#endregion

		#region Methods

		IEnumerable<string> GetAttributes(IEnumerable<string> attributes, AttributesSetting attributesSetting);

		#endregion
	}
}