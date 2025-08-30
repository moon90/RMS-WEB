using RMS.Core.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Core.Extensions
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Apply paging
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        /// <summary>
        /// Apply sort
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortDirection"></param>
        /// <returns></returns>
        public static IQueryable<T> ApplySorting<T>(this IQueryable<T> source, string sortBy, string sortDirection)
        {
            if (string.IsNullOrEmpty(sortBy)) return source;

            var isDescending = sortDirection?.ToLower() == "desc";

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.PropertyOrField(parameter, sortBy);
            var lambda = Expression.Lambda(property, parameter);

            string methodName = isDescending ? "OrderByDescending" : "OrderBy";

            var result = typeof(Queryable).GetMethods()
                .Where(m => m.Name == methodName
                            && m.IsGenericMethodDefinition // Ensure it's a generic method definition
                            && m.GetParameters().Length == 2 // Ensure it has two parameters
                            && m.GetParameters()[0].ParameterType == typeof(IQueryable<>) // First parameter is IQueryable<>
                            && m.GetParameters()[1].ParameterType == typeof(Expression<>)) // Second parameter is Expression<>
                .Single()
                .MakeGenericMethod(typeof(T), property.Type)
                .Invoke(null, new object[] { source, lambda });

            return (IQueryable<T>)result;
        }

        /// <summary>
        /// Apply fitler for string value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="property"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IQueryable<T> ApplyStringFilter<T>(this IQueryable<T> query, Expression<Func<T, string>> property, DynamicFilterField filter)
        {
            var value = filter.Value?.ToString();
            if (string.IsNullOrEmpty(value))
            {
                return query;
            }

            return filter.MatchMode switch
            {
                "startsWith" => query.Where(ExpressionHelper.BuildStartsWithExpression(property, value)),
                "contains" => query.Where(ExpressionHelper.BuildContainsExpression(property, value)),
                "notContains" => query.Where(ExpressionHelper.BuildNotContainsExpression(property, value)),
                "equals" => query.Where(ExpressionHelper.BuildEqualsExpression(property, value)),
                "notEquals" => query.Where(ExpressionHelper.BuildNotEqualsExpression(property, value)),
                "endsWith" => query.Where(ExpressionHelper.BuildEndsWithExpression(property, value)),
                _ => query
            };
        }

        /// <summary>
        /// Apply filter for numeric value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="property"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IQueryable<T> ApplyNumberFilter<T>(this IQueryable<T> query, Expression<Func<T, int>> property, DynamicFilterField filter)
        {
            if (!int.TryParse(filter.Value?.ToString(), out int value))
            {
                return query;
            }

            return filter.MatchMode switch
            {
                "equals" => query.Where(ExpressionHelper.BuildEqualsExpression(property, value)),
                "notEquals" => query.Where(ExpressionHelper.BuildNotEqualsExpression(property, value)),
                "gt" => query.Where(ExpressionHelper.BuildGreaterThanExpression(property, value)),
                "gte" => query.Where(ExpressionHelper.BuildGreaterThanOrEqualExpression(property, value)),
                "lt" => query.Where(ExpressionHelper.BuildLessThanExpression(property, value)),
                "lte" => query.Where(ExpressionHelper.BuildLessThanOrEqualExpression(property, value)),
                _ => query
            };
        }

        /// <summary>
        /// Apply filter for date time value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="property"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IQueryable<T> ApplyDateTimeFilter<T>(
            this IQueryable<T> query,
            Expression<Func<T, DateTime>> property,
            DynamicFilterField filter)
        {
            if (!DateTime.TryParse(filter.Value?.ToString(), new CultureInfo("en-US"), out DateTime filterValue))
            {
                return query;
            }

            var dateValue = filterValue.Date;

            return filter.MatchMode switch
            {
                "dateIs" => query.Where(ExpressionHelper.BuildDateEqualsExpression(property, dateValue)),
                "dateIsNot" => query.Where(ExpressionHelper.BuildDateNotEqualsExpression(property, dateValue)),
                "dateAfter" => query.Where(ExpressionHelper.BuildDateGreaterThanExpression(property, dateValue)),
                //"greaterThanOrEqual" => query.Where(ExpressionHelper.BuildDateGreaterThanOrEqualExpression(property, dateValue)),
                "dateBefore" => query.Where(ExpressionHelper.BuildDateLessThanExpression(property, dateValue)),
                //"lessThanOrEqual" => query.Where(ExpressionHelper.BuildDateLessThanOrEqualExpression(property, dateValue)),
                _ => query
            };
        }
    }
}
