using System.Linq.Expressions;

namespace DataEntity.Pagination
{
    public static class PaginationQueryExtension
    {
        public static IQueryable<TModel> OrderByQuery<TModel>(this IQueryable<TModel> query, string SortField, bool Ascending)
        {
            var param = Expression.Parameter(typeof(TModel), "p");
            var prop = Expression.Property(param, SortField);
            var exp = Expression.Lambda(prop, param);
            string method = Ascending ? "OrderBy" : "OrderByDescending";
            Type[] types = [query.ElementType, exp.Body.Type];
            var mce = Expression.Call(typeof(Queryable), method, types, query.Expression, exp);
            return query.Provider.CreateQuery<TModel>(mce);
        }
    }
}
