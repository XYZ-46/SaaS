using System.Linq.Expressions;

namespace DataEntity.Pagination
{
    public static class QueryExtension
    {
        public static IQueryable<TModel> OrderByQuery<TModel>(this IQueryable<TModel> query, string SortField, bool Ascending)
        {
            var param = Expression.Parameter(typeof(TModel));
            var prop = Expression.Property(param, SortField);
            var exp = Expression.Lambda(prop, param);

            // Jika sudah ada order
            string methodOrder;
            if (query.Expression.Type == typeof(IOrderedQueryable<TModel>)) // sudah ada orderby
                methodOrder = Ascending ? "ThenBy" : "ThenByDescending";
            else // order blm ada sama sekali
                methodOrder = Ascending ? "OrderBy" : "OrderByDescending";

            Type[] types = [query.ElementType, exp.Body.Type];
            var mce = Expression.Call(typeof(Queryable), methodOrder, types, query.Expression, exp);
            return query.Provider.CreateQuery<TModel>(mce);
        }
    }
}

