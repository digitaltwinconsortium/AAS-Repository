
namespace AdminShell
{

    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class PolicyDecisionPoint
    {
        [Required]
        [DataMember(Name="externalPolicyDecisionPoints")]
        public bool? ExternalPolicyDecisionPoints { get; set; }
    }
}
