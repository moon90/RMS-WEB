using RMS.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.Models.Request
{
    public class DynamicFilterRequest
    {
        public int SortOrder { get; set; }
        public Dictionary<string, DynamicFilterField> Filters { get; set; } = new();
        public string? GlobalFilter { get; set; }
    }
}
