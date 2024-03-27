using Azure.Identity;
using Newtonsoft.Json;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobinCore.Models;

namespace RobinCore
{
    public class AzureKeyVaultRobin
    {

        private HR_IAppSettings _appConfig;
     

        public AzureKeyVaultRobin(HR_IAppSettings appConfig)
        {
            _appConfig = appConfig;
        }

        public async Task<ViziteLoginRequest>? RobinAzureKeyVault(string AzureKeyVaultCode, string _azureServiceLink)
        {
            ClientSecretCredential credential = new ClientSecretCredential(_appConfig.tenant_id, _appConfig.client_id, _appConfig.client_secret);
            SecretClient SecretClient = new SecretClient(new Uri(_azureServiceLink), credential);
            var tt = await SecretClient.GetSecretAsync(AzureKeyVaultCode);
            ViziteLoginRequest response = JsonConvert.DeserializeObject<ViziteLoginRequest>(tt.Value.Value);
            return response;
        }
    }
}
