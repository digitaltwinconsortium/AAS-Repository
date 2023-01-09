
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    // Note: In versions prior to V2.0.1, the SDK has "HasDataSpecification" containing only a Reference.
    // In 2.0.1, theoretically each entity with HasDataSpecification could also contain an EmbeddedDataSpecification.

    [DataContract]
    public class HasDataSpecification
    {
        [DataMember(Name = "embeddedDataSpecifications")]
        [XmlArray(ElementName = "embeddedDataSpecifications")]
        public List<EmbeddedDataSpecification> EmbeddedDataSpecifications { get; set; } = new();

        public HasDataSpecification() { }

        public HasDataSpecification(HasDataSpecification src)
        {
            foreach (var r in src.EmbeddedDataSpecifications)
                EmbeddedDataSpecifications.Add(r);
        }
    }
}
