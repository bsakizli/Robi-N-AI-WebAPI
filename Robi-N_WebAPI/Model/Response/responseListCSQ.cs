using static Robi_N_WebAPI.Model.Xml.Response.responseCSQList;

namespace Robi_N_WebAPI.Model.Response
{
    public class responseListCSQ:GlobalResponse
    {
       

        public class csq
        {
            public string? name { get; set; }
        }

        public List<csq>? csqs { get; set; }
    }
}
