using System;
using System.Diagnostics.CodeAnalysis;
using System.DirectoryServices.Protocols;
using System.Net;

namespace HansKindberg.DirectoryServices.Protocols.Connections
{
	public class LdapConnectionFactory : ILdapConnectionFactory
	{
		#region Fields

		private static readonly LdapDirectoryIdentifier _defaultLdapDirectoryIdentifier = new LdapDirectoryIdentifier("server", true, false);
		private const int _defaultProtocolVersion = 3;

		#endregion

		#region Properties

		protected internal virtual LdapDirectoryIdentifier DefaultLdapDirectoryIdentifier
		{
			get { return _defaultLdapDirectoryIdentifier; }
		}

		#endregion

		#region Methods

		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Should be disposed by the caller.")]
		public virtual LdapConnection Create(ILdapConnectionSettings ldapConnectionSettings)
		{
			if(ldapConnectionSettings == null)
				throw new ArgumentNullException("ldapConnectionSettings");

			if(string.IsNullOrEmpty(ldapConnectionSettings.Host))
				throw new ArgumentException("The host can not be null or empty.", "ldapConnectionSettings");

			var ldapConnection = new LdapConnection(this.CreateLdapDirectoryIdentifier(ldapConnectionSettings.Host, ldapConnectionSettings.Port));

			if(ldapConnectionSettings.AuthenticationType != null)
				ldapConnection.AuthType = ldapConnectionSettings.AuthenticationType.Value;

			if(ldapConnectionSettings.UserName != null)
				ldapConnection.Credential = new NetworkCredential(ldapConnectionSettings.UserName, ldapConnectionSettings.Password);

			if(ldapConnectionSettings.SecureSocketLayer != null)
				ldapConnection.SessionOptions.SecureSocketLayer = ldapConnectionSettings.SecureSocketLayer.Value;

			if(ldapConnectionSettings.Timeout != null)
				ldapConnection.Timeout = ldapConnectionSettings.Timeout.Value;

			ldapConnection.SessionOptions.ProtocolVersion = _defaultProtocolVersion;

			return ldapConnection;
		}

		protected internal virtual LdapDirectoryIdentifier CreateLdapDirectoryIdentifier(string host, int? port)
		{
			var portNumber = port == null ? this.DefaultLdapDirectoryIdentifier.PortNumber : port.Value;

			return new LdapDirectoryIdentifier(host, portNumber, this.DefaultLdapDirectoryIdentifier.FullyQualifiedDnsHostName, this.DefaultLdapDirectoryIdentifier.Connectionless);
		}

		#endregion
	}
}