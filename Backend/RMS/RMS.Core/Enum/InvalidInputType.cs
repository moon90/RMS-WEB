using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Core.Enum
{
    public enum InvalidInputType
    {
        [Description("Undefined")]
        Undefined,

        [Description("Null")]
        Null,

        [Description("Zero or Negative")]
        ZeroOrNegative,

        [Description("Null or Empty")]
        NullOrEmpty,

        [Description("Property is not exist.")]
        NotExistProperty
    }
}
