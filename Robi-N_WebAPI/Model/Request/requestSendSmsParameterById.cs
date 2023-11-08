namespace Robi_N_WebAPI.Model.Request
{
    public class requestSendSmsParameterById
    {
        public long messageId { get; set; }
        public string gsmNumber { get; set; }
        public List<smsParameter>? parameters { get; set; }

    }

    public class smsParameter
    {
        public string? key { get; set; }
        public string? value { get; set; }
    }
}
