using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Core.Extensions
{
    public class DynamicFilterField
    {
        /// <summary>
        /// Filter column, such as 'fullName', 'email'
        /// </summary>
        public object? Value { get; set; }
        /// <summary>
        /// Filter type: 'startsWith', 'contains', 'equals'
        /// </summary>
        public string? MatchMode { get; set; }
    }
}
