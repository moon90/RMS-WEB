using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.Models.BaseModels
{
    public enum ApiErrorCode
    {
        None = 0,
        ValidationError = 1,
        NotFound = 2,
        Unauthorized = 3,
        Conflict = 4,
        ServerError = 5
    }
}
