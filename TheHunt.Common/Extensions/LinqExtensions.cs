using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace TheHunt.Common.Extensions
{
    public static class LinqExtensions
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate)
        {
            return condition ? source.Where(predicate) : source;
        }
    }
}
