
namespace AdminShell
{
    using System.Runtime.Serialization;

    [DataContract]
    public class Property : DataElement
    {
        [DataMember(Name = "Value")]
        [MetaModelName("Property.Value")]
        public string Value { get; set; }

        [DataMember(Name = "valueId")]
        public GlobalReference ValueId { get; set; }

        [DataMember(Name = "valueType")]
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

