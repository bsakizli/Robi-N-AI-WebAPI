namespace Robi_N_WebAPI.Model.Service.Response
{
    public class ServiceResponseAddAutoCall
    {
        public class Body
        {
            public string id { get; set; }
            public string list_name { get; set; }
            public string list_prefix { get; set; }
            public int list_type { get; set; }
            public string? list_startdate { get; set; }
            public string? list_stopdate { get; set; }
            public string list_starttime { get; set; }
            public string list_stoptime { get; set; }
            public int retry_count { get; set; }
            public int try_time { get; set; }
            public string department { get; set; }
            public string trunk { get; set; }
            public string destination_type { get; set; }
            public string destination_name { get; set; }
            public int queue_limit { get; set; }
            public int queue_limit_type { get; set; }
            public int callstop_type { get; set; }
            public List<object> groups { get; set; }
        }

        public class Header
        {
            public bool error { get; set; }
            public int code { get; set; }
            public string message { get; set; }
        }

        public class Root
        {
            public Header header { get; set; }
            public List<Body> body { get; set; }
        }
    }
}
