namespace ReusableLibrary.Abstractions.Models
{
    public interface IPagingSettings
    {
        bool AlwaysVisible { get; }

        int DefaultItemsPerPage { get; }

        int[] PageSizes { get; }

        int MaxPageSize { get; }

        int PageCount { get; }

        int EnsurePageIndexInRange(int? pageIndex);

        int EnsurePageSizeInRange(int? pageSize);

        int[] AdjustPageSize(int totalItemCount);
    }
}
