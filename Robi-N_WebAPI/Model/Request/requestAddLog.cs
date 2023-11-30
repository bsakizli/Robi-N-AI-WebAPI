using System.Xml.Serialization;

namespace Robi_N_WebAPI.Model.Request
{
    [XmlRoot(ElementName = "logs")]
    public class Logs
    {

        [XmlElement(ElementName = "key")]
        public string? key { get; set; }

        [XmlElement(ElementName = "value")]
        public string? value { get; set; }
    }

    [XmlRoot(ElementName = "requestAddLog")]
    public class requestAddLog
    {

        [XmlElement(ElementName = "uniqId")]
        public string? uniqId { get; set; }

        [XmlElement(ElementName = "logkey")]
        public int logkey { get; set; }

        [XmlElement(ElementName = "logs")]
        public List<Logs>? logs { get; set; }
    }

}
