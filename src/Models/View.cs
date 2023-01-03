
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class View : Referable
    {
        [DataMember(Name = "embeddedDataSpecifications")]
        public List<EmbeddedDataSpecification> EmbeddedDataSpecifications { get; set; } = new();

        [XmlIgnore]
        [DataMember(Name = "containedElements")]
        public List<Reference> ContainedElements { get; set; } = new();

        [DataMember(Name = "semanticId")]
        public SemanticId SemanticId { get; set; } = new();

        [DataMember(Name = "hasDataSpecification")]
        public HasDataSpecification HasDataSpecification { get; set; } = new();
    }
}
