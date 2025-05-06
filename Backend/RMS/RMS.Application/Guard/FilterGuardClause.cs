using RMS.Core.Enum;
using RMS.Core.Exceptions;
using RMS.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Application.Guard
{
    public class FilterGuardClause
    {
        public class Against<T> where T : PagedQuery
        {
            private const int _maxPagesize = 100;

            /// <summary>
            /// Throws an <see cref="NotFoundException" /> if <paramref name="value" /> with <paramref name="key" /> is not found.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="value">Value of T</param>
            /// <param name="key">key of T</param>
            /// <returns><paramref name="input" /> if the value is not null.</returns>
            /// <exception cref="NotFoundException"></exception>
            public static void NotValid(T query)
            {
                if (query.PageNumber <= 0)
                    throw new InvalidInputException(nameof(query.PageNumber), InvalidInputType.ZeroOrNegative);

                if (query.PageSize <= 0 || query.PageSize >= _maxPagesize)
                    throw new InvalidInputException($"{nameof(query.PageSize)} is out of the allowed range (1 to {_maxPagesize}).");
            }

            public static void OrderPropertyNotFound<TEntity>(T query, string defaultOrder, bool isDescending) where TEntity : class
            {
                if (string.IsNullOrEmpty(query.OrderBy))
                {
                    query.OrderBy = defaultOrder;
                    query.IsDescending = isDescending;
                }
                else
                {
                    if (typeof(TEntity).GetProperty(query.OrderBy, BindingFlags.Public | BindingFlags.Instance) is null)
                        throw new InvalidInputException($"{typeof(TEntity).Name} does not has property with name: {query.OrderBy}"); ;
                }
            }
        }
    }
}
