namespace RMS.Application.DTOs.PermissionDTOs.InputDTOs
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