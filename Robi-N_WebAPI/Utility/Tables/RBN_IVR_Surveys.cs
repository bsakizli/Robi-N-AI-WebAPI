using System.ComponentModel.DataAnnotations;

namespace Robi_N_WebAPI.Utility.Tables
{
    public class RBN_IVR_Surveys
    {
        [Key]
        public long id { get; set; }
        public string? name { get; set; }
        public DateTime? add_date { get; set; }
        public bool active { get; set; }
    }
}
