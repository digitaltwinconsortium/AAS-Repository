
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class PolicyEnforcementPoint
    {
        [Required]
        [DataMember(Name="externalPolicyEnforcementPoint")]
        public bool? ExternalPolicyEnforcementPoint { get; set; }
    }
}
