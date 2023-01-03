
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Serialization;

    public class ConceptDictionary : Referable
    {
        [XmlElement(ElementName = "ConceptDescriptions")]
        public List<Reference> conceptDescriptionsRefs = new();
    }
}
