
namespace AdminShell
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public partial class ValueObject
    {
        [DataMember(Name="value")]
        [XmlElement(ElementName = "value")]
        public string Value { get; set; }

        [DataMember(Name="valueId")]
        [XmlElement(ElementName = "valueId")]
        public Reference ValueId { get; set; }

        [DataMember(Name="valueType")]
        [XmlElement(ElementName = "valueType")]
        public ValueTypeEnum ValueType { get; set; }
     }
}
