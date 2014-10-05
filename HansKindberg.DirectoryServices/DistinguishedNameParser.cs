using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace HansKindberg.DirectoryServices
{
	public class DistinguishedNameParser : IDistinguishedNameParser
	{
		#region Fields

		private readonly IDistinguishedNameComponentValidator _distinguishedNameComponentValidator;

		#endregion

		#region Constructors

		public DistinguishedNameParser(IDistinguishedNameComponentValidator distinguishedNameComponentValidator)
		{
			if(distinguishedNameComponentValidator == null)
				throw new ArgumentNullException("distinguishedNameComponentValidator");

			this._distinguishedNameComponentValidator = distinguishedNameComponentValidator;
		}

		#endregion

		#region Properties

		protected internal virtual IDistinguishedNameComponentValidator DistinguishedNameComponentValidator
		{
			get { return this._distinguishedNameComponentValidator; }
		}

		#endregion

		#region Methods

		public virtual IDistinguishedName Parse(string value)
		{
			if(value == null)
				throw new ArgumentNullException("value");

			if(value.Length == 0)
				throw new ArgumentException("The value can not be empty.", "value");

			var distinguishedName = new DistinguishedName();

			try
			{
				foreach(var component in this.Split(value, DistinguishedName.DefaultComponentDelimiter))
				{
					var componentParts = this.Split(component, DistinguishedNameComponent.DefaultNameValueDelimiter).ToArray(); // Maybe we should use: this.Split(component, DistinguishedNameComponent.DefaultNameValueDelimiter, 2).ToArray();

					if(componentParts.Length != 2)
						throw new FormatException(string.Format(CultureInfo.InvariantCulture, "Each component in the distinguished name must consist of a name and a value separated by \"{0}\".", DistinguishedNameComponent.DefaultNameValueDelimiter));

					distinguishedName.Components.Add(new DistinguishedNameComponent(componentParts[0].Trim(), componentParts[1], this.DistinguishedNameComponentValidator));
				}
			}
			catch(Exception exception)
			{
				throw new FormatException(string.Format(CultureInfo.InvariantCulture, "The distinguished name \"{0}\" is invalid.", value), exception);
			}

			return distinguishedName;
		}

		protected internal virtual IEnumerable<string> Split(string value, char separator)
		{
			return this.Split(value, separator, int.MaxValue);
		}

		protected internal virtual IEnumerable<string> Split(string value, char separator, int count)
		{
			if(value == null)
				throw new ArgumentNullException("value");

			if(count < 0)
				throw new ArgumentOutOfRangeException("count", "The count can not be less than zero.");

			var temporaryValueParts = value.Split(new[] {separator});

			var valueParts = new List<string>();

			for(int i = 0; i < temporaryValueParts.Length; i++)
			{
				if(valueParts.Count == count - 1)
				{
					valueParts.Add(string.Join(separator.ToString(CultureInfo.InvariantCulture), temporaryValueParts.Skip(i).ToArray()));
					break;
				}

				var valuePart = temporaryValueParts[i];

				while(valuePart.EndsWith(@"\", StringComparison.OrdinalIgnoreCase) && i < temporaryValueParts.Length - 1)
				{
					valuePart += separator + temporaryValueParts[i + 1];
					i++;
				}

				valueParts.Add(valuePart);
			}

			return valueParts.ToArray();
		}

		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public virtual bool TryParse(string value, out IDistinguishedName distinguishedName)
		{
			distinguishedName = null;

			if(string.IsNullOrEmpty(value))
				return false;

			try
			{
				distinguishedName = this.Parse(value);
				return true;
			}
			catch
			{
				distinguishedName = null;
				return false;
			}
		}

		#endregion
	}
}