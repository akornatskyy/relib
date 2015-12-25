using System;
using System.Diagnostics;
using System.Linq;
using ReusableLibrary.Abstractions.Models;
using ReusableLibrary.Supplemental.Models;

namespace ReusableLibrary.Supplemental.Collections
{
    public static class QueryableExtensions
    {
        [DebuggerStepThrough]
        public static IQueryable<TResult> SatisfyPaging<TResult>(this IQueryable<TResult> queryable, int pageIndex, int pageSize)
        {
            if (pageSize == Int32.MaxValue)
            {
                throw new ArgumentOutOfRangeException("pageSize");
            }

            checked
            {
                return queryable.Skip(pageIndex * pageSize)
                    .Take(pageSize + 1);
            }
        }

        [DebuggerStepThrough]
        public static IQueryable<TResult> Satisfy<TResult>(this IQueryable<TResult> source, ISpecification<TResult> specification)
        {
            return source.Where(specification.IsSatisfied());
        }

        [DebuggerStepThrough]
        public static IPagedList<T> ToPagedList<T>(this IQueryable<T> source, int index, int pageSize)
        {
            return new PagedList<T>(source, index, pageSize);
        }

        [DebuggerStepThrough]
        public static IPagedList<T> ToPagedList<T>(this IQueryable<T> source, int index, int pageSize, bool hasMore)
        {
            return new PagedList<T>(source, index, pageSize, hasMore);
        }

        [DebuggerStepThrough]
        public static IPagedList<T> ToPagedList<T>(this IQueryable<T> source, int index, int pageSize, int pageIndexOffset, bool hasMore)
        {
            return new PagedOffsetList<T>(source, index, pageSize, pageIndexOffset, hasMore);
        }
    }
}
