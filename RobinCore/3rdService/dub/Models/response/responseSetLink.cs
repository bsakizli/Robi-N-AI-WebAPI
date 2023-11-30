using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobinCore._3rdService.dub.Models.response
{
    public class responseSetLink
    {
        public class Root
        {
            public string? id { get; set; }
            public string? domain { get; set; }
            public string? key { get; set; }
            public string? url { get; set; }
            public bool archived { get; set; }
            public object? expiresAt { get; set; }
            public object? password { get; set; }
            public bool proxy { get; set; }
            public string? title { get; set; }
            public string? description { get; set; }
            public object? image { get; set; }
            public object? utm_source { get; set; }
            public object? utm_medium { get; set; }
            public object? utm_campaign { get; set; }
            public object? utm_term { get; set; }
            public object? utm_content { get; set; }
            public bool rewrite { get; set; }
            public object? ios { get; set; }
            public object? android { get; set; }
            public object? geo { get; set; }
            public string? userId { get; set; }
            public string? projectId { get; set; }
            public bool publicStats { get; set; }
            public int clicks { get; set; }
            public object? lastClicked { get; set; }
            public bool checkDisabled { get; set; }
            public object? lastChecked { get; set; }
            public DateTime createdAt { get; set; }
            public DateTime updatedAt { get; set; }
            public object? tagId { get; set; }
            public object? comments { get; set; }
        }
    }
}
