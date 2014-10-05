using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.DirectoryServices.Protocols;
using System.Linq;

namespace HansKindberg.DirectoryServices.Protocols.Abstractions
{
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
	public class DirectoryAttributeWrapper : IDirectoryAttribute
	{
		#region Fields

		private readonly DirectoryAttribute _directoryAttribute;

		#endregion

		#region Constructors

		public DirectoryAttributeWrapper(DirectoryAttribute directoryAttribute)
		{
			if(directoryAttribute == null)
				throw new ArgumentNullException("directoryAttribute");

			this._directoryAttribute = directoryAttribute;
		}

		#endregion

		#region Properties

		public virtual int Count
		{
			get { return this.DirectoryAttribute.Count; }
		}

		protected internal virtual DirectoryAttribute DirectoryAttribute
		{
			get { return this._directoryAttribute; }
		}

		public virtual bool IsReadOnly
		{
			get { return ((IList) this.DirectoryAttribute).IsReadOnly; }
		}

		public virtual object this[int index]
		{
			get { return this.DirectoryAttribute[index]; }
			set { this.DirectoryAttribute[index] = value; }
		}

		public virtual string Name
		{
			get { return this.DirectoryAttribute.Name; }
			set { this.DirectoryAttribute.Name = value; }
		}

		#endregion

		#region Methods

		public virtual void Add(object item)
		{
			((IList) this.DirectoryAttribute).Add(item);
		}

		public virtual void AddRange(IEnumerable<object> values)
		{
			if(values == null)
			{
				// ReSharper disable AssignNullToNotNullAttribute
				this.DirectoryAttribute.AddRange(null);
				// ReSharper restore AssignNullToNotNullAttribute
			}
			else
			{
				var valuesAsArray = values as object[];

				this.DirectoryAttribute.AddRange(valuesAsArray ?? values.ToArray());
			}
		}

		public virtual void Clear()
		{
			this.DirectoryAttribute.Clear();
		}

		public virtual bool Contains(object item)
		{
			return this.DirectoryAttribute.Contains(item);
		}

		public virtual void CopyTo(object[] array, int arrayIndex)
		{
			this.DirectoryAttribute.CopyTo(array, arrayIndex);
		}

		public static DirectoryAttributeWrapper FromDirectoryAttribute(DirectoryAttribute directoryAttribute)
		{
			return directoryAttribute;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public virtual IEnumerator<object> GetEnumerator()
		{
			return this.DirectoryAttribute.Cast<object>().GetEnumerator();
		}

		public virtual int IndexOf(object item)
		{
			return this.DirectoryAttribute.IndexOf(item);
		}

		public virtual void Insert(int index, object item)
		{
			((IList) this.DirectoryAttribute).Insert(index, item);
		}

		public virtual bool Remove(object item)
		{
			var remove = this.Contains(item);

			this.DirectoryAttribute.Remove(item);

			return remove;
		}

		public virtual void RemoveAt(int index)
		{
			this.DirectoryAttribute.RemoveAt(index);
		}

		#endregion

		#region Implicit operator

		public static implicit operator DirectoryAttributeWrapper(DirectoryAttribute directoryAttribute)
		{
			return directoryAttribute != null ? new DirectoryAttributeWrapper(directoryAttribute) : null;
		}

		#endregion
	}
}