
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml;
    using System.Xml.Serialization;

    [DataContract]
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

        // Important note: XML serialization uses Submodel Element Wrappers while JSON serialization does not!
        // So we have to first deserialize into a placeholder Json member and then copy the contents into the correct member
        [XmlArray(ElementName = "submodelElements")]
        public List<SubmodelElementWrapper> SubmodelElements { get; set; } = new();

        [XmlIgnore]
        [DataMember(Name = "submodelElements")]
        public SubmodelElement[] JsonSubmodelElements
        {
            get
            {
                var submodelElements = new List<SubmodelElement>();

                foreach (SubmodelElementWrapper smew in SubmodelElements)
                {
                    submodelElements.Add(smew.SubmodelElement);
                }

                return submodelElements.ToArray();
            }

            set
            {
                if (value != null)
                {
                    SubmodelElements.Clear();

                    foreach (SubmodelElement sme in value)
                    {
                        SubmodelElements.Add(new SubmodelElementWrapper() { SubmodelElement = sme });
                    }
                }
            }
        }
    }
}
