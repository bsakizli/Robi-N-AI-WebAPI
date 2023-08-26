namespace Robi_N_WebAPI.Model.Service.Request
{
    public class ServiceRequestAddAutoCall
    {
        public class Request
        {
            public class Body
            {
                public string @event { get; set; }
                public Data data { get; set; }
            }

            public class Data
            {
                public string list_name { get; set; }
                public string list_prefix { get; set; }
                public int liste_type { get; set; }
                public string list_startdate { get; set; }
                public string list_stopdate { get; set; }
                public string list_starttime { get; set; }
                public string list_stoptime { get; set; }
                public int retry_count { get; set; }
                public int try_time { get; set; }
                public int queue_limit_type { get; set; }
                public string department { get; set; }
                public string trunk { get; set; }
                public int callstop_type { get; set; }
                public string destination_type { get; set; }
                public string destination_name { get; set; }
                public int queue_limit { get; set; }
                public List<string> groups { get; set; }
                public List<Number> numbers { get; set; }
            }

            public class Header
            {
                public string username { get; set; }
                public string password { get; set; }
            }

            public class Number
            {
                public object number { get; set; }
                public string name { get; set; }
            }

            public class Root
            {
                public Header header { get; set; }
                public Body body { get; set; }
            }
        }
    }
}
