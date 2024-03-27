using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobinCore
{
    public class AppSettingsRepoNetas : HR_IAppSettings
    {

        private IConfiguration _configuration;

        public AppSettingsRepoNetas(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string client_id => _configuration["AzureKeyVaultHR_Netas:client_id"];
        public string client_secret => _configuration["AzureKeyVaultHR_Netas:client_secret"];
        public string tenant_id => _configuration["AzureKeyVaultHR_Netas:tenant_id"];

    }
}
