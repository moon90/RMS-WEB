using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Core.Configurations
{
    public class KeyVaultOption
    {
        public required string ClientId { get; set; }
        public required string ClientSecret { get; set; }
        public required string TenantId { get; set; }
        public required string DbConnection { get; set; }
        public required string ElmahConnection { get; set; }
    }
}
