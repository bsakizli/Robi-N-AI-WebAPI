using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Robi_N_WebAPI.Utility.Tables
{
    [Index(nameof(Id), nameof(uniqId), IsUnique = true)]
    public class RBN_IVR_LOGS
    {
        [Key]
        public int Id { get; set; }
        public string? uniqId { get; set; }
        public string? log { get; set; }
        public DateTime? addDate { get; set; }
        public bool active { get; set; }
    }
}
