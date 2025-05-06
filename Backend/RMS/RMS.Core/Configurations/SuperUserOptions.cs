using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Core.Configurations
{
    public class SuperUserOptions
    {
        public const string SectionName = "super-users";
        public string SuperUsers { get; set; } = string.Empty;
    }

    public class ExternalUserOptions
    {
        public const string SectionName = "external-users";
        public string ExternalUsers { get; set; } = string.Empty;
    }
}
