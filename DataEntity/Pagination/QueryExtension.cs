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

        static ConstantExpression GetConstantExpresion(ParameterExpression paramEx, string propertyName, string value)
        {
            var prop = Expression.Property(paramEx, propertyName);

            TypeConverter conv = TypeDescriptor.GetConverter(prop.Type);
            var paramValue = (prop.Type.Name == "String") ? value : conv.ConvertFrom(value);

            ConstantExpression valueExp = Expression.Constant(paramValue);
            return valueExp;
        }

        public static IQueryable<TModel> FilterQuery<TModel>(this IQueryable<TModel> query, SearchCriteria search)
        {
            var param = Expression.Parameter(typeof(TModel));
            var prop = Expression.Property(param, search.PropertyName);


            Expression paramExpression = Expression.Default(prop.Type);

            MethodInfo method;
            switch (search.GetOperator())
            {
                case OperatorEnm.Equal:
                    ConstantExpression valueExp = GetConstantExpresion(param, search.PropertyName, search.Value);
                    paramExpression = Expression.Equal(prop, Expression.Convert(valueExp, prop.Type));
                    break;
                case OperatorEnm.NotEqual:
                    valueExp = GetConstantExpresion(param, search.PropertyName, search.Value);
                    paramExpression = Expression.NotEqual(prop, Expression.Convert(valueExp, prop.Type));
                    break;

                case OperatorEnm.Contain:
                    method = prop.Type.GetMethod("Contains", [prop.Type]);
                    valueExp = GetConstantExpresion(param, search.PropertyName, search.Value);
                    paramExpression = Expression.Call(prop, method, valueExp);
                    break;
                case OperatorEnm.NotContain:
                    valueExp = GetConstantExpresion(param, search.PropertyName, search.Value);
                    method = prop.Type.GetMethod("Contains", [prop.Type]);
                    paramExpression = Expression.Call(prop, method, valueExp);
                    paramExpression = Expression.Not(paramExpression);
                    break;

                case OperatorEnm.StartWith:
                    valueExp = GetConstantExpresion(param, search.PropertyName, search.Value);
                    method = prop.Type.GetMethod("StartsWith", [prop.Type]);
                    paramExpression = Expression.Call(prop, method, valueExp);
                    break;
                case OperatorEnm.NotStartWith:
                    valueExp = GetConstantExpresion(param, search.PropertyName, search.Value);
                    method = prop.Type.GetMethod("StartsWith", [prop.Type]);
                    paramExpression = Expression.Call(prop, method, valueExp);
                    paramExpression = Expression.Not(paramExpression);
                    break;

                case OperatorEnm.EndWith:
                    valueExp = GetConstantExpresion(param, search.PropertyName, search.Value);
                    method = prop.Type.GetMethod("EndsWith", [prop.Type]);
                    paramExpression = Expression.Call(prop, method, valueExp);
                    break;
                case OperatorEnm.NotEndWith:
                    valueExp = GetConstantExpresion(param, search.PropertyName, search.Value);
                    method = prop.Type.GetMethod("EndsWith", [prop.Type]);
                    paramExpression = Expression.Call(prop, method, valueExp);
                    paramExpression = Expression.Not(paramExpression);
                    break;

                case OperatorEnm.GreaterThan:
                    valueExp = GetConstantExpresion(param, search.PropertyName, search.Value);
                    paramExpression = Expression.GreaterThan(prop, Expression.Convert(valueExp, prop.Type));
                    break;
                case OperatorEnm.NotGreaterThan:
                    valueExp = GetConstantExpresion(param, search.PropertyName, search.Value);
                    paramExpression = Expression.GreaterThan(prop, Expression.Convert(valueExp, prop.Type));
                    paramExpression = Expression.Not(paramExpression);
                    break;

                case OperatorEnm.GreaterThanOrEqual:
                    valueExp = GetConstantExpresion(param, search.PropertyName, search.Value);
                    paramExpression = Expression.GreaterThanOrEqual(prop, Expression.Convert(valueExp, prop.Type));
                    break;
                case OperatorEnm.NotGreaterThanOrEqual:
                    valueExp = GetConstantExpresion(param, search.PropertyName, search.Value);
                    paramExpression = Expression.GreaterThanOrEqual(prop, Expression.Convert(valueExp, prop.Type));
                    paramExpression = Expression.Not(paramExpression);
                    break;

                case OperatorEnm.LessThan:
                    valueExp = GetConstantExpresion(param, search.PropertyName, search.Value);
                    paramExpression = Expression.LessThan(prop, Expression.Convert(valueExp, prop.Type));
                    break;
                case OperatorEnm.NotLessThan:
                    valueExp = GetConstantExpresion(param, search.PropertyName, search.Value);
                    paramExpression = Expression.LessThan(prop, Expression.Convert(valueExp, prop.Type));
                    paramExpression = Expression.Not(paramExpression);
                    break;

                case OperatorEnm.LessThanOrEqual:
                    valueExp = GetConstantExpresion(param, search.PropertyName, search.Value);
                    paramExpression = Expression.LessThanOrEqual(prop, Expression.Convert(valueExp, prop.Type));
                    break;
                case OperatorEnm.NotLessThanOrEqual:
                    valueExp = GetConstantExpresion(param, search.PropertyName, search.Value);
                    paramExpression = Expression.LessThanOrEqual(prop, Expression.Convert(valueExp, prop.Type));
                    paramExpression = Expression.Not(paramExpression);
                    break;

                case OperatorEnm.Between:
                    var paramStartValue = GetConstantExpresion(param, search.PropertyName, search.StartValue);
                    var paramEndValue = GetConstantExpresion(param, search.PropertyName, search.EndValue);

                    var exStart = Expression.LessThanOrEqual(prop, Expression.Convert(paramStartValue, prop.Type));
                    var exEnd = Expression.GreaterThanOrEqual(prop, Expression.Convert(paramEndValue, prop.Type));

                    paramExpression = Expression.And(exStart, exEnd);
                    break;
                case OperatorEnm.NotBetween:
                    paramStartValue = GetConstantExpresion(param, search.PropertyName, search.StartValue);
                    paramEndValue = GetConstantExpresion(param, search.PropertyName, search.EndValue);

                    exStart = Expression.LessThanOrEqual(prop, Expression.Convert(paramStartValue, prop.Type));
                    exEnd = Expression.GreaterThanOrEqual(prop, Expression.Convert(paramEndValue, prop.Type));

                    paramExpression = Expression.And(exStart, exEnd);
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

