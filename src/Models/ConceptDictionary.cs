
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Serialization;

    public class ConceptDictionary : Referable
    {
        [XmlElement(ElementName = "ConceptDescriptions")]
        public List<Reference> conceptDescriptionsRefs = new();

        public ConceptDictionary() { }

        public static ConceptDictionary CreateNew(string idShort = null)
        {
            var d = new ConceptDictionary();

            if (idShort != null)
                d.IdShort = idShort;

            return d;
        }

        public AasElementSelfDescription GetSelfDescription()
        {
            return new AasElementSelfDescription("ConceptDictionary", "CDic");
        }
    }
}
