using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace HansKindberg.DirectoryServices
{
	public class DistinguishedNameComponentValidator : IDistinguishedNameComponentValidator
	{
		#region Fields

		private IEnumerable<char> _invalidValueCharacters;
		private static readonly IEnumerable<char> _specialInvalidValueCharacters = new[] {'/'};
		private static readonly Regex _validNameRegularExpression = new Regex(@"^[0-9a-zA-Z]+\z$", RegexOptions.Compiled);

		#endregion

		#region Properties

		protected internal virtual char ComponentDelimiter
		{
			get { return DistinguishedName.DefaultComponentDelimiter; }
		}

		protected internal virtual IEnumerable<char> InvalidValueCharacters
		{
			get { return this._invalidValueCharacters ?? (this._invalidValueCharacters = new[] {this.ComponentDelimiter, this.NameValueDelimiter}.Concat(this.SpecialInvalidValueCharacters)); }
		}

		protected internal virtual char NameValueDelimiter
		{
			get { return DistinguishedNameComponent.DefaultNameValueDelimiter; }
		}

		protected internal virtual IEnumerable<char> SpecialInvalidValueCharacters
		{
			get { return _specialInvalidValueCharacters; }
		}

		protected internal virtual Regex ValidNameRegularExpression
		{
			get { return _validNameRegularExpression; }
		}

		#endregion

		#region Methods

		public virtual void ValidateName(string name)
		{
			if(name == null)
				throw new ArgumentNullException("name");

			if(name.Length == 0)
				throw new ArgumentException("The name can not be empty.", "name");

			if(!this.ValidNameRegularExpression.IsMatch(name))
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The name \"{0}\" is invalid.", name), "name");
		}

		public virtual void ValidateValue(string value)
		{
			if(value == null)
				throw new ArgumentNullException("value");

			var temporaryValue = value;

			// ReSharper disable LoopCanBeConvertedToQuery
			foreach(var invalidValueCharacter in this.InvalidValueCharacters)
			{
				temporaryValue = temporaryValue.Replace(@"\" + invalidValueCharacter, string.Empty);
			}

			foreach(var character in temporaryValue)
			{
				foreach(var invalidValueCharacter in this.InvalidValueCharacters)
				{
					if(character.Equals(invalidValueCharacter))
						throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The value \"{0}\" is invalid. The value can not contain the character '{1}'. If you need to include the character you have to escape it.", value, invalidValueCharacter), "value");
				}
			}
			// ReSharper restore LoopCanBeConvertedToQuery
		}

		#endregion
	}
}