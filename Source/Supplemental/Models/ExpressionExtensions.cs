using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ReusableLibrary.Supplemental.Collections;

namespace ReusableLibrary.Supplemental.Models
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T1, bool>> OrOneOf<T1, T2>(this Expression<Func<T1, bool>> expr,
            IEnumerable<T2> items, Func<T2, Expression<Func<T1, bool>>> func)
        {
            var expr2 = OneOf(items, func);
            if (expr2 == null)
            {
                return expr;
            }

            return Or(expr, expr2);
        }

        public static Expression<Func<T1, bool>> AndOneOf<T1, T2>(this Expression<Func<T1, bool>> expr,
            IEnumerable<T2> items, Func<T2, Expression<Func<T1, bool>>> func)
        {
            var expr2 = OneOf(items, func);
            if (expr2 == null)
            {
                return expr;
            }

            return And(expr, expr2);
        }

        private static Expression<Func<T1, bool>> OneOf<T1, T2>(IEnumerable<T2> items, Func<T2, Expression<Func<T1, bool>>> func)
        {
            Expression<Func<T1, bool>> expr = null;
            items.Translate(a => func(a)).ForEach(x => expr = expr == null ? x : expr.Or(x));
            return expr;
        }
    }
}
