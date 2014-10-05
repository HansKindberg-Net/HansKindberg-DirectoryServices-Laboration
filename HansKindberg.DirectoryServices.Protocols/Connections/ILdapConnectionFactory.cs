using System.DirectoryServices.Protocols;

namespace HansKindberg.DirectoryServices.Protocols.Connections
{
	public interface ILdapConnectionFactory
	{
		#region Methods

		LdapConnection Create(ILdapConnectionSettings ldapConnectionSettings);

		#endregion
	}
}