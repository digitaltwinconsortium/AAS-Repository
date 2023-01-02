
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class Qualifier : HasSemantics
    {
        [DataMember(Name="Value")]
        public string Value { get; set; }

        [DataMember(Name="ValueId")]
        public Reference ValueId { get; set; }

        [DataMember(Name="valueType")]
        public ValueTypeEnum ValueType { get; set; }

        [Required]
        [DataMember(Name="Type")]
        public string Type { get; set; }
    }
}
