using System;

namespace RMS.Application.DTOs.PermissionDTOs.OutputDTOs
{
    public class PermissionDto
    {
        public int Id { get; set; }
        public string? PermissionName { get; set; }
        public string? PermissionKey { get; set; }
        public string? ControllerName { get; set; }
        public string? ActionName { get; set; }
        public string? ModuleName { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public required string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
    }
}