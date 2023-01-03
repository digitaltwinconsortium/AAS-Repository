
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class Qualifier : IAasElement
    {
        [DataMember(Name = "Value")]
        [MetaModelName("Qualifier.Value")]
        public string Value { get; set; }

        [DataMember(Name = "valueId")]
        public GlobalReference ValueId { get; set; }

        [DataMember(Name = "valueType")]
        [MetaModelName("Qualifier.valueType")]
        public ValueTypeEnum ValueType { get; set; }

        [Required]
        [DataMember(Name = "type")]
        [MetaModelName("Qualifier.type")]
        public string Type { get; set; }

        [XmlElement(ElementName = "semanticId")]
        public SemanticId semanticId = null;

        public SemanticId GetSemanticId() { return semanticId; }

        public Qualifier() { }

        public Qualifier(Qualifier src)
        {
            if (src.semanticId != null)
                semanticId = new SemanticId(src.semanticId);

            this.Type = src.Type;

            this.Value = src.Value;

            if (src.ValueId != null)
                this.ValueId = new GlobalReference(src.ValueId);
        }
    }
}
