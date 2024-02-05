using System.ComponentModel.DataAnnotations;

namespace Robi_N_WebAPI.Utility.Tables
{
    public class RBN_WhatsAppMessageTemplate
    {
        [Key]
        public int Id { get; set; }
        public int? SmsId { get; set; }
        public string? MessageBody { get; set; }
        public DateTime? add_date { get; set; }
        public DateTime? update_date { get; set; }
        public bool active { get; set; }
    }
}
