namespace Robi_N_WebAPI.Model.Response
{
    public class ResponseEmptorTicketCreate
    {
        public bool status { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public TicketDetails ticket { get; set; }

    }

    public class TicketDetails
    {
        public string id { get; set; }
    }
}
