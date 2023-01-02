
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class Range : SubmodelElement
    {
        [DataMember(Name="max")]
        public string Max { get; set; }

        [DataMember(Name="min")]
        public string Min { get; set; }

        [Required]
        [DataMember(Name="valueType")]
        public ValueTypeEnum ValueType { get; set; }
    }
}
