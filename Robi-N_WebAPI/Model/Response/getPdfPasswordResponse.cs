using Microsoft.Kiota.Abstractions;

namespace Robi_N_WebAPI.Model.Response
{
    public class getPdfPasswordResponse : GlobalResponse
    {
        public long password { get; set; }
        public DateTime date { get; set; }
    }
}
