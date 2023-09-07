using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudUtility.Model
{
    public class textToSpech
    {
        public class response
        {
            public bool status { get; set; }
            public Byte[] AudioContent { get; set; }
            public string base64Content { get; set; }
        }
    }
}
