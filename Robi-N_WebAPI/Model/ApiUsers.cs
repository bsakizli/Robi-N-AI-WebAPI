using System.ComponentModel.DataAnnotations;

namespace Robi_N_WebAPI.Model
{
    public class ApiUsers
    {
        [Key]
        public int Id { get; set; }
        
        public string? username { get; set; }
       
        public string? password { get; set; }
        public string? salt { get; set; }
        public int? expirationTime { get; set; }


        public string? role { get; set; }
        public DateTime? add_date { get; set; }
        public DateTime? update_date { get; set; }
        public bool? active { get; set; }
    }
}
