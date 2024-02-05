using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsAppBusinessAPI.Model
{
    public class SendLocationMessageRequest
    {
        public string? to { get; set; }
        public bool view_once { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string? name { get; set; }
        public string? url { get; set; }
    }
}
