using System.Collections.Generic;

namespace HansKindberg.DirectoryServices.Protocols
{
	public class SearchOptions : ISearchOptions
	{
		#region Properties

		public virtual IEnumerable<string> Attributes { get; set; }
		public virtual AttributesSetting AttributesSetting { get; set; }
		public virtual string Filter { get; set; }

		#endregion
	}
}