using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Domain.Entities
{
    public class SystemSetting : BaseEntity
    {
        public int Id { get; set; }
        public required string SettingKey { get; set; }
        public string? SettingValue { get; set; }
        public string? SettingGroup { get; set; }
        public string? Description { get; set; }
        public string? SettingType { get; set; } // "String", "Number", "Boolean", "Image"
    }
}
