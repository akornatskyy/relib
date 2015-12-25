using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Abstractions.Repository;
using ReusableLibrary.Supplemental.Collections;

namespace ReusableLibrary.Supplemental.Repository
{
    public static class RetrieveMultipleRepositoryIntegration
    {
        public static IPagedList<TResult> RetrieveMultiple<TSpecification, TResult>(
            this IRetrieveMultipleRepository<TSpecification, TResult> repository,
            TSpecification specification,
            int? pageIndex,
            int? pageSize,
            IPagingSettings pagingSettings)
        {
            int pageIndexInRange = pagingSettings.EnsurePageIndexInRange(pageIndex);
            int pageSizeInRange = pagingSettings.EnsurePageSizeInRange(pageSize);

            var request = new RetrieveMultipleRequest<TSpecification>(specification)
            {
                PageIndex = pageIndexInRange / pagingSettings.PageCount,
                PageSize = pageSizeInRange * pagingSettings.PageCount
            };

            var result = repository.RetrieveMultiple(request);

            var pageIndexOffset = (request.PageIndex * request.PageSize) / pageSizeInRange;
            var pagedList = result.Items.ToPagedList(pageIndexInRange, pageSizeInRange, pageIndexOffset, result.HasMore);
            pagedList.State.Settings = pagingSettings;
            return pagedList;
        }
    }
}
