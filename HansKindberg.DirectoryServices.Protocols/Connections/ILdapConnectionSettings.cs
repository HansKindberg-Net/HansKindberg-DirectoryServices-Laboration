using System;
using System.DirectoryServices.Protocols;

namespace HansKindberg.DirectoryServices.Protocols.Connections
{
	public interface ILdapConnectionSettings
	{
		#region Properties

		AuthType? AuthenticationType { get; }
		string Host { get; }
		string Password { get; }
		int? Port { get; }
		bool? SecureSocketLayer { get; }
		TimeSpan? Timeout { get; }
		string UserName { get; }

		#endregion
	}
}