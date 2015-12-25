using System;

namespace ReusableLibrary.Abstractions.Models
{
    public interface IPagedListState
    {
        IPagingSettings Settings { get; set; }

        int PageCount { get; }

        int TotalItemCount { get; }

        int PageIndex { get; }

        int PageNumber { get; }

        int PageSize { get; }

        bool HasItems { get; }

        bool HasPreviousPage { get; }

        bool HasNextPage { get; }

        bool HasMore { get; }

        bool IsFirstPage { get; }

        bool IsLastPage { get; }
    }
}
