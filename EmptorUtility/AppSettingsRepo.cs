using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptorUtility
{
    public class AppSettingsRepo : IAppSettings
    {
        private readonly IConfiguration _configuration;

        public AppSettingsRepo(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string? EmptorConnectionString => _configuration.GetConnectionString("EmptorConnection");


        //public string EmptorConnectionString => throw new NotImplementedException();
    }
}
