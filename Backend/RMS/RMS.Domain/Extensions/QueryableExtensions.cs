using Microsoft.EntityFrameworkCore;
using RMS.Core.Extensions;
using RMS.Domain.Helper;
using RMS.Domain.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.Extensions
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Asynchronously creates a PagedResult from an IQueryable source.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="source">The IQueryable source.</param>
        /// <param name="pageNumber">The current page number (1-based).</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A Task that represents the asynchronous operation, containing a PagedResult<T>.</returns>
        public static async Task<PagedResult<T>> ToPagedList<T>(this IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<T>(items, pageNumber, pageSize, count);
        }

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
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string sortBy, string sortDirection)
        {
            try
            {
                if (string.IsNullOrEmpty(sortBy)) return source;

                var isDescending = sortDirection?.ToLower() == "desc";

                var type = typeof(T);
                var propertyInfo = type.GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo == null)
                {
                    throw new ArgumentException($"Property '{sortBy}' not found on type '{typeof(T).Name}'.", nameof(sortBy));
                }
                var parameter = Expression.Parameter(type, "p");
                var propertyAccess = Expression.MakeMemberAccess(parameter, propertyInfo);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);

                string methodName = isDescending ? "OrderByDescending" : "OrderBy";

                var genericMethod = typeof(Queryable).GetMethods()
                    .Where(m => m.Name == methodName && m.IsGenericMethodDefinition)
                    .Where(m =>
                    {
                        var parameters = m.GetParameters().ToList();
                        // Queryable.OrderBy<TSource, TKey>(IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
                        return parameters.Count == 2 &&
                               parameters[0].ParameterType.IsGenericType &&
                               parameters[0].ParameterType.GetGenericTypeDefinition() == typeof(IQueryable<>) &&
                               parameters[1].ParameterType.IsGenericType &&
                               parameters[1].ParameterType.GetGenericTypeDefinition() == typeof(Expression<>) &&
                               parameters[1].ParameterType.GetGenericArguments()[0].GetGenericTypeDefinition() == typeof(Func<,>);
                    })
                    .Single();

                var method = genericMethod.MakeGenericMethod(type, propertyInfo.PropertyType);

                MethodCallExpression resultExp = Expression.Call(null, method, source.Expression, Expression.Quote(orderByExp));

                return (IQueryable<T>)source.Provider.CreateQuery(resultExp);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
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