using System.ComponentModel.DataAnnotations;

namespace Robi_N_WebAPI.Utility.Tables
{
    public class RBN_RequestACallBack
    {
        [Key]
        public int Id { get; set; }
        public string? CallingNumber { get; set; }
        public string? CallId { get; set; }
        public long? CallCode { get; set; }
        public DateTime? add_date { get; set; }
        public bool active { get; set; }
    }
}
