using System.Linq.Expressions;

namespace API.Pagination
{
    public static class OrderBy
    {
        public static IQueryable<T> OrderByField<T>(this IQueryable<T> query, string SortField, bool Ascending)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(param, SortField);
            var exp = Expression.Lambda(prop, param);
            string method = Ascending ? "OrderBy" : "OrderByDescending";
            Type[] types = [query.ElementType, exp.Body.Type];
            var mce = Expression.Call(typeof(Queryable), method, types, query.Expression, exp);
            return query.Provider.CreateQuery<T>(mce);
        }
    }
}
