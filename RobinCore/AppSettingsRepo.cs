using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobinCore
{
    public class AppSettingsRepo : IAppSettings
    {

        private IConfiguration _configuration;

        public AppSettingsRepo(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string client_id => _configuration["AzureKeyVaultHR:client_id"];
        public string client_secret => _configuration["AzureKeyVaultHR:client_secret"];
        public string tenant_id => _configuration["AzureKeyVaultHR:tenant_id"];
        public string url => _configuration["AzureKeyVaultHR:url"];

    }
}
