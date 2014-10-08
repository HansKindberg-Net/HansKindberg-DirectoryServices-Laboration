using System.Configuration;

namespace HansKindberg.DirectoryServices.Configuration
{
	public class DirectorySettingsSection : ConfigurationSection
	{
		#region Fields

		public const string DefaultSectionName = "directorySettings";
		private const string _defaultDirectorySettingsPropertyName = "defaultDirectorySettings";

		#endregion

		#region Properties

		[ConfigurationProperty(_defaultDirectorySettingsPropertyName)]
		public virtual string DefaultDirectorySettings
		{
			get { return (string) this[_defaultDirectorySettingsPropertyName]; }
			set { this[_defaultDirectorySettingsPropertyName] = value; }
		}

		[ConfigurationProperty("", IsDefaultCollection = true)]
		public virtual DirectorySettingsElementCollection DirectorySettingsCollection
		{
			get { return (DirectorySettingsElementCollection) this[string.Empty]; }
		}

		#endregion
	}
}