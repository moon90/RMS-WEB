using System;
using RMS.Application.Interfaces;

namespace RMS.Application.DTOs.SystemSettings
{
    public class SystemSettingDto
    {
        public int Id { get; set; }
        public string SettingKey { get; set; } = string.Empty;
        public string? SettingValue { get; set; }
        public string? SettingGroup { get; set; }
        public string? Description { get; set; }
        public string? SettingType { get; set; }
    }

    public class UpdateSystemSettingDto
    {
        public string SettingKey { get; set; } = string.Empty;
        public string? SettingValue { get; set; }
    }
}
