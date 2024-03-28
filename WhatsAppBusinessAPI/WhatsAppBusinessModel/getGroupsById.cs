using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsAppBusinessAPI.WhatsAppBusinessModel
{
    public class getGroupsById
    {
        public class Image
        {
            public string? id { get; set; }
            public string? mime_type { get; set; }
            public int file_size { get; set; }
            public string? sha256 { get; set; }
            public string? caption { get; set; }
            public string? preview { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class LastMessage
        {
            public string? id { get; set; }
            public string? type { get; set; }
            public string? chat_id { get; set; }
            public string? from { get; set; }
            public bool from_me { get; set; }
            public string? from_name { get; set; }
            public string? source { get; set; }
            public int timestamp { get; set; }
            public int device_id { get; set; }
            public Image image { get; set; }
        }

        public class Participant
        {
            public string? id { get; set; }
            public string? rank { get; set; }
        }

        public class Root
        {
            public string? id { get; set; }
            public string? name { get; set; }
            public string? type { get; set; }
            public int timestamp { get; set; }
            public string? chat_pic { get; set; }
            public int unread { get; set; }
            public bool not_spam { get; set; }
            public LastMessage last_message { get; set; }
            public List<Participant> participants { get; set; }
            public int name_at { get; set; }
            public int created_at { get; set; }
            public string? created_by { get; set; }
        }
    }
}
