using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using HansKindberg.Configuration;

namespace HansKindberg.DirectoryServices.Configuration
{
	public class DirectorySettingsElement : NamedConfigurationElement, IDirectorySettings
	{
		#region Fields

		private IEnumerable<string> _attributes;
		private const char _attributesDelimiter = ',';
		private const string _attributesPropertyName = "attributes";
		private const string _defaultDistinguishedNamePropertyName = "defaultDistinguishedName";
		private const string _defaultHiddenObjectClass = "hiddenObject";
		private const string _defaultObjectClassAttribute = "objectClass";
		private const string _filterPropertyName = "filter";
		private const string _hiddenObjectClassPropertyName = "hiddenObjectClass";
		private const string _identityAttributePropertyName = "identityAttribute";
		private IEnumerable<string> _minimumNumberOfAttributes;
		private static readonly IEnumerable<string> _noExistingAttributes = new[] {string.Empty};
		private const string _objectClassAttributePropertyName = "objectClassAttribute";
		private const string _pageSizePropertyName = "pageSize";
		private const string _sizeLimitPropertyName = "sizeLimit";

		#endregion

		#region Properties

		public virtual IEnumerable<string> Attributes
		{
			get
			{
				if(this._attributes == null)
				{
					var attributes = new List<string>();

					if(this.AttributesInternal != null)
					{
						foreach(var attribute in this.AttributesInternal.Replace(";", this.AttributesDelimiter.ToString(CultureInfo.InvariantCulture)).Split(new[] {this.AttributesDelimiter}, StringSplitOptions.RemoveEmptyEntries).Select(attribute => attribute.Trim()))
						{
							if(!string.IsNullOrEmpty(attribute) && !attributes.Contains(attribute, StringComparer.OrdinalIgnoreCase))
								attributes.Add(attribute);
						}
					}

					if(attributes.Any())
					{
						if(this.IdentityAttribute != null && !attributes.Contains(this.IdentityAttribute, StringComparer.OrdinalIgnoreCase))
							attributes.Add(this.IdentityAttribute);

						if(this.ObjectClassAttribute != null && !attributes.Contains(this.ObjectClassAttribute, StringComparer.OrdinalIgnoreCase))
							attributes.Add(this.ObjectClassAttribute);
					}

					attributes.Sort();

					this._attributes = attributes.ToArray();
				}

				return this._attributes;
			}
			protected internal set { this._attributes = value; }
		}

		protected internal virtual char AttributesDelimiter
		{
			get { return _attributesDelimiter; }
		}

		protected internal virtual string AttributesInternal
		{
			get { return (string) this[this.AttributesPropertyName]; }
			set
			{
				this.Attributes = null;
				this[this.AttributesPropertyName] = value;
			}
		}

		protected internal virtual string AttributesPropertyName
		{
			get { return _attributesPropertyName; }
		}

		public virtual string DefaultDistinguishedName
		{
			get { return (string) this[this.DefaultDistinguishedNamePropertyName]; }
			set { this[this.DefaultDistinguishedNamePropertyName] = value; }
		}

		protected internal virtual string DefaultDistinguishedNamePropertyName
		{
			get { return _defaultDistinguishedNamePropertyName; }
		}

		protected internal virtual string DefaultHiddenObjectClass
		{
			get { return _defaultHiddenObjectClass; }
		}

		protected internal virtual string DefaultObjectClassAttribute
		{
			get { return _defaultObjectClassAttribute; }
		}

		public virtual string Filter
		{
			get { return (string) this[this.FilterPropertyName]; }
			set { this[this.FilterPropertyName] = value; }
		}

		protected internal virtual string FilterPropertyName
		{
			get { return _filterPropertyName; }
		}

		public virtual string HiddenObjectClass
		{
			get { return (string) this[this.HiddenObjectClassPropertyName]; }
			set { this[this.HiddenObjectClassPropertyName] = value; }
		}

		protected internal virtual string HiddenObjectClassPropertyName
		{
			get { return _hiddenObjectClassPropertyName; }
		}

		public virtual string IdentityAttribute
		{
			get { return (string) this[this.IdentityAttributePropertyName]; }
			set
			{
				this.MinimumNumberOfAttributes = null;
				this[this.IdentityAttributePropertyName] = value;
			}
		}

		protected internal virtual string IdentityAttributePropertyName
		{
			get { return _identityAttributePropertyName; }
		}

		protected internal virtual IEnumerable<string> MinimumNumberOfAttributes
		{
			get
			{
				if(this._minimumNumberOfAttributes == null)
				{
					var minimumNumberOfAttributes = new List<string>();

					if(this.IdentityAttribute != null)
						minimumNumberOfAttributes.Add(this.IdentityAttribute);

					if(this.ObjectClassAttribute != null)
						minimumNumberOfAttributes.Add(this.ObjectClassAttribute);

					if(!minimumNumberOfAttributes.Any())
						minimumNumberOfAttributes.AddRange(this.NoExistingAttributes);

					this._minimumNumberOfAttributes = minimumNumberOfAttributes.ToArray();
				}

				return this._minimumNumberOfAttributes;
			}
			set { this._minimumNumberOfAttributes = value; }
		}

		protected internal virtual IEnumerable<string> NoExistingAttributes
		{
			get { return _noExistingAttributes; }
		}

