
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class PolicyEnforcementPoint
    {
        [Required]
        [DataMember(Name="externalPolicyEnforcementPoint")]
        [XmlElement(ElementName = "externalPolicyEnforcementPoint")]
        public bool? ExternalPolicyEnforcementPoint { get; set; }
    }
}
