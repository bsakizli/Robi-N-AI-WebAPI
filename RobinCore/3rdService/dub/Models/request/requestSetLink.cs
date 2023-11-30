using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobinCore._3rdService.dub.Models.request
{
    public class requestSetLink
    {
        public class Root
        {
            public string? domain { get; set; }
            public string? url { get; set; }
            public bool archived { get; set; }
            public string? expiresAt { get; set; }
            public object? password { get; set; }
            public bool proxy { get; set; }
            public string? title { get; set; }
            public string? description { get; set; }
            public object? image { get; set; }
            public bool rewrite { get; set; }
            public object? ios { get; set; }
            public object? android { get; set; }
            public object? geo { get; set; }
            public bool publicStats { get; set; }
            public string? tagId { get; set; }
            public object? comments { get; set; }
        }

    }
}
