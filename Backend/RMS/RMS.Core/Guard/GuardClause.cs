using RMS.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Core.Guard
{
    public class GuardClause
    {
        public class Against
        {
            /// <summary>
            /// Throws an <see cref="NotFoundException" /> if <paramref name="value" /> with <paramref name="key" /> is not found.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="value">Value of T</param>
            /// <param name="key">key of T</param>
            /// <returns><paramref name="input" /> if the value is not null.</returns>
            /// <exception cref="NotFoundException"></exception>
            public static T NotFound<T>(T? value, object key)
                => value == null ? throw new NotFoundException(typeof(T).Name, key) : value;

            /// <summary>
            /// Throws an <see cref="NullReferenceException" /> if the list of <paramref name="T" /> is null or empty.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="value">A list of T</param>
            /// <returns></returns>
            /// <exception cref="NullReferenceException"></exception>
            public static List<T> NullOrEmpty<T>(List<T>? value)
                => value == null || value.Count == 0 ? throw new NullReferenceException($"A list of {typeof(T).Name}") : value;

            /// <summary>
            /// Throws an <see cref="InvalidInputException" /> if the <paramref name="T" /> is null.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="value"></param>
            /// <returns></returns>
            /// <exception cref="InvalidInputException"></exception>
            public static T Null<T>(T? value, string param)
                => value == null ? throw new InvalidInputException(param, Enum.InvalidInputType.Null) : value;
        }
    }
}
