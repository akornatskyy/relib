using System;
using System.Diagnostics;

namespace ReusableLibrary.Abstractions.Models
{
    public class PagingSettings : IPagingSettings
    {
        public static readonly PagingSettings Default = new PagingSettings()
        {
            PageCount = 5,
            DefaultItemsPerPage = 10,
            PageSizes = new int[] { 10, 25, 50 }
        };

        #region IPagingSettings Members

        public bool AlwaysVisible { get; set; }

        public int DefaultItemsPerPage { get; set; }

        public int[] PageSizes { get; set; }

        public int MaxPageSize
        {
            [DebuggerStepThrough]
            get { return PageSizes[PageSizes.Length - 1]; }
        }

        public int PageCount { get; set; }

        [DebuggerStepThrough]
        public int EnsurePageIndexInRange(int? pageIndex)
        {
            if (!pageIndex.HasValue || pageIndex.Value < 0)
            {
                return 0;
            }

            return pageIndex.Value;
        }

        [DebuggerStepThrough]
        public int EnsurePageSizeInRange(int? pageSize)
        {
            if (!pageSize.HasValue || pageSize <= 0)
            {
                return DefaultItemsPerPage;
            }
            else if (pageSize > MaxPageSize)
            {
                return MaxPageSize;
            }

            return pageSize.Value;
        }

        public int[] AdjustPageSize(int totalItemCount)
        {
            int n = 0;
            while (n < PageSizes.Length)
            {
                if (PageSizes[n++] >= totalItemCount)
                {
                    break;
                }
            }

            if (n <= 1)
            {
                return new int[] { };
            }

            var adjustedPageSize = new int[n];
            Array.Copy(PageSizes, adjustedPageSize, n);
            return adjustedPageSize;
        }

        #endregion
    }
}
