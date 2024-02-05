using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsAppBusinessAPI.WhatsAppBusinessModel
{
    public class SendLocationMessage
    {
        public class Location
        {
            public double latitude { get; set; }
            public double longitude { get; set; }
            public string name { get; set; }
            public string url { get; set; }
        }

        public class Message
        {
            public string? id { get; set; }
            public bool from_me { get; set; }
            public string? type { get; set; }
            public string? chat_id { get; set; }
            public int? timestamp { get; set; }
            public string? source { get; set; }
            public int? device_id { get; set; }
            public string? status { get; set; }
            public Location location { get; set; }
            public string? from { get; set; }
        }

        public class Root
        {
            public bool sent { get; set; }
            public Message? message { get; set; }
        }
    }
}
