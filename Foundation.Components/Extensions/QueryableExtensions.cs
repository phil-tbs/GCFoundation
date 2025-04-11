using System.Linq.Dynamic.Core;

namespace Foundation.Components.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string field, bool ascending)
        {
            var sort = ascending ? "ascending" : "descending";
            return source.OrderBy($"{field} {sort}");
        }

        public static IQueryable<T> WhereDynamic<T>(this IQueryable<T> source, string field, string value)
        {
            return source.Where($"{field}.Contains(@0)", value);
        }
    }
}
