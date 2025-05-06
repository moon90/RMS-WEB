using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.Models.Request
{
    public class CommonFilterModel
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string Filter { get; set; }
        public string Group { get; set; }
        public string Sort { get; set; }
    }
}
