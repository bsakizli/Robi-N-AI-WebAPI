using System.ComponentModel.DataAnnotations;

namespace Robi_N_WebAPI.Utility.Tables
{
    public class RBN_SGK_Settings
    {
        [Key]
        public int id { get; set; }
        public int firmCode { get; set; }
        public string? tenant_id { get; set; }
        public string? client_id { get; set; }
        public string? client_secret { get; set; }
        public string? url { get; set; }
        public bool active { get; set; }
    }
}
