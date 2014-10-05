using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using HansKindberg.Validation;

namespace HansKindberg.DirectoryServices
{
	public class DistinguishedNameComponentValidator : IDistinguishedNameComponentValidator
	{
		#region Fields

		private IEnumerable<char> _invalidValueCharacters;
		private static readonly IEnumerable<char> _specialInvalidValueCharacters = new[] {'/'};
		//private static readonly Regex _validNameRegularExpression = new Regex("^[0-9a-zA-Z]+$", RegexOptions.Compiled);
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

		public virtual IValidationResult ValidateName(string name)
		{
			var validationResult = new ValidationResult();

			if(name == null)
				validationResult.Exceptions.Add(new ArgumentNullException("name"));
			else if(name.Length == 0)
				validationResult.Exceptions.Add(new ArgumentException("The name can not be empty.", "name"));
			else if(!this.ValidNameRegularExpression.IsMatch(name))
				validationResult.Exceptions.Add(new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The name \"{0}\" is invalid.", name), "name"));

			return validationResult;
		}

		public virtual IValidationResult ValidateValue(string value)
		{
			var validationResult = new ValidationResult();

			if(value == null)
			{
				validationResult.Exceptions.Add(new ArgumentNullException("value"));
			}
			else
			{
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
							validationResult.Exceptions.Add(new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The value \"{0}\" is invalid. The value can not contain the character '{1}'. If you need to include the character you have to escape it.", value, invalidValueCharacter), "value"));
					}
				}
				// ReSharper restore LoopCanBeConvertedToQuery
			}

			return validationResult;
		}

		#endregion
	}
}