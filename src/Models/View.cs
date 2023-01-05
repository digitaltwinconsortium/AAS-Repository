
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class View : Referable
    {
        [DataMember(Name = "embeddedDataSpecifications")]
        [XmlArray(ElementName = "embeddedDataSpecifications")]
        public List<EmbeddedDataSpecification> EmbeddedDataSpecifications { get; set; } = new();

        [DataMember(Name = "containedElements")]
        [XmlArray(ElementName = "containedElements")]
        public List<Reference> ContainedElements { get; set; } = new();

        [DataMember(Name = "semanticId")]
        [XmlElement(ElementName = "semanticId")]
        public SemanticId SemanticId { get; set; } = new();

        [DataMember(Name = "hasDataSpecification")]
        [XmlElement(ElementName = "hasDataSpecification")]
        public HasDataSpecification HasDataSpecification { get; set; } = new();
    }
}
