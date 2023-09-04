using Robi_N_WebAPI.Model;

namespace Robi_N_WebAPI
{
    public class ldapAccessControl : GlobalResponse
    {
        public string? displayName { get; set; }
        public string? fullName { get; set; }
        public string? userName { get; set; }
        public string? token { get; set; }
        public DateTime? expiredDate { get; set; }
    }
}
