using System.Collections.Generic;
using System.Linq;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Supplemental.Collections
{
    public class PagedList<T> : List<T>, IPagedList<T>
    {
        public PagedList(IEnumerable<T> source, int index, int pageSize)
            : this(AsQueryable(source), index, pageSize)
        {
        }

        public PagedList(IEnumerable<T> source, int index, int pageSize, bool hasMore)
            : this(AsQueryable(source), index, pageSize, hasMore)
        {
        }

        public PagedList(IQueryable<T> source, int index, int pageSize)
            : this(source, index, pageSize, false)
        {
        }

        public PagedList(IQueryable<T> source, int index, int pageSize, bool hasMore)
        {
            if (source == null)
            {
                source = new List<T>().AsQueryable();
            }

            State = new PagedListState(index, pageSize, source.Count(), hasMore);
            if (State.TotalItemCount > 0)
            {
                AddRange(source.Skip(State.PageIndex * State.PageSize).Take(State.PageSize).ToList());
            }
        }

        protected PagedList()
        {
        }

        #region IPagedList<T> Members

        public IPagedListState State { get; protected set; }

        #endregion

        protected static IQueryable<T> AsQueryable(IEnumerable<T> source)
        {
            var queryable = source as IQueryable<T>;
            if (queryable == null)
            {
                queryable = source.AsQueryable();
            }

            return queryable;
        }
    }
}
