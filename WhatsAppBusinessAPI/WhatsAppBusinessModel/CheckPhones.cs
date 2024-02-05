using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsAppBusinessAPI.WhatsAppBusinessModel
{
    public class Contact
    {
        public string? input { get; set; }
        public string? status { get; set; }
        public string? wa_id { get; set; }
    }

    public class CheckPhones
    {
        public List<Contact>? contacts { get; set; }
    }
}
