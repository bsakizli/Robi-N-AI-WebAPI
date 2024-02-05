using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsAppBusinessAPI.Model
{
    public class CheckPhonesRequest
    {
        public string? blocking { get; set; }
        public bool force_check { get; set; }
        public List<string>? contacts { get; set; }
    }
}
