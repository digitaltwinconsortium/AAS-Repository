
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class ProtocolInformation
    {
        [Required]
        [DataMember(Name="endpointAddress")]
        public string EndpointAddress { get; set; }

        [DataMember(Name="endpointProtocol")]
        public string EndpointProtocol { get; set; }

        [DataMember(Name="endpointProtocolVersion")]
        public string EndpointProtocolVersion { get; set; }

        [DataMember(Name="subprotocol")]
        public string Subprotocol { get; set; }

        [DataMember(Name="subprotocolBody")]
        public string SubprotocolBody { get; set; }

        [DataMember(Name="subprotocolBodyEncoding")]
        public string SubprotocolBodyEncoding { get; set; }
    }
}
