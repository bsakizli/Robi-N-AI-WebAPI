using System.ComponentModel.DataAnnotations;

namespace Robi_N_WebAPI.Utility.Tables
{
    public class RBN_AI_SERVICE_ROLE
    {
        [Key]
        public int Id { get; set; }
        public string? RoleName { get; set; }
        public DateTime add_date { get; set; }
        public DateTime update_date { get; set; }
        public bool active { get; set; }
    }
}
