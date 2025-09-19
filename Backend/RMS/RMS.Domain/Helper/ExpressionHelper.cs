using System;
using System.Linq.Expressions;

namespace RMS.Domain.Helper
{
    public static class ExpressionHelper
    {
        public static Expression<Func<T, bool>> BuildStartsWithExpression<T>(Expression<Func<T, string>> property, string value)
        {
            var parameter = property.Parameters[0];
            var method = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
            var call = Expression.Call(property.Body, method, Expression.Constant(value));
            return Expression.Lambda<Func<T, bool>>(call, parameter);
        }

        public static Expression<Func<T, bool>> BuildContainsExpression<T>(Expression<Func<T, string>> property, string value)
        {
            var parameter = property.Parameters[0];
            var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var call = Expression.Call(property.Body, method, Expression.Constant(value));
            return Expression.Lambda<Func<T, bool>>(call, parameter);
        }

        public static Expression<Func<T, bool>> BuildNotContainsExpression<T>(Expression<Func<T, string>> property, string value)
        {
            var parameter = property.Parameters[0];
            var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var call = Expression.Call(property.Body, method, Expression.Constant(value));
            var notCall = Expression.Not(call);
            return Expression.Lambda<Func<T, bool>>(notCall, parameter);
        }

        public static Expression<Func<T, bool>> BuildEqualsExpression<T, TValue>(Expression<Func<T, TValue>> property, TValue value)
        {
            var parameter = property.Parameters[0];
            var body = Expression.Equal(property.Body, Expression.Constant(value, typeof(TValue)));
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> BuildNotEqualsExpression<T, TValue>(Expression<Func<T, TValue>> property, TValue value)
        {
            var parameter = property.Parameters[0];
            var body = Expression.NotEqual(property.Body, Expression.Constant(value, typeof(TValue)));
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> BuildEndsWithExpression<T>(Expression<Func<T, string>> property, string value)
        {
            var parameter = property.Parameters[0];
            var method = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
            var call = Expression.Call(property.Body, method, Expression.Constant(value));
            return Expression.Lambda<Func<T, bool>>(call, parameter);
        }

        public static Expression<Func<T, bool>> BuildGreaterThanExpression<T, TValue>(Expression<Func<T, TValue>> property, TValue value) where TValue : IComparable
        {
            var parameter = property.Parameters[0];
            var body = Expression.GreaterThan(property.Body, Expression.Constant(value, typeof(TValue)));
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> BuildGreaterThanOrEqualExpression<T, TValue>(Expression<Func<T, TValue>> property, TValue value) where TValue : IComparable
        {
            var parameter = property.Parameters[0];
            var body = Expression.GreaterThanOrEqual(property.Body, Expression.Constant(value, typeof(TValue)));
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> BuildLessThanExpression<T, TValue>(Expression<Func<T, TValue>> property, TValue value) where TValue : IComparable
        {
            var parameter = property.Parameters[0];
            var body = Expression.LessThan(property.Body, Expression.Constant(value, typeof(TValue)));
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> BuildLessThanOrEqualExpression<T, TValue>(Expression<Func<T, TValue>> property, TValue value) where TValue : IComparable
        {
            var parameter = property.Parameters[0];
            var body = Expression.LessThanOrEqual(property.Body, Expression.Constant(value, typeof(TValue)));
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> BuildDateEqualsExpression<T>(Expression<Func<T, DateTime>> property, DateTime value)
        {
            var parameter = property.Parameters[0];
            var dateProperty = Expression.Property(property.Body, "Date");
            var body = Expression.Equal(dateProperty, Expression.Constant(value.Date, typeof(DateTime)));
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> BuildDateNotEqualsExpression<T>(Expression<Func<T, DateTime>> property, DateTime value)
        {
            var parameter = property.Parameters[0];
            var dateProperty = Expression.Property(property.Body, "Date");
            var body = Expression.NotEqual(dateProperty, Expression.Constant(value.Date, typeof(DateTime)));
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> BuildDateGreaterThanExpression<T>(Expression<Func<T, DateTime>> property, DateTime value)
        {
            var parameter = property.Parameters[0];
            var dateProperty = Expression.Property(property.Body, "Date");
            var body = Expression.GreaterThan(dateProperty, Expression.Constant(value.Date, typeof(DateTime)));
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> BuildDateLessThanExpression<T>(Expression<Func<T, DateTime>> property, DateTime value)
        {
            var parameter = property.Parameters[0];
            var dateProperty = Expression.Property(property.Body, "Date");
            var body = Expression.LessThan(dateProperty, Expression.Constant(value.Date, typeof(DateTime)));
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }
    }
}
