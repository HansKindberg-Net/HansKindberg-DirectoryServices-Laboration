using System;

namespace HansKindberg.DirectoryServices
{
	public class DistinguishedNameComponent : IDistinguishedNameComponent
	{
		#region Fields

		public const char DefaultNameValueDelimiter = '=';
		private readonly string _name;
		private const StringComparison _nameComparison = StringComparison.OrdinalIgnoreCase;
		private const bool _persistNameCaseWhenConvertingToString = false;
		private readonly string _value;
		private const StringComparison _valueComparison = StringComparison.OrdinalIgnoreCase;

		#endregion

		#region Constructors

		public DistinguishedNameComponent(string name, string value, IDistinguishedNameComponentValidator distinguishedNameComponentValidator)
		{
			if(distinguishedNameComponentValidator == null)
				throw new ArgumentNullException("distinguishedNameComponentValidator");

			distinguishedNameComponentValidator.ValidateName(name);

			distinguishedNameComponentValidator.ValidateValue(value);

			this._name = name;
			this._value = value;
		}

		#endregion

		#region Properties

		public virtual string Name
		{
			get { return this._name; }
		}

		protected internal virtual StringComparison NameComparison
		{
			get { return _nameComparison; }
		}

		protected internal virtual char NameValueDelimiter
		{
			get { return DefaultNameValueDelimiter; }
		}

		protected internal virtual bool PersistNameCaseWhenConvertingToString
		{
			get { return _persistNameCaseWhenConvertingToString; }
		}

		public virtual string Value
		{
			get { return this._value; }
		}

		protected internal virtual StringComparison ValueComparison
		{
			get { return _valueComparison; }
		}

		#endregion

		#region Methods

		public override bool Equals(object obj)
		{
			return this.Equals(obj as IDistinguishedNameComponent);
		}

		public virtual bool Equals(IDistinguishedNameComponent other)
		{
			if(other == null)
				return false;

			if(!string.Equals(this.Name, other.Name, this.NameComparison))
				return false;

			if(!string.Equals(this.Value, other.Value, this.ValueComparison))
				return false;

			return true;
		}

		public override int GetHashCode()
		{
			return this.ToString().ToUpperInvariant().GetHashCode();
		}

		public override string ToString()
		{
			return this.ToString(this.PersistNameCaseWhenConvertingToString);
		}

		public virtual string ToString(bool persistNameCase)
		{
			return (!persistNameCase ? this.Name.ToUpperInvariant() : this.Name) + this.NameValueDelimiter + this.Value;
		}

		#endregion
	}
}