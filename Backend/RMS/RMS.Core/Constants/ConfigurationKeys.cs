using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Core.Constants
{
    public static class ConfigurationKeys
    {
        public const string SuperUsers = "super-users";
        public const string ExternalUsers = "external-users";

        // Constant for Configuration
        public const string AzureAppClientId = "Azure:App:ClientId";
        public const string AzureAppClientSecret = "Azure:App:ClientSecret";
        public const string AzureTenantId = "Azure:App:TenantId";
        public const string DbConnectionString = "Database:ConnectionString";
        public const string ElmahConnectionString = "Elmah:ConnectionString";

        // Constant for Azure KeyVault
        public const string AzureKeyVaultClientId = "afemscob-clientId";
        public const string AzureKeyVaultClientSecret = "afemscob-clientSecret";
        public const string AzureKeyVaultTenantId = "afemscob-tenantId";
        public const string AzureKeyVaultDatabaseConnection = "afemscob-dbConnection";
        public const string AzureKeyVaultElmahConnection = "afemscob-elmahConnection";
    }
}
