using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsAppBusinessAPI.WhatsAppBusinessModel
{
    public class SendTextMessage
    {
        public class Context
        {
            public List<string> mentions { get; set; }
            public string? quoted_id { get; set; }
            public string? quoted_author { get; set; }
            public QuotedContent? quoted_content { get; set; }
            public string? quoted_type { get; set; }
        }

        public class Message
        {
            public string? id { get; set; }
            public bool from_me { get; set; }
            public string? type { get; set; }
            public string? chat_id { get; set; }
            public int timestamp { get; set; }
            public string? source { get; set; }
            public int device_id { get; set; }
            public string? status { get; set; }
            public Text? text { get; set; }
            public Context? context { get; set; }
            public string? from { get; set; }
        }

        public class QuotedContent
        {
            public string? body { get; set; }
        }

        public class Root
        {
            public bool sent { get; set; }
            public Message? message { get; set; }
        }

        public class Text
        {
            public string? body { get; set; }
        }
    }
    
}
