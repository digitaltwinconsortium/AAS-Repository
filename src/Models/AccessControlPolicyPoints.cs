
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class AccessControlPolicyPoints
    {
        [Required]
        [DataMember(Name="policyAdministrationPoint")]
        public PolicyAdministrationPoint PolicyAdministrationPoint { get; set; }

        [Required]
        [DataMember(Name="policyDecisionPoint")]
        public PolicyDecisionPoint PolicyDecisionPoint { get; set; }

        [Required]
        [DataMember(Name="policyEnforcementPoint")]
        public PolicyEnforcementPoint PolicyEnforcementPoint { get; set; }

        [DataMember(Name="policyInformationPoints")]
        public PolicyInformationPoints PolicyInformationPoints { get; set; }
    }
}
