using System.Xml.Serialization;

namespace Robi_N_WebAPI.Model.Request
{


    [XmlRoot(ElementName = "parameters")]
    public class smsParameter
    {

        [XmlElement(ElementName = "key")]
        public string? key { get; set; }

        [XmlElement(ElementName = "value")]
        public string? value { get; set; }
    }

    [XmlRoot(ElementName = "requestSendSmsParameterById")]
    public class requestSendSmsParameterById
    {

        [XmlElement(ElementName = "messageId")]
        public int messageId { get; set; }

        [XmlElement(ElementName = "gsmNumber")]
        public string? gsmNumber { get; set; }

        [XmlElement(ElementName = "parameters")]
        public List<smsParameter>? parameters { get; set; }
    }
}
