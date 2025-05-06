using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Core.Extensions
{
    public static class ExpressionExtensions
    {
        private const string _defaultParam = "e";

        /// <summary>
        /// Build dynamic filter Epression<Func<T, bool>> by IDictionary
        /// </summary>
        /// <typeparam name="T">Type of Entity</typeparam>
        /// <param name="filters">IDictionary<string, string>?</param>
        /// <returns>An Expression of Entity or return x => true when null</returns>
        public static Expression<Func<T, bool>> BuildDynamicFilter<T>(IDictionary<string, string>? filters)
        {
            if (filters is null || filters.Count == 0)
                return x => true;

            var param = Expression.Parameter(typeof(T), _defaultParam);
            Expression? filterExpression = null;

            foreach (var filter in filters)
            {
                var property = typeof(T).GetProperty(filter.Key, BindingFlags.Public | BindingFlags.Instance);
                if (property == null) continue; // Skip if property does not exist

                var propertyAccess = Expression.Property(param, property);
                var propertyType = property.PropertyType;
                var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

                // Convert value to correct type
                object? convertedValue = filter.Value == null ? null : Convert.ChangeType(filter.Value, underlyingType);
                var constant = Expression.Constant(convertedValue, underlyingType);

                var condition = Nullable.GetUnderlyingType(propertyType) != null
                    ? Expression.Equal(Expression.Convert(propertyAccess, underlyingType), constant)
                    : Expression.Equal(propertyAccess, constant);

                filterExpression = filterExpression == null ? condition : Expression.AndAlso(filterExpression, condition);
            }

            return filterExpression != null ? Expression.Lambda<Func<T, bool>>(filterExpression, param) : x => true;
        }
    }
}
