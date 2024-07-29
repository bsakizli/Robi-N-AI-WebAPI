using System.ComponentModel.DataAnnotations;

namespace Robi_N_WebAPI.Utility.Tables
{
    public class RBN_SMS_TEMPLATES
    {
        [Key]
        public int Id { get; set; }
        public int? MessageCode { get; set; }
        public string? Message { get; set; }
        public DateTime addDate { get; set; }
        public DateTime updateDate { get; set; }
        public bool whatsappSend { get; set; }
        public bool active { get; set; }
    }
}
