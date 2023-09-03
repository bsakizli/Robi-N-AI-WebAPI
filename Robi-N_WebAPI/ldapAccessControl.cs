using Robi_N_WebAPI.Model;

namespace Robi_N_WebAPI
{
    public class ldapAccessControl : GlobalResponse
    {
        public string? displayName { get; set; }
        public string? fullName { get; set; }
    }
}
