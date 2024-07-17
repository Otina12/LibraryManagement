using Library.Model.Models;
using System.Linq.Expressions;

namespace Library.Service.Helpers
{
    public static class DataFilters
    {
        public static IQueryable<T> ApplySort<T>(
            this IQueryable<T> collection, string sortBy, string sortOrder,
            IDictionary<string, Expression<Func<T, object>>> sortDictionary)
        {
            if (string.IsNullOrEmpty(sortBy) || !sortDictionary.ContainsKey(sortBy))
            {
                return collection;
            }

            var selector = sortDictionary[sortBy];

            if (sortOrder == "desc")
            {
                return collection.OrderByDescending(selector);
            }
            else if (sortOrder == "asc")
            {
                return collection.OrderBy(selector);
            }

            return collection;
        }

        public static IEnumerable<T> ApplyPagination<T>(this IQueryable<T> collection, int pageNumber, int pageSize)
        {
            pageNumber = Math.Max(1, pageNumber);
            return collection.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }

        public static IQueryable<T> ApplySearch<T>(this IQueryable<T> collection, string searchString, Func<T, string>[] searchProperties)
        {
            if (string.IsNullOrEmpty(searchString) || searchProperties.Length == 0)
            {
                return collection;
            }

            return collection
                .AsEnumerable()
                .Where(item => searchProperties.Any(prop =>
                prop(item).Contains(searchString) == true))
                .AsQueryable();
        }

        public static IQueryable<T> IncludeDeleted<T>(this IQueryable<T> query, bool includeDeleted) where T : BaseModel
        {
            return includeDeleted ? query : query.Where(x => x.DeleteDate == null);
        }
    }
}
