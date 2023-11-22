using System.Xml.Serialization;

namespace Robi_N_WebAPI.Model.Request
{
    [XmlRootAttribute("requestSendSmsById")]
    public class requestSendSmsById
    {
        public string? gsmNumber { get; set; }
        public int messageId { get; set; }
    }
}
