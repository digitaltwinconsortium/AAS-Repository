
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class AccessControlPolicyPoints
    {
        [Required]
        [DataMember(Name="policyAdministrationPoint")]
        [XmlElement(ElementName= "policyAdministrationPoint")]
        public PolicyAdministrationPoint PolicyAdministrationPoint { get; set; }

        [Required]
        [DataMember(Name="policyDecisionPoint")]
        [XmlElement(ElementName = "policyDecisionPoint")]
        public PolicyDecisionPoint PolicyDecisionPoint { get; set; }

        [Required]
        [DataMember(Name="policyEnforcementPoint")]
        [XmlElement(ElementName = "policyEnforcementPoint")]
        public PolicyEnforcementPoint PolicyEnforcementPoint { get; set; }

        [DataMember(Name="policyInformationPoints")]
        [XmlElement(ElementName = "policyInformationPoints")]
        public PolicyInformationPoints PolicyInformationPoints { get; set; }
    }
}
