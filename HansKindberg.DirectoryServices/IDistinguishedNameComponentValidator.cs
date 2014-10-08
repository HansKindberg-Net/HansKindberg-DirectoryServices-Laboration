namespace HansKindberg.DirectoryServices
{
	public interface IDistinguishedNameComponentValidator
	{
		#region Methods

		void ValidateName(string name);
		void ValidateValue(string value);

		#endregion
	}
}