using HansKindberg.Validation;

namespace HansKindberg.DirectoryServices
{
	public interface IDistinguishedNameComponentValidator
	{
		#region Methods

		IValidationResult ValidateName(string name);
		IValidationResult ValidateValue(string value);

		#endregion
	}
}