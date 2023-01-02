
namespace AdminShell
{
    using System.Runtime.Serialization;

    [DataContract]
    public class Property : SubmodelElement
    {
        [DataMember(Name="Value")]
        public string Value { get; set; }

        [DataMember(Name="ValueId")]
        public Reference ValueId { get; set; }

        [DataMember(Name="valueType")]
        public ValueTypeEnum ValueType { get; set; }
    }
}
