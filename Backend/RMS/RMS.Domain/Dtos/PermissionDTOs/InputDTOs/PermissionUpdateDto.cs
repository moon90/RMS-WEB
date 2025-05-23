﻿namespace RMS.Domain.Dtos.PermissionDTOs.InputDTOs
{
    public class PermissionUpdateDto
    {
        public int Id { get; set; }

        public string PermissionName { get; set; }

        public string PermissionKey { get; set; }

        public string? ControllerName { get; set; }
        public string? ActionName { get; set; }
        public string? ModuleName { get; set; }
    }
}
