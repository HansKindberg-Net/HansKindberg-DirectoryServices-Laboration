using System;
using System.DirectoryServices.Protocols;

namespace HansKindberg.DirectoryServices.Protocols.Connections
{
	public class LdapConnectionSettings : ILdapConnectionSettings
	{
		#region Fields

		public const char DefaultNameValueDelimiter = '=';
		public const char DefaultParameterDelimiter = ';';
		private int? _port;

		#endregion

		#region Properties

		public virtual AuthType? AuthenticationType { get; set; }
		public virtual string DistinguishedName { get; set; }
		public virtual string Host { get; set; }
		public virtual string Password { get; set; }

		public virtual int? Port
		{
			get { return this._port; }
			set
			{
				if(value != null & value < 0)
					throw new ArgumentException("The port can not be less than zero.");

				this._port = value;
			}
		}

		public virtual bool? SecureSocketLayer { get; set; }
		public virtual TimeSpan? Timeout { get; set; }
		public virtual string UserName { get; set; }

		#endregion
	}
}