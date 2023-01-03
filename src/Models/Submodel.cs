
namespace AdminShell
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml;
    using System.Xml.Serialization;

    [DataContract]
    public class Submodel : Identifiable
    {
        [DataMember(Name = "embeddedDataSpecifications")]
        public List<EmbeddedDataSpecification> EmbeddedDataSpecifications { get; set; }

        [DataMember(Name = "qualifiers")]
        public List<Qualifier> Qualifiers { get; set; }

        [DataMember(Name = "semanticId")]
        public Reference SemanticId { get; set; }

        [DataMember(Name = "kind")]
        public ModelingKind Kind { get; set; }

        [DataMember(Name = "submodelElements")]
        public List<SubmodelElement> SubmodelElements { get; set; }

        [XmlElement(ElementName = "kind")]
        public ModelingKind kind = new ModelingKind();

        [XmlElement(ElementName = "semanticId")]
        public SemanticId semanticId = new SemanticId();

        public SemanticId GetSemanticId() { return semanticId; }

        [XmlElement(ElementName = "EmbeddedDataSpecification")]
        public HasDataSpecification hasDataSpecification = null;

        [XmlIgnore]
        private List<SubmodelElementWrapper> _submodelElements = null;

        public List<SubmodelElementWrapper> submodelElements
        {
            get { return _submodelElements; }
            set { _submodelElements = value; }
        }

        public static void SetParentsForSME(Referable parent, SubmodelElement se)
        {
            if (se == null)
                return;

            se.parent = parent;

            var childs = se.EnumerateChildren();
            if (childs != null)
                foreach (var c in childs)
                    SetParentsForSME(se, c.submodelElement);
        }

        public void SetAllParents()
        {
            if (this.submodelElements != null)
                foreach (var sme in this.submodelElements)
                    SetParentsForSME(this, sme.submodelElement);
        }

        private static void SetParentsForSME(Referable parent, SubmodelElement se, DateTime timeStamp)
        {
            if (se == null)
                return;

            se.parent = parent;
            se.TimeStamp = timeStamp;
            se.TimeStampCreate = timeStamp;

            var childs = se.EnumerateChildren();
            if (childs != null)
                foreach (var c in childs)
                    SetParentsForSME(se, c.submodelElement, timeStamp);
        }
    }
}
