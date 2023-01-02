
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class PolicyAdministrationPoint
    {
        [Required]
        [DataMember(Name="externalAccessControl")]
        public bool? ExternalAccessControl { get; set; }

        [DataMember(Name="localAccessControl")]
        public AccessControl LocalAccessControl { get; set; }
    }
}
