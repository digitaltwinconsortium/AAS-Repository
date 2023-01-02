
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class PolicyInformationPoints
    {
        [Required]
        [DataMember(Name="externalInformationPoint")]
        public bool? ExternalInformationPoint { get; set; }

        [DataMember(Name="internalInformationPoint")]
        public List<Reference> InternalInformationPoint { get; set; }
    }
}
