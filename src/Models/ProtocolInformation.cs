
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class ProtocolInformation
    {
        [Required]
        [DataMember(Name="endpointAddress")]
        [XmlElement(ElementName = "endpointAddress")]
        public string EndpointAddress { get; set; }

        [DataMember(Name="endpointProtocol")]
        [XmlElement(ElementName = "endpointProtocol")]
        public string EndpointProtocol { get; set; }

        [DataMember(Name="endpointProtocolVersion")]
        [XmlElement(ElementName = "endpointProtocolVersion")]
        public string EndpointProtocolVersion { get; set; }

        [DataMember(Name="subprotocol")]
        [XmlElement(ElementName = "subprotocol")]
        public string Subprotocol { get; set; }

        [DataMember(Name="subprotocolBody")]
        [XmlElement(ElementName = "subprotocolBody")]
        public string SubprotocolBody { get; set; }

        [DataMember(Name="subprotocolBodyEncoding")]
        [XmlElement(ElementName = "subprotocolBodyEncoding")]
        public string SubprotocolBodyEncoding { get; set; }
    }
}
