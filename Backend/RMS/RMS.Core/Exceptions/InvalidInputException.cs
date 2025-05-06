using RMS.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Core.Exceptions
{
    public class InvalidInputException : Exception
    {
        public InvalidInputException() : base()
        {

        }

        public InvalidInputException(string param, InvalidInputType type) : base(BuildMessage(param, type))
        {

        }

        public InvalidInputException(string message) : base(message)
        {

        }

        public InvalidInputException(string message, Exception innerException) : base(message, innerException)
        {

        }

        private static string BuildMessage(string param, InvalidInputType type)
            => type switch
            {
                InvalidInputType.Null => $"{param} could not be null.",
                InvalidInputType.ZeroOrNegative => $"{param} could not be equal or less than zero.",
                InvalidInputType.NullOrEmpty => $"{param} could not be null or empty.",
                _ => $"{param} is not valid."
            };
    }
}
