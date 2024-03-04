using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobinCore._3rdService.SuccessFactors.Models
{
    public class responsePersonal
    {
        public class Root
        {
            public string? Adi { get; set; }
            public string? Soyadi { get; set; }
            public DateTime? GirisTarihi { get; set; }
            public DateTime? CikisTarihi { get; set; }
            public string? TcKimlikNo { get; set; }
            public string? Attribute4 { get; set; }
            public bool? Aktif { get; set; }
        }
    }
}
