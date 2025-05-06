using RMS.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Core.Guard
{
    public class IntGuardClause
    {
        public class Against
        {
            /// <summary>
            /// Throws an <see cref="InvalidInputException" /> if <paramref name="value" /> is zero or negative.
            /// </summary>
            /// <param name="value"></param>
            /// <param name="param"></param>
            /// <returns><paramref name="input" /> if the value is not null.</returns>
            /// <exception cref="InvalidInputException"></exception>
            public static void ZeroOrNegative(int value, string param)
            {
                if (value <= 0)
                    throw new InvalidInputException(param, Enum.InvalidInputType.ZeroOrNegative);
            }

            /// <summary>
            /// Throws an <see cref="InvalidInputException" /> if <paramref name="value" /> is zero or negative.
            /// </summary>
            /// <param name="value"></param>
            /// <param name="param"></param>
            /// <returns><paramref name="input" /> if the value is not null.</returns>
            /// <exception cref="InvalidInputException"></exception>
            public static void ZeroOrNegative(object value, string param)
            {
                if (value is int v && v <= 0)
                    throw new InvalidInputException(param, Enum.InvalidInputType.ZeroOrNegative);
            }
        }
    }
}
