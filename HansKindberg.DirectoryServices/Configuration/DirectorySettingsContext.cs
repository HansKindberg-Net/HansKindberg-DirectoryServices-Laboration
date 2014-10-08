using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;

namespace HansKindberg.DirectoryServices.Configuration
{
	public class DirectorySettingsContext : IDirectorySettingsContext
	{
		#region Fields

		private readonly IDictionary<string, IDirectorySettings> _directorySettingsDictionary = new Dictionary<string, IDirectorySettings>(StringComparer.OrdinalIgnoreCase);
		private readonly DirectorySettingsSection _directorySettingsSection;
		private readonly object _lockObject = new object();

		#endregion

		#region Constructors

		public DirectorySettingsContext(DirectorySettingsSection directorySettingsSection)
		{
			if(directorySettingsSection == null)
				throw new ArgumentNullException("directorySettingsSection");

			this._directorySettingsSection = directorySettingsSection;
		}

		#endregion

		#region Properties

		public virtual IDirectorySettings Default
		{
			get { return this.Get(this.DirectorySettingsSection.DefaultDirectorySettings); }
		}

		protected internal virtual IDictionary<string, IDirectorySettings> DirectorySettingsDictionary
		{
			get { return this._directorySettingsDictionary; }
		}

		protected internal virtual DirectorySettingsSection DirectorySettingsSection
		{
			get { return this._directorySettingsSection; }
		}

		protected internal virtual object LockObject
		{
			get { return this._lockObject; }
		}

		#endregion

		#region Methods

		public virtual IDirectorySettings Get(string name)
		{
			IDirectorySettings directorySettings;

			if(!this.DirectorySettingsDictionary.TryGetValue(name, out directorySettings))
			{
				lock(this.LockObject)
				{
					if(!this.DirectorySettingsDictionary.TryGetValue(name, out directorySettings))
					{
						directorySettings = this.DirectorySettingsSection.DirectorySettingsCollection.FirstOrDefault(item => string.Equals(name, item.Name, StringComparison.OrdinalIgnoreCase));

						if(directorySettings == null)
							throw new ConfigurationErrorsException(string.Format(CultureInfo.InvariantCulture, "The directory-settings item with name \"{0}\" does not exist.", name));

						this.DirectorySettingsDictionary[name] = directorySettings;
					}
				}
			}

			return directorySettings;
		}

		/// <summary>
		/// If you are using an IoC-container you can override this method and return the instance you get from the container.
		/// </summary>
		protected internal virtual IDirectorySettings GetDirectorySettingsInstance(Type directorySettingsType)
		{
			return (IDirectorySettings) Activator.CreateInstance(directorySettingsType);
		}

		#endregion
	}
}