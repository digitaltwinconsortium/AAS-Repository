
namespace AdminShell
{

    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class PolicyDecisionPoint
    {
        [Required]
        [DataMember(Name="externalPolicyDecisionPoints")]
        [XmlElement(ElementName = "externalPolicyDecisionPoints")]
        public bool? ExternalPolicyDecisionPoints { get; set; }
    }
}
