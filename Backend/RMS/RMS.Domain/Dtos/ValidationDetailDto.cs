using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.Dtos
{
    public class ValidationDetailDto
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
    }
}
