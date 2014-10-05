using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace HansKindberg.DirectoryServices
{
	public class DistinguishedName : IDistinguishedName
	{
		#region Fields

		public const char DefaultComponentDelimiter = ',';
		private readonly IList<IDistinguishedNameComponent> _components = new List<IDistinguishedNameComponent>();
		private const bool _persistNameCaseWhenConvertingToString = false;

		#endregion

		#region Properties

		protected internal virtual char ComponentDelimiter
		{
			get { return DefaultComponentDelimiter; }
		}

		public virtual IList<IDistinguishedNameComponent> Components
		{
			get { return this._components; }
		}

		public virtual IDistinguishedName Parent
		{
			get
			{
				if(this.Components.Count < 2)
					return null;

				var parent = new DistinguishedName();

				foreach(var component in this.Components.Skip(1))
				{
					parent.Components.Add(component);
				}

				return parent;
			}
		}

		protected internal virtual bool PersistNameCaseWhenConvertingToString
		{
			get { return _persistNameCaseWhenConvertingToString; }
		}

		#endregion

		#region Methods

		public override bool Equals(object obj)
		{
			return this.Equals(obj as IDistinguishedName);
		}

		public bool Equals(IDistinguishedName other)
		{
			if(other == null)
				return false;

			if(this.Components.Count != other.Components.Count)
				return false;

			return !this.Components.Where((component, i) => !component.Equals(other.Components[i])).Any();
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
			return string.Join(this.ComponentDelimiter.ToString(CultureInfo.InvariantCulture), this.Components.Select(component => component.ToString()).ToArray());
		}

		#endregion
	}
}