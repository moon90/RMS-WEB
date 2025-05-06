using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.Dtos.PermissionDTOs.OutputDTOs
{
    public class PermissionDto
    {
        public int Id { get; set; }
        public string PermissionName { get; set; }
        public string PermissionKey { get; set; }
        public string? ControllerName { get; set; }
        public string? ActionName { get; set; }
        public string? ModuleName { get; set; }
    }
}
