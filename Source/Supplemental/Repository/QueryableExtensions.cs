using System.Collections.Generic;
using System.Linq;
using ReusableLibrary.Abstractions.Repository;

namespace ReusableLibrary.Supplemental.Repository
{
    public static class QueryableExtensions
    {
        public static RetrieveMultipleResponse<TResult> ToResponse<TResult>(
            this IQueryable<TResult> queryable, int pageSize)
        {
            var result = queryable.ToList();
            return ToResponse(result, pageSize);
        }

        public static RetrieveMultipleResponse<TResult> ToResponse<TResult>(
            this IEnumerable<TResult> enumerable, int pageSize)
        {
            var result = enumerable as IList<TResult>;
            if (result == null)
            {
                result = enumerable.ToList();
            }

            var hasMore = result.Count > pageSize;            
            if (hasMore)
            {
                result.RemoveAt(pageSize);
            }

            return new RetrieveMultipleResponse<TResult>(result, hasMore);
        }
    }
}
