using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Core.Helper
{
    public static class ExpressionHelper
    {
        // For string type
        public static Expression<Func<T, bool>> BuildContainsExpression<T>(
            Expression<Func<T, string>> property,
            string value)
        {
            var parameter = property.Parameters[0];
            var body = Expression.Call(
                property.Body,
                typeof(string).GetMethod("Contains", new[] { typeof(string) }),
                Expression.Constant(value));

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> BuildNotContainsExpression<T>(
            Expression<Func<T, string>> property,
            string value)
        {
            var containsExpression = BuildContainsExpression(property, value);
            var notExpression = Expression.Not(containsExpression.Body);

            return Expression.Lambda<Func<T, bool>>(notExpression, containsExpression.Parameters);
        }

        public static Expression<Func<T, bool>> BuildStartsWithExpression<T>(Expression<Func<T, string>> property, string value)
        {
            var parameter = property.Parameters[0];
            var body = Expression.Call(
                property.Body,
                typeof(string).GetMethod("StartsWith", new[] { typeof(string) }),
                Expression.Constant(value));

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> BuildEndsWithExpression<T>(
            Expression<Func<T, string>> property,
            string value)
        {
            var parameter = property.Parameters[0];
            var body = Expression.Call(
                property.Body,
                typeof(string).GetMethod("EndsWith", new[] { typeof(string) }),
                Expression.Constant(value));

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> BuildEqualsExpression<T>(Expression<Func<T, string>> property, string value)
        {
            var parameter = property.Parameters[0];
            var body = Expression.Equal(
                property.Body,
                Expression.Constant(value));

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> BuildNotEqualsExpression<T>(Expression<Func<T, string>> property, string value)
        {
            var parameter = property.Parameters[0];
            var body = Expression.NotEqual(
                property.Body,
                Expression.Constant(value));

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        // For numeric types
        public static Expression<Func<T, bool>> BuildEqualsExpression<T, TValue>(Expression<Func<T, TValue>> property, TValue value)
        {
            var parameter = property.Parameters[0];
            var body = Expression.Equal(
                property.Body,
                Expression.Constant(value, typeof(TValue)));

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> BuildNotEqualsExpression<T, TValue>(Expression<Func<T, TValue>> property, TValue value)
        {
            var parameter = property.Parameters[0];
            var body = Expression.NotEqual(
                property.Body,
                Expression.Constant(value, typeof(TValue)));

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> BuildGreaterThanExpression<T, TValue>(Expression<Func<T, TValue>> property, TValue value)
        {
            var parameter = property.Parameters[0];
            var body = Expression.GreaterThan(
                property.Body,
                Expression.Constant(value, typeof(TValue)));

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> BuildGreaterThanOrEqualExpression<T, TValue>(
            Expression<Func<T, TValue>> property,
            TValue value) where TValue : IComparable
        {
            var parameter = property.Parameters[0];
            var body = Expression.GreaterThanOrEqual(
                property.Body,
                Expression.Constant(value, typeof(TValue)));

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> BuildLessThanExpression<T, TValue>(Expression<Func<T, TValue>> property, TValue value)
        {
            var parameter = property.Parameters[0];
            var body = Expression.LessThan(
                property.Body,
                Expression.Constant(value, typeof(TValue)));

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> BuildLessThanOrEqualExpression<T, TValue>(Expression<Func<T, TValue>> property, TValue value)
        {
            var parameter = property.Parameters[0];
            var body = Expression.LessThanOrEqual(
                property.Body,
                Expression.Constant(value, typeof(TValue)));

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        // For DateTime
        public static Expression<Func<T, bool>> BuildDateEqualsExpression<T>(Expression<Func<T, DateTime>> property, DateTime value)
        {
            var parameter = property.Parameters[0];
            var body = Expression.Equal(
                property.Body,
                Expression.Constant(value.Date)); // Compare only date part

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> BuildDateNotEqualsExpression<T>(
            Expression<Func<T, DateTime>> property,
            DateTime value)
        {
            var equalsExpr = BuildDateEqualsExpression(property, value);
            return Expression.Lambda<Func<T, bool>>(
                Expression.Not(equalsExpr.Body),
                equalsExpr.Parameters);
        }

        public static Expression<Func<T, bool>> BuildDateGreaterThanExpression<T>(Expression<Func<T, DateTime>> property, DateTime value)
        {
            var parameter = property.Parameters[0];
            var body = Expression.GreaterThan(
                Expression.Property(property.Body, "Date"),
                Expression.Constant(value.Date));

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> BuildDateGreaterThanOrEqualExpression<T>(
            Expression<Func<T, DateTime>> property,
            DateTime value)
        {
            var parameter = property.Parameters[0];
            var body = Expression.GreaterThanOrEqual(
                Expression.Property(property.Body, "Date"),
                Expression.Constant(value.Date));

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> BuildDateLessThanExpression<T>(
           Expression<Func<T, DateTime>> property,
           DateTime value)
        {
            var parameter = property.Parameters[0];
            var body = Expression.LessThan(
                Expression.Property(property.Body, "Date"),
                Expression.Constant(value.Date));

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> BuildDateLessThanOrEqualExpression<T>(Expression<Func<T, DateTime>> property, DateTime value)
        {
            var parameter = property.Parameters[0];
            var body = Expression.LessThanOrEqual(
                property.Body,
                Expression.Constant(value.Date));

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        // Additional method for nullable types
        public static Expression<Func<T, bool>> BuildEqualsExpression<T, TValue>(Expression<Func<T, TValue?>> property, TValue value) where TValue : struct
        {
            var parameter = property.Parameters[0];
            var body = Expression.Equal(
                property.Body,
                Expression.Constant(value, typeof(TValue?)));

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> BuildNotEqualsExpression<T, TValue>(Expression<Func<T, TValue?>> property, TValue value) where TValue : struct
        {
            var parameter = property.Parameters[0];
            var body = Expression.NotEqual(
                property.Body,
                Expression.Constant(value, typeof(TValue?)));

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> BuildLessThanOrEqualExpression<T, TValue>(Expression<Func<T, TValue?>> property, TValue value) where TValue : struct
        {
            var parameter = property.Parameters[0];
            var body = Expression.LessThanOrEqual(
                property.Body,
                Expression.Constant(value, typeof(TValue?)));

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> BuildGreaterThanExpression<T, TValue>(
            Expression<Func<T, TValue?>> property,
            TValue value) where TValue : struct, IComparable
        {
            var parameter = property.Parameters[0];
            var body = Expression.GreaterThan(
                property.Body,
                Expression.Constant(value, typeof(TValue?)));

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression<Func<T, bool>> BuildGreaterThanOrEqualExpression<T, TValue>(
            Expression<Func<T, TValue?>> property,
            TValue value) where TValue : struct, IComparable
        {
            var parameter = property.Parameters[0];
            var body = Expression.GreaterThanOrEqual(
                property.Body,
                Expression.Constant(value, typeof(TValue?)));

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }
    }
}
