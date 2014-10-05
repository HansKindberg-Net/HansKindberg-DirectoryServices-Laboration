using System;

namespace HansKindberg.DirectoryServices
{
	public interface IDistinguishedNameComponent : IEquatable<IDistinguishedNameComponent>
	{
		#region Properties

		string Name { get; }
		string Value { get; }

		#endregion
	}
}