using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.Dtos.PermissionDTOs.InputDTOs
{
    public class PermissionCreateDto
    {
        public required string PermissionName { get; set; }
        public required string PermissionKey { get; set; }

        public string? ControllerName { get; set; }
        public string? ActionName { get; set; }
        public string? ModuleName { get; set; }
    }
}
