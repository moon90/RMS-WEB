using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RMS.Application.Exceptions
{
    public class ValidationExceptionCustom : Exception
    {
        public List<string> Errors { get; }

        public ValidationExceptionCustom()
            : base("One or more validation failures have occurred.")
        {
            Errors = new List<string>();
        }

        public ValidationExceptionCustom(IEnumerable<ValidationFailure> failures)
            : this()
        {
            Errors.AddRange(failures.Select(f => f.ErrorMessage));
        }
    }
}