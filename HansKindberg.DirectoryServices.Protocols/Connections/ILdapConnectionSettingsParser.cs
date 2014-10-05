namespace HansKindberg.DirectoryServices.Protocols.Connections
{
	public interface ILdapConnectionSettingsParser
	{
		#region Methods

		ILdapConnectionSettings Parse(string connectionString);
		bool TryParse(string connectionString, out ILdapConnectionSettings ldapConnectionSettings);

		#endregion
	}
}