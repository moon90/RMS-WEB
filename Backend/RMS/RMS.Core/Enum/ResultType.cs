using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Core.Enum
{
    public enum ResultType
    {
        [Description("Undefined")]
        Undefined,

        [Description("Exist")]
        Exist,

        [Description("Server Error")]
        ServerError,

        [Description("Invalid input")]
        InvalidInput,

        [Description("Not found")]
        NotFound
    }
}
