using RMS.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Core.Guard
{
    public class StringGuardClause
    {
        public class Against
        {
            /// <summary>
            /// Throws an <see cref="InvalidInputException" /> if <paramref name="param" /> is null or empty.
            /// </summary>
            /// <param name="value"></param>
            /// <param name="param"></param>
            /// <returns><paramref name="input" /> if the value is not null.</returns>
            /// <exception cref="InvalidInputException"></exception>
            public static void NullOrEmpty(string value, string param)
            {
                if (string.IsNullOrEmpty(value))
                    throw new InvalidInputException(param, Enum.InvalidInputType.NullOrEmpty);
            }
        }
    }
}
