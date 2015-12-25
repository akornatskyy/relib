using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Supplemental.Collections
{
    public sealed class PagedOffsetList<T> : PagedList<T>
    {
        public PagedOffsetList(IEnumerable<T> source, int pageIndex, int pageSize, bool hasMore)
            : this(AsQueryable(source), pageIndex, pageSize, 0, hasMore)
        {
        }

        public PagedOffsetList(IEnumerable<T> source, int pageIndex, int pageSize, int pageIndexOffset, bool hasMore)
            : this(AsQueryable(source), pageIndex, pageSize, pageIndexOffset, hasMore)
        {
        }

        public PagedOffsetList(IQueryable<T> source, int pageIndex, int pageSize, bool hasMore)
            : this(source, pageIndex, pageSize, 0, hasMore)
        {
        }

        public PagedOffsetList(IQueryable<T> source, int pageIndex, int pageSize, int pageIndexOffset, bool hasMore)
        {
            if (pageIndexOffset > pageIndex)
            {
                throw new ArgumentOutOfRangeException("pageIndexOffset", "PageIndexOffest can not be greater than PageIndex");
            }

            if (source == null)
            {
                source = new List<T>().AsQueryable();
            }

            State = new PagedListState(pageIndex, pageSize, source.Count() + (pageIndexOffset * pageSize), hasMore);
            if (State.TotalItemCount > 0)
            {
                AddRange(source.Skip((pageIndex - pageIndexOffset) * pageSize).Take(pageSize).ToList());
            }
        }
    }
}
