
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class Endpoint
    {
        [Required]
        [DataMember(Name="interface")]
        [XmlElement(ElementName = "interface")]
        public string Interface { get; set; }

        [Required]
        [DataMember(Name="protocolInformation")]
        [XmlElement(ElementName = "protocolInformation")]
        public ProtocolInformation ProtocolInformation { get; set; }
    }
}