		public virtual string ObjectClassAttribute
		{
			get { return (string) this[this.ObjectClassAttributePropertyName]; }
			set
			{
				this.MinimumNumberOfAttributes = null;
				this[this.ObjectClassAttributePropertyName] = value;
			}
		}

		protected internal virtual string ObjectClassAttributePropertyName
		{
			get { return _objectClassAttributePropertyName; }
		}

		public virtual int? PageSize
		{
			get { return (int?) this[this.PageSizePropertyName]; }
			set { this[this.PageSizePropertyName] = value; }
		}

		protected internal virtual string PageSizePropertyName
		{
			get { return _pageSizePropertyName; }
		}

		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				if(!this.Initialized)
				{
					if(!base.Properties.Contains(this.AttributesPropertyName))
						base.Properties.Add(this.CreateStringConfigurationProperty(this.AttributesPropertyName, new RegexStringValidator("^[0-9a-zA-Z, ]+$")));

					if(!base.Properties.Contains(this.DefaultDistinguishedNamePropertyName))
						base.Properties.Add(this.CreateStringConfigurationProperty(this.DefaultDistinguishedNamePropertyName));

					if(!base.Properties.Contains(this.FilterPropertyName))
						base.Properties.Add(this.CreateStringConfigurationProperty(this.FilterPropertyName));

					if(!base.Properties.Contains(this.HiddenObjectClassPropertyName))
						base.Properties.Add(this.CreateStringConfigurationProperty(this.HiddenObjectClassPropertyName, this.DefaultHiddenObjectClass));

					if(!base.Properties.Contains(this.IdentityAttributePropertyName))
						base.Properties.Add(this.CreateStringConfigurationProperty(this.IdentityAttributePropertyName));

					if(!base.Properties.Contains(this.ObjectClassAttributePropertyName))
						base.Properties.Add(this.CreateStringConfigurationProperty(this.ObjectClassAttributePropertyName, this.DefaultObjectClassAttribute));

					if(!base.Properties.Contains(this.PageSizePropertyName))
						base.Properties.Add(this.CreateNullableIntegerConfigurationProperty(this.PageSizePropertyName));

					if(!base.Properties.Contains(this.SizeLimitPropertyName))
						base.Properties.Add(this.CreateNullableIntegerConfigurationProperty(this.SizeLimitPropertyName));
				}

				return base.Properties;
			}
		}

		public virtual int? SizeLimit
		{
			get { return (int?) this[this.SizeLimitPropertyName]; }
			set { this[this.SizeLimitPropertyName] = value; }
		}

		protected internal virtual string SizeLimitPropertyName
		{
			get { return _sizeLimitPropertyName; }
		}

		#endregion

		#region Methods

		protected internal virtual ConfigurationProperty CreateNullableIntegerConfigurationProperty(string name)
		{
			return new ConfigurationProperty(name, typeof(int?), null);
		}

		protected internal virtual ConfigurationProperty CreateStringConfigurationProperty(string name)
		{
			return this.CreateStringConfigurationProperty(name, (object) null);
		}

		protected internal virtual ConfigurationProperty CreateStringConfigurationProperty(string name, object defaultValue)
		{
			return this.CreateStringConfigurationProperty(name, defaultValue, new RegexStringValidator("^[0-9a-zA-Z]+$"));
		}

		protected internal virtual ConfigurationProperty CreateStringConfigurationProperty(string name, ConfigurationValidatorBase validator)
		{
			return this.CreateStringConfigurationProperty(name, null, validator);
		}

		protected internal virtual ConfigurationProperty CreateStringConfigurationProperty(string name, object defaultValue, ConfigurationValidatorBase validator)
		{
			return new ConfigurationProperty(name, typeof(string), defaultValue, null, validator, ConfigurationPropertyOptions.None);
		}

		protected internal virtual IEnumerable<string> FilterAttributes(IEnumerable<string> attributes)
		{
			if(this.Attributes == null || !this.Attributes.Any())
				return attributes;

			// ReSharper disable PossibleMultipleEnumeration

			if(attributes == null || !attributes.Any())
				return this.Attributes;

			var filteredAttributes = attributes.Where(attribute => this.Attributes.Contains(attribute, StringComparer.OrdinalIgnoreCase)).ToArray();

			// ReSharper restore PossibleMultipleEnumeration

			if(!filteredAttributes.Any())
				return this.NoExistingAttributes;

			return filteredAttributes;
		}

		public virtual IEnumerable<string> GetAttributes(IEnumerable<string> attributes, AttributesSetting attributesSetting)
		{
			switch(attributesSetting)
			{
				case AttributesSetting.Identity:
					return this.GetAttributes(this.IdentityAttribute);
				case AttributesSetting.Minimum:
					return this.MinimumNumberOfAttributes;
				case AttributesSetting.None:
					return this.NoExistingAttributes;
				default:
					return this.FilterAttributes(attributes);
			}
		}

		protected internal virtual IEnumerable<string> GetAttributes(string attribute)
		{
			var attributes = new List<string>();

			if(string.IsNullOrEmpty(attribute))
				attributes.AddRange(this.NoExistingAttributes);
			else
				attributes.Add(attribute);

			return attributes.ToArray();
		}

		#endregion
	}
}