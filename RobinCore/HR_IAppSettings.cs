using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobinCore
{
    public interface HR_IAppSettings
    {


        public string? client_id { get;}
        public string? client_secret { get;}
        public string? tenant_id { get;}
    }
}
