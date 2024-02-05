using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsAppBusinessAPI.Model
{
    public class SendTextMessageRequest
    {
        public int? typing_time { get; set; }
        public string? to { get; set; }
        //public string? quoted { get; set; }
        //public int? ephemeral { get; set; }
        //public string? edit { get; set; }
        public string? body { get; set; }
        //public bool? no_link_preview { get; set; }
        //public List<string>? mentions { get; set; }
        //public bool? view_once { get; set; }
    }
}
