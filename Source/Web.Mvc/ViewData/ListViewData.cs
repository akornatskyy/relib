using System;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Web.Mvc
{
    [Serializable]
    public class ListViewData<TSpecification, TResult> : SearchViewData<TSpecification>, IListViewData
        where TSpecification : new()
    {
        public ListViewData()
        {
        }

        public ListViewData(TSpecification specification)
            : base(specification)
        {
        }

        public IPagedList<TResult> Items { get; set; }

        #region IListViewData Members

        object IListViewData.Specification
        {
            get
            {
                return this.Specification;
            }
        }

        IPagedListState IListViewData.Items
        {
            get
            {
                return this.Items.State;
            }
        }

        #endregion
    }
}
