using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace HansKindberg.DirectoryServices.Protocols.Abstractions
{
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
	[SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
	public interface IDirectoryAttribute : IList<object>
	{
		#region Properties

		string Name { get; set; }

		#endregion

		#region Methods

		void AddRange(IEnumerable<object> values);

		#endregion
	}
}