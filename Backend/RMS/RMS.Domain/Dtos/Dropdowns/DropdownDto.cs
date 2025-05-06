using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.Dtos.Dropdowns
{
    public class DropdownDto
    {
        public int Value { get; set; }
        public required string Label { get; set; }
    }
}
