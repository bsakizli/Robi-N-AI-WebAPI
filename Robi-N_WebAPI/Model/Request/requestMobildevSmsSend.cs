namespace Robi_N_WebAPI.Model.Request
{
    public class requestMobildevSmsSend
    {
        public class Root
        {
            public string? UserName { get; set; }
            public string? PassWord { get; set; }
            public int Action { get; set; }
            public string? Mesgbody { get; set; }
            public List<string>? Numbers { get; set; }
            public string? AccountId { get; set; }
            public string? Originator { get; set; }
            public int Encoding { get; set; }
            public string? MessageType { get; set; }
        }


    }
}
