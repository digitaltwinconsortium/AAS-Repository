
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class RelationshipElement : SubmodelElement
    {
        [Required]
        [DataMember(Name="first")]
        public Reference First { get; set; }

        [Required]
        [DataMember(Name="second")]
        public Reference Second { get; set; }

    }
}
