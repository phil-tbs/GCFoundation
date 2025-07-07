using System.Linq.Dynamic.Core;

namespace GCFoundation.Components.Extensions
{
    /// <summary>
    /// Provides extension methods for dynamically filtering and sorting IQueryable sources.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Dynamically sorts an <see cref="IQueryable{T}"/> by the specified field name and sort direction.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the source.</typeparam>
        /// <param name="source">The source IQueryable to sort.</param>
        /// <param name="field">The name of the field to sort by.</param>
        /// <param name="ascending">Determines whether to sort in ascending (true) or descending (false) order.</param>
        /// <returns>A sorted <see cref="IQueryable{T}"/>.</returns>
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string field, bool ascending)
        {
            var sort = ascending ? "ascending" : "descending";
            return source.OrderBy($"{field} {sort}");
        }

        /// <summary>
        /// Dynamically filters an <see cref="IQueryable{T}"/> where the specified field contains the given value.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the source.</typeparam>
        /// <param name="source">The source IQueryable to filter.</param>
        /// <param name="field">The name of the field to apply the filter on.</param>
        /// <param name="value">The value to search for using a Contains operation.</param>
        /// <returns>A filtered <see cref="IQueryable{T}"/>.</returns>
        public static IQueryable<T> WhereDynamic<T>(this IQueryable<T> source, string field, string value)
        {
            return source.Where($"{field}.Contains(@0)", value);
        }
    }
}
