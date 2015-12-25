using System;
using System.Linq.Expressions;

namespace ReusableLibrary.Supplemental.Models
{
    public interface ISpecification<TResult>
    {
        Expression<Func<TResult, bool>> IsSatisfied();
    }
}
