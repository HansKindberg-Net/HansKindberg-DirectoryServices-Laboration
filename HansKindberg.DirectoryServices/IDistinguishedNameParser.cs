namespace HansKindberg.DirectoryServices
{
	public interface IDistinguishedNameParser
	{
		#region Methods

		IDistinguishedName Parse(string value);
		bool TryParse(string value, out IDistinguishedName distinguishedName);

		#endregion
	}
}