
namespace AdminShell
{
    using JsonSubTypes;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml;
    using System.Xml.Serialization;

    [DataContract]
    [JsonConverter(typeof(JsonSubtypes), "modelType.name")]
    public class Submodel : Identifiable
    {
        [DataMember(Name = "embeddedDataSpecifications")]
        [XmlArray(ElementName = "embeddedDataSpecifications")]
        public List<EmbeddedDataSpecification> EmbeddedDataSpecifications { get; set; } = new();

        [DataMember(Name = "qualifiers")]
        [XmlArray(ElementName = "qualifiers")]
        public List<Qualifier> Qualifiers { get; set; } = new();

        [DataMember(Name = "semanticId")]
        [XmlElement(ElementName = "semanticId")]
        public Reference SemanticId { get; set; } = new();

        [DataMember(Name = "kind")]
        [XmlElement(ElementName = "kind")]
        public ModelingKind Kind { get; set; } = new();

        [XmlArray(ElementName = "submodelElements")]
        public List<SubmodelElement> SubmodelElements { get; set; } = new();

        public Submodel(Submodel other)
            : base()
        {
            if (other == null)
            {
                return;
            }

            foreach (var ed in other.EmbeddedDataSpecifications)
            {
                EmbeddedDataSpecifications.Add(new EmbeddedDataSpecification(ed));
            }

            foreach (var q in other.Qualifiers)
            {
                Qualifiers.Add(new Qualifier(q));
            }

            SemanticId = new Reference(other.SemanticId);

            Kind = other.Kind;

            foreach (var sme in other.SubmodelElements)
            {
                SubmodelElements.Add(new SubmodelElement(sme));
            }
        }
    }
}
