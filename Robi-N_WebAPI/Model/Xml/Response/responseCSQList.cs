using System.Xml.Serialization;

namespace Robi_N_WebAPI.Model.Xml.Response
{
    public class responseCSQList
    {
        [XmlRoot(ElementName = "skillNameUriPair")]
        public class SkillNameUriPair
        {
            [XmlElement(ElementName = "refURL")]
            public string RefURL { get; set; }
            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }
        }

        [XmlRoot(ElementName = "skillCompetency")]
        public class SkillCompetency
        {
            [XmlElement(ElementName = "competencelevel")]
            public string Competencelevel { get; set; }
            [XmlElement(ElementName = "skillNameUriPair")]
            public SkillNameUriPair SkillNameUriPair { get; set; }
            [XmlElement(ElementName = "weight")]
            public string Weight { get; set; }
        }

        [XmlRoot(ElementName = "skillGroup")]
        public class SkillGroup
        {
            [XmlElement(ElementName = "skillCompetency")]
            public SkillCompetency SkillCompetency { get; set; }
            [XmlElement(ElementName = "selectionCriteria")]
            public string SelectionCriteria { get; set; }
        }

        [XmlRoot(ElementName = "poolSpecificInfo")]
        public class PoolSpecificInfo
        {
            [XmlElement(ElementName = "skillGroup")]
            public SkillGroup SkillGroup { get; set; }
        }

        [XmlRoot(ElementName = "csq")]
        public class Csq
        {
            [XmlElement(ElementName = "self")]
            public string Self { get; set; }
            [XmlElement(ElementName = "id")]
            public string Id { get; set; }
            [XmlElement(ElementName = "name")]
            public string Name { get; set; }
            [XmlElement(ElementName = "queueType")]
            public string QueueType { get; set; }
            [XmlElement(ElementName = "routingType")]
            public string RoutingType { get; set; }
            [XmlElement(ElementName = "queueAlgorithm")]
            public string QueueAlgorithm { get; set; }
            [XmlElement(ElementName = "autoWork")]
            public string AutoWork { get; set; }
            [XmlElement(ElementName = "wrapupTime")]
            public string WrapupTime { get; set; }
            [XmlElement(ElementName = "resourcePoolType")]
            public string ResourcePoolType { get; set; }
            [XmlElement(ElementName = "serviceLevel")]
            public string ServiceLevel { get; set; }
            [XmlElement(ElementName = "serviceLevelPercentage")]
            public string ServiceLevelPercentage { get; set; }
            [XmlElement(ElementName = "poolSpecificInfo")]
            public PoolSpecificInfo PoolSpecificInfo { get; set; }
            [XmlElement(ElementName = "accountUserId")]
            public string AccountUserId { get; set; }
            [XmlElement(ElementName = "accountPassword")]
            public string AccountPassword { get; set; }
            [XmlElement(ElementName = "channelProvider")]
            public ChannelProvider ChannelProvider { get; set; }
            [XmlElement(ElementName = "pollingInterval")]
            public string PollingInterval { get; set; }
            [XmlElement(ElementName = "folderName")]
            public string FolderName { get; set; }
            [XmlElement(ElementName = "snapshotAge")]
            public string SnapshotAge { get; set; }
        }

        [XmlRoot(ElementName = "channelProvider")]
        public class ChannelProvider
        {
            [XmlElement(ElementName = "refURL")]
            public string RefURL { get; set; }
            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }
        }

        [XmlRoot(ElementName = "csqs")]
        public class Csqs
        {
            [XmlElement(ElementName = "csq")]
            public List<Csq> Csq { get; set; }
        }
    }
}
