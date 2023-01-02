
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class Endpoint
    {
        [Required]
        [DataMember(Name="interface")]
        public string Interface { get; set; }

        [Required]
        [DataMember(Name="protocolInformation")]
        public ProtocolInformation ProtocolInformation { get; set; }
    }
}
