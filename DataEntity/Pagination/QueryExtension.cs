using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

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

        static Expression GetExpresion(MemberExpression prop, string value, OperatorEnm op)
        {
            TypeConverter conv = TypeDescriptor.GetConverter(prop.Type);
            var paramValue = (prop.Type.Name == "String") ? value : conv.ConvertFrom(value);

            ConstantExpression valueExp = Expression.Constant(paramValue);
            Expression exp = Expression.Default(prop.Type);
            switch (op)
            {
                case OperatorEnm.Equal:
                    exp = Expression.Equal(prop, Expression.Convert(valueExp, prop.Type));
                    break;
                case OperatorEnm.NotEqual:
                    exp = Expression.NotEqual(prop, Expression.Convert(valueExp, prop.Type));
                    break;
                case OperatorEnm.GreaterThan:
                case OperatorEnm.NotGreaterThan:
                    exp = Expression.GreaterThan(prop, Expression.Convert(valueExp, prop.Type));
                    break;
                case OperatorEnm.GreaterThanOrEqual:
                case OperatorEnm.NotGreaterThanOrEqual:
                    exp = Expression.GreaterThanOrEqual(prop, Expression.Convert(valueExp, prop.Type));
                    break;

                case OperatorEnm.LessThan:
                case OperatorEnm.NotLessThan:
                    exp = Expression.LessThan(prop, Expression.Convert(valueExp, prop.Type));
                    break;

                case OperatorEnm.LessThanOrEqual:
                case OperatorEnm.NotLessThanOrEqual:
                    exp = Expression.LessThanOrEqual(prop, Expression.Convert(valueExp, prop.Type));
                    break;
            }
            return exp;
        }

        static ConstantExpression GetConstantExpression(ParameterExpression param, string propertyName, string propertyValue)
        {
            MemberExpression prop = Expression.Property(param, propertyName);
            TypeConverter conv = TypeDescriptor.GetConverter(prop.Type);
            var paramValue = (prop.Type.Name == "String") ? propertyValue : conv.ConvertFrom(propertyValue);

            return Expression.Constant(paramValue);
        }

        public static IQueryable<TModel> FilterQuery<TModel>(this IQueryable<TModel> query, FilterCriteria filterCriteria)
        {
            if (filterCriteria.Operator == OperatorEnm.Between)
                throw new ArgumentException($"Invalid operator {filterCriteria.Operator} for FilterQuery");

            ParameterExpression param = Expression.Parameter(typeof(TModel));
            MemberExpression prop = Expression.Property(param, filterCriteria.PropertyName);

            var paramExpression = GetExpresion(prop, filterCriteria.PropertyValue, filterCriteria.Operator);

            MethodInfo method;
            switch (filterCriteria.Operator)
            {
                case OperatorEnm.Contain:
                    method = prop.Type.GetMethod("Contains", [prop.Type]);
                    paramExpression = Expression.Call(prop, method, GetConstantExpression(param, filterCriteria.PropertyName, filterCriteria.PropertyValue));
                    break;
                case OperatorEnm.NotContain:
                    method = prop.Type.GetMethod("Contains", [prop.Type]);
                    paramExpression = Expression.Call(prop, method, GetConstantExpression(param, filterCriteria.PropertyName, filterCriteria.PropertyValue));
                    paramExpression = Expression.Not(paramExpression);
                    break;

                case OperatorEnm.StartWith:
                    method = prop.Type.GetMethod("StartsWith", [prop.Type]);
                    paramExpression = Expression.Call(prop, method, GetConstantExpression(param, filterCriteria.PropertyName, filterCriteria.PropertyValue));
                    break;
                case OperatorEnm.NotStartWith:
                    method = prop.Type.GetMethod("StartsWith", [prop.Type]);
                    paramExpression = Expression.Call(prop, method, GetConstantExpression(param, filterCriteria.PropertyName, filterCriteria.PropertyValue));
                    paramExpression = Expression.Not(paramExpression);
                    break;

                case OperatorEnm.EndWith:
                    method = prop.Type.GetMethod("EndsWith", [prop.Type]);
                    paramExpression = Expression.Call(prop, method, GetConstantExpression(param, filterCriteria.PropertyName, filterCriteria.PropertyValue));
                    break;
                case OperatorEnm.NotEndWith:
                    method = prop.Type.GetMethod("EndsWith", [prop.Type]);
                    paramExpression = Expression.Call(prop, method, GetConstantExpression(param, filterCriteria.PropertyName, filterCriteria.PropertyValue));
                    paramExpression = Expression.Not(paramExpression);
                    break;

                case OperatorEnm.NotGreaterThan:
                    paramExpression = Expression.Not(paramExpression);
                    break;

                case OperatorEnm.NotGreaterThanOrEqual:
                    paramExpression = Expression.Not(paramExpression);
                    break;

                case OperatorEnm.NotLessThan:
                    paramExpression = Expression.Not(paramExpression);
                    break;

                case OperatorEnm.NotLessThanOrEqual:
                    paramExpression = Expression.Not(paramExpression);
                    break;

                case OperatorEnm.InSet:
                    break;
                case OperatorEnm.NotInSet:
                    break;
            }

            var argExpression = Expression.Lambda<Func<TModel, bool>>(paramExpression, param);

            Type[] types = [query.ElementType];
            var mce = Expression.Call(typeof(Queryable), "Where", types, query.Expression, argExpression);
            return query.Provider.CreateQuery<TModel>(mce);
        }

    }
}

