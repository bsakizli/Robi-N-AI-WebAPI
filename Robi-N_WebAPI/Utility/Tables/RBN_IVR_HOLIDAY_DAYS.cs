using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Robi_N_WebAPI.Utility.Tables
{
    public class RBN_IVR_HOLIDAY_DAYS
    {
        [Key]
        public int Id { get; set; }
        public string? displayName { get; set; }
        [Required,]
        public string? csq { get; set; }
        public string? description { get; set; }
        public string? holidayName { get; set; }

    
        public DateTime holidayDate { get; set; }
      
        public DateTime startDate { get; set; }
       
        public DateTime endDate { get; set; }
       

        public int? years { get; set; }
        public DateTime addDate { get; set; }
        public DateTime updateDate { get; set; }
        public bool active { get; set; }
    }
}
