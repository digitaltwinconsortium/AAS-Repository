
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
        public List<EmbeddedDataSpecification> EmbeddedDataSpecifications { get; set; }

        [DataMember(Name = "qualifiers")]
        [XmlArray(ElementName = "qualifiers")]
        public List<Qualifier> Qualifiers { get; set; }

        [DataMember(Name = "semanticId")]
        [XmlElement(ElementName = "semanticId")]
        public Reference SemanticId { get; set; }

        [DataMember(Name = "kind")]
        [XmlElement(ElementName = "kind")]
        public ModelingKind Kind { get; set; }

        [DataMember(Name = "submodelElements")]
        [XmlArray(ElementName = "submodelElements")]
        public List<SubmodelElementWrapper> SubmodelElements { get; set; }

        [DataMember(Name = "HasDataSpecification")]
        [XmlElement(ElementName = "HasDataSpecification")]
        public HasDataSpecification HasDataSpecification { get; set; }

        public void SetSMEsParent()
        {
            if (SubmodelElements != null)
            {
                foreach (SubmodelElementWrapper smew in SubmodelElements)
                {
                    SetSMEParent(smew.SubmodelElement, this);
                }
            }
        }

        private void SetSMEParent(SubmodelElement sme, Referable parent)
        {
            if (sme == null)
            {
                return;
            }

            sme.Parent = parent;

            // recurse if needed
            if (sme is SubmodelElementCollection collection)
            {
                if (collection.Value != null)
                {
                    foreach (SubmodelElementWrapper smew in collection.Value)
                    {
                        SetSMEParent(smew.SubmodelElement, sme);
                    }
                }
            }
        }
    }
}
