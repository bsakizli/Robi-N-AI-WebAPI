using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailEntity.Models
{
    public class EmailReports
    {
        public string KimlikNumarasi { get; set; }
        public string? AdSoyad { get; set; }
        public long? MedulaRaporId { get; set; }
        public string? RaporTakipNumarasi { get; set; }
        public long? OnayReferansId { get; set; }
        public int? FirmCode { get; set; }
        public DateTime? RaporBaslamaTarihi { get; set; }
        public DateTime? RaporBirisTarihi { get; set; }
    }
}
