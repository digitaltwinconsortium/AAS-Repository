
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace AdminShell
{
    [DataContract]
    public class Extension : HasSemantics
    {
        [Required]
        [DataMember(Name="name")]
        public string Name { get; set; } = string.Empty;

        [DataMember(Name="refersTo")]
        public List<ModelReference> RefersTo { get; set; } = new();

        [DataMember(Name = "Value")]
        public string Value { get; set; } = string.Empty;

        [DataMember(Name = "valueType")]
        public ValueTypeEnum ValueType { get; set; } = ValueTypeEnum.StringEnum;
    }
}
