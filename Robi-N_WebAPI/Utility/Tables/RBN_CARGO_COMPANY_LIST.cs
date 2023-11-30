using System.ComponentModel.DataAnnotations;

namespace Robi_N_WebAPI.Utility.Tables
{
    public class RBN_CARGO_COMPANY_LIST
    {
        [Key]
        public int Id { get; set; }
        public string? cargoName { get; set; }
        public string? trackingUrl { get; set; }
        public int validityPeriod { get; set; }
        public DateTime addDate { get; set; }
        public DateTime updateDate { get; set; }
        public bool active { get; set; }
    }
}
