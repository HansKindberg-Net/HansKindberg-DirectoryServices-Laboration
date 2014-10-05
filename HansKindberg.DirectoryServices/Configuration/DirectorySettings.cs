using System;
using System.Collections.Generic;

namespace HansKindberg.DirectoryServices.Configuration
{
	public abstract class DirectorySettings : IDirectorySettings
	{
		#region Fields

		private const string _hiddenObjectClass = "hiddenObject";
		private static readonly IEnumerable<string> _noExistingAttributes = new[] {"4829CE14-DB87-4958-926E-981472C92B94"};
		private const string _objectClassAttributeName = "objectClass";
		private int? _pageSize;
		private int? _sizeLimit;

		#endregion

		#region Properties

		public virtual string Filter { get; set; }

		public virtual string HiddenObjectClass
		{
			get { return _hiddenObjectClass; }
		}

		public abstract string IdentityAttributeName { get; }
		public abstract IEnumerable<string> IdentityAttributes { get; }
		public abstract IEnumerable<string> MinimumNumberOfAttributes { get; }

		public virtual IEnumerable<string> NoExistingAttributes
		{
			get { return _noExistingAttributes; }
		}

		public virtual string ObjectClassAttributeName
		{
			get { return _objectClassAttributeName; }
		}

		public virtual int? PageSize
		{
			get { return this._pageSize; }
			set
			{
				if(value != null & value < 0)
					throw new ArgumentException("The page-size can not be less than zero.");

				this._pageSize = value;
			}
		}

		public virtual int? SizeLimit
		{
			get { return this._sizeLimit; }
			set
			{
				if(value != null & value < 0)
					throw new ArgumentException("The size-limit can not be less than zero.");

				this._sizeLimit = value;
			}
		}

		#endregion
	}
}