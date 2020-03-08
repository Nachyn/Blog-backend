using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Application.Common.Extensions
{
    public static class QueryableExtensions
    {
        public static IOrderedQueryable<T> AppendOrderBy<T, TKey>(
            this IQueryable<T> query, Expression<Func<T, TKey>> keySelector)
        {
            return query.Expression.Type == typeof(IOrderedQueryable<T>)
                ? ((IOrderedQueryable<T>) query).ThenBy(keySelector)
                : query.OrderBy(keySelector);
        }

        public static IOrderedQueryable<T> AppendOrderByDescending<T, TKey>(
            this IQueryable<T> query, Expression<Func<T, TKey>> keySelector)
        {
            return query.Expression.Type == typeof(IOrderedQueryable<T>)
                ? ((IOrderedQueryable<T>) query).ThenByDescending(keySelector)
                : query.OrderByDescending(keySelector);
        }

        public static IOrderedEnumerable<T> AppendOrderBy<T, TKey>(
            this IEnumerable<T> enumerable, Func<T, TKey> keySelector)
        {
            return enumerable.GetType() == typeof(IOrderedEnumerable<T>)
                ? ((IOrderedEnumerable<T>) enumerable).ThenBy(keySelector)
                : enumerable.OrderBy(keySelector);
        }

        public static IOrderedEnumerable<T> AppendOrderByDescending<T, TKey>(
            this IEnumerable<T> enumerable, Func<T, TKey> keySelector)
        {
            return enumerable.GetType() == typeof(IOrderedEnumerable<T>)
                ? ((IOrderedEnumerable<T>) enumerable).ThenByDescending(keySelector)
                : enumerable.OrderByDescending(keySelector);
        }
    }
}