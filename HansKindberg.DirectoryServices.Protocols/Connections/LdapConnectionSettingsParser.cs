using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.DirectoryServices.Protocols;
using System.Globalization;
using System.Linq;

namespace HansKindberg.DirectoryServices.Protocols.Connections
{
	public class LdapConnectionSettingsParser : ILdapConnectionSettingsParser
	{
		#region Fields

		private static readonly IEqualityComparer<string> _defaultStringComparer = System.StringComparer.OrdinalIgnoreCase;
		private readonly char _nameValueDelimiter;
		private readonly char _parameterDelimiter;
		private readonly IEqualityComparer<string> _stringComparer;

		#endregion

		#region Constructors

		public LdapConnectionSettingsParser() : this(LdapConnectionSettings.DefaultParameterDelimiter, LdapConnectionSettings.DefaultNameValueDelimiter, _defaultStringComparer) {}

		public LdapConnectionSettingsParser(char parameterDelimiter, char nameValueDelimiter, IEqualityComparer<string> stringComparer)
		{
			if(stringComparer == null)
				throw new ArgumentNullException("stringComparer");

			this._nameValueDelimiter = nameValueDelimiter;
			this._parameterDelimiter = parameterDelimiter;
			this._stringComparer = stringComparer;
		}

		#endregion

		#region Properties

		public virtual char NameValueDelimiter
		{
			get { return this._nameValueDelimiter; }
		}

		public virtual char ParameterDelimiter
		{
			get { return this._parameterDelimiter; }
		}

		public virtual IEqualityComparer<string> StringComparer
		{
			get { return this._stringComparer; }
		}

		#endregion

		#region Methods

		protected internal virtual IDictionary<string, string> GetConnectionStringAsDictionary(string connectionString)
		{
			var dictionary = new Dictionary<string, string>(this.StringComparer);

			if(!string.IsNullOrEmpty(connectionString))
			{
				foreach(var nameValue in connectionString.Split(new[] {this.ParameterDelimiter}))
				{
					var nameValueParts = nameValue.Split(new[] {this.NameValueDelimiter}, 2);

					if(nameValueParts.Length == 0)
						continue;

					var value = nameValueParts.Length > 1 ? nameValueParts[1] : string.Empty;

					dictionary.Add(nameValueParts[0], value);
				}
			}

			return dictionary;
		}

		public virtual ILdapConnectionSettings Parse(string connectionString)
		{
			if(connectionString == null)
				throw new ArgumentNullException("connectionString");

			var ldapConnectionSettings = new LdapConnectionSettings();

			var dictionary = this.GetConnectionStringAsDictionary(connectionString);

			if(dictionary.Any())
			{
				try
				{
					foreach(var keyValuePair in dictionary)
					{
						this.TrySetValue(ldapConnectionSettings, keyValuePair);
					}
				}
				catch(Exception exception)
				{
					throw new FormatException(string.Format(CultureInfo.InvariantCulture, "The connection-string \"{0}\" could not be parsed.", connectionString), exception);
				}
			}

			return ldapConnectionSettings;
		}

		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public virtual bool TryParse(string connectionString, out ILdapConnectionSettings ldapConnectionSettings)
		{
			if(connectionString == null)
				throw new ArgumentNullException("connectionString");

			try
			{
				ldapConnectionSettings = this.Parse(connectionString);
				return true;
			}
			catch
			{
				ldapConnectionSettings = null;
				return false;
			}
		}

		[SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
		protected internal virtual void TrySetValue(LdapConnectionSettings ldapConnectionSettings, KeyValuePair<string, string> keyValuePair)
		{
			if(ldapConnectionSettings == null)
				throw new ArgumentNullException("ldapConnectionSettings");

			if(string.IsNullOrEmpty(keyValuePair.Key))
				throw new FormatException("A key can not be empty.");

			switch(keyValuePair.Key.ToLowerInvariant())
			{
				case "authenticationtype":
				{
					ldapConnectionSettings.AuthenticationType = (AuthType) Enum.Parse(typeof(AuthType), keyValuePair.Value);
					break;
				}
				case "host":
				{
					ldapConnectionSettings.Host = keyValuePair.Value;
					break;
				}
				case "password":
				{
					ldapConnectionSettings.Password = keyValuePair.Value;
					break;
				}
				case "port":
				{
					ldapConnectionSettings.Port = int.Parse(keyValuePair.Value, CultureInfo.InvariantCulture);
					break;
				}
				case "securesocketlayer":
				{
					ldapConnectionSettings.SecureSocketLayer = bool.Parse(keyValuePair.Value);
					break;
				}
				case "timeout":
				{
					ldapConnectionSettings.Timeout = TimeSpan.Parse(keyValuePair.Value);
					break;
				}
				case "username":
				{
					ldapConnectionSettings.UserName = keyValuePair.Value;
					break;
				}
				default:
				{
					throw new FormatException(string.Format(CultureInfo.InvariantCulture, "The key \"{0}\" is not valid.", keyValuePair.Key));
				}
			}
		}

		#endregion
	}
}