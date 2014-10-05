using System.Collections.Generic;

namespace HansKindberg.DirectoryServices.Protocols.ActiveDirectory
{
	public class ActiveDirectorySettings : DirectorySettings
	{
		#region Fields

		private const string _identityAttributeName = "objectGUID";
		private static readonly IEnumerable<string> _identityAttributes = new[] {_identityAttributeName};
		private static readonly IEnumerable<string> _minimumNumberOfAttributes = new[] {"objectClass", _identityAttributeName};

		#endregion

		#region Properties

		public override string IdentityAttributeName
		{
			get { return _identityAttributeName; }
		}

		public override IEnumerable<string> IdentityAttributes
		{
			get { return _identityAttributes; }
		}

		public override IEnumerable<string> MinimumNumberOfAttributes
		{
			get { return _minimumNumberOfAttributes; }
		}

		#endregion
	}
}