using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Robi_N_WebAPI.Utility.Tables
{
    public class RBN_IVR_HOLIDAY_DAYS
    {
        [Key]
        public int Id { get; set; }
        public string? displayName { get; set; }
        public string? description { get; set; }
        public string? holidayName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? holidayDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime startDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime endDate { get; set; }
        [DataType(DataType.Date)]

        public DateTime addDate { get; set; }
        public DateTime updateDate { get; set; }
        public bool active { get; set; }
    }
}
