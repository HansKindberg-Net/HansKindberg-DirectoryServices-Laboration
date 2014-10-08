using System.Configuration;

namespace HansKindberg.DirectoryServices.Configuration
{
	public class SectionGroup : ConfigurationSectionGroup
	{
		#region Fields

		public const string DefaultSectionGroupName = "hansKindberg.directoryServices";

		#endregion

		//[ConfigurationProperty(PrincipalRepositorySection.DefaultSectionName)]
		//public virtual PrincipalRepositorySection PrincipalRepository
		//{
		//	get { return (PrincipalRepositorySection) this.Sections[PrincipalRepositorySection.DefaultSectionName]; }
		//}
	}
}