using System;
using System.Diagnostics;

namespace ReusableLibrary.Abstractions.Models
{
    public class PagedListState : IPagedListState
    {
        public PagedListState()
        {
            Settings = PagingSettings.Default;
        }

        public PagedListState(int index, int pageSize, int totalItemCount, bool hasMore)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index", "PageIndex cannot be below 0");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException("pageSize", "PageSize cannot be less than 1");
            }

            PageIndex = index;
            PageSize = pageSize;            
            HasMore = hasMore;
            TotalItemCount = totalItemCount;
            if (TotalItemCount > 0)
            {
                PageCount = (int)Math.Ceiling(TotalItemCount / (double)PageSize);
                if (HasMore)
                {
                    PageCount++;
                }
            }

            Settings = PagingSettings.Default;
        }

        #region IPagedListState Members

        public IPagingSettings Settings { get; set; }

        public int PageCount { get; private set; }

        public int TotalItemCount { get; private set; }

        public int PageIndex { get; private set; }

        public int PageSize { get; private set; }

        public bool HasMore { get; private set; }

        public int PageNumber
        {
            [DebuggerStepThrough]
            get
            {
                return PageIndex + 1;
            }
        }

        public bool HasItems
        {
            [DebuggerStepThrough]
            get
            {
                return TotalItemCount > 0;
            }
        }

        public bool HasPreviousPage
        {
            [DebuggerStepThrough]
            get
            {
                return PageIndex > 0;
            }
        }

        public bool HasNextPage
        {
            [DebuggerStepThrough]
            get
            {
                return PageIndex < (PageCount - 1);
            }
        }

        public bool IsFirstPage
        {
            [DebuggerStepThrough]
            get
            {
                return PageIndex <= 0;
            }
        }

        public bool IsLastPage
        {
            [DebuggerStepThrough]
            get
            {
                return PageIndex >= (PageCount - 1);
            }
        }

        #endregion
    }
}
