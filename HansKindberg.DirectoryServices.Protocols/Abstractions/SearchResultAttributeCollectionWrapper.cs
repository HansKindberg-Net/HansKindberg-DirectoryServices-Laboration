using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.DirectoryServices.Protocols;

namespace HansKindberg.DirectoryServices.Protocols.Abstractions
{
	[SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
	public class SearchResultAttributeCollectionWrapper : IEnumerable<IDirectoryAttribute>
	{
		#region Fields

		private readonly SearchResultAttributeCollection _searchResultAttributeCollection;

		#endregion

		#region Constructors

		public SearchResultAttributeCollectionWrapper(SearchResultAttributeCollection searchResultAttributeCollection)
		{
			if(searchResultAttributeCollection == null)
				throw new ArgumentNullException("searchResultAttributeCollection");

			this._searchResultAttributeCollection = searchResultAttributeCollection;
		}

		#endregion

		#region Properties

		protected internal virtual SearchResultAttributeCollection SearchResultAttributeCollection
		{
			get { return this._searchResultAttributeCollection; }
		}

		#endregion

		#region Methods

		public static SearchResultAttributeCollectionWrapper FromSearchResultAttributeCollection(SearchResultAttributeCollection searchResultAttributeCollection)
		{
			return searchResultAttributeCollection;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public virtual IEnumerator<IDirectoryAttribute> GetEnumerator()
		{
			var attributes = new List<IDirectoryAttribute>();

			// ReSharper disable PossibleNullReferenceException
			// ReSharper disable LoopCanBeConvertedToQuery
			foreach(string attributeName in this.SearchResultAttributeCollection.AttributeNames)
			{
				attributes.Add((DirectoryAttributeWrapper) this.SearchResultAttributeCollection[attributeName]);
			}
			// ReSharper restore LoopCanBeConvertedToQuery
			// ReSharper restore PossibleNullReferenceException

			return attributes.GetEnumerator();
		}

		#endregion

		#region Implicit operator

		public static implicit operator SearchResultAttributeCollectionWrapper(SearchResultAttributeCollection searchResultAttributeCollection)
		{
			return searchResultAttributeCollection != null ? new SearchResultAttributeCollectionWrapper(searchResultAttributeCollection) : null;
		}

		#endregion
	}
}