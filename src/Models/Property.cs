
namespace AdminShell
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class Property : DataElement
    {
        [DataMember(Name = "value")]
        [XmlElement(ElementName = "value")]
        [MetaModelName("Property.value")]
        public string Value { get; set; }

        [DataMember(Name = "valueId")]
        [XmlElement(ElementName = "valueId")]
        public GlobalReference ValueId { get; set; }

        [DataMember(Name = "valueType")]
        [XmlElement(ElementName = "valueType")]
        [MetaModelName("Property.valueType")]
        public ValueTypeEnum ValueType { get; set; }

        public Property() { }

        public Property(SubmodelElement src)
            : base(src)
        {
            if (!(src is Property p))
                return;

            ValueType = p.ValueType;

            Value = p.Value;

            if (p.ValueId != null)
                ValueId = new GlobalReference(p.ValueId);
        }
    }
}

