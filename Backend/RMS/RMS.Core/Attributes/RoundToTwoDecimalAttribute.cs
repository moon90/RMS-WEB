using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Core.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class RoundToTwoDecimalAttribute : Attribute
    {
        public int MultipleExtend { get; set; }
        public int DivideExtend { get; set; }
    }
}
