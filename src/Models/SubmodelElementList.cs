
namespace AdminShell
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class SubmodelElementList : SubmodelElementCollection
    {
        [DataMember(Name = "semanticIdValues")]
        [XmlElement(ElementName = "semanticIdValues")]
        public Reference SemanticIdValues { get; set; }

        [DataMember(Name = "submodelElementTypeValues")]
        [XmlElement(ElementName = "submodelElementTypeValues")]
        public ModelType SubmodelElementTypeValues { get; set; }

        [DataMember(Name = "valueTypeValues")]
        [XmlElement(ElementName = "valueTypeValues")]
        public ValueTypeEnum ValueTypeValues { get; set; }

        [DataMember(Name = "orderRelevant")]
        [XmlElement(ElementName = "orderRelevant")]
        public bool OrderRelevant = false;

        [DataMember(Name = "semanticIdListElement")]
        [XmlElement(ElementName = "semanticIdListElement")]
        public SemanticId SemanticIdListElement { get; set; }

        [DataMember(Name = "typeValueListElement")]
        [XmlElement(ElementName = "typeValueListElement")]
        public string TypeValueListElement { get; set; }

        [DataMember(Name = "valueTypeListElement")]
        [XmlElement(ElementName = "valueTypeListElement")]
        public string ValueTypeListElement { get; set; }

        public SubmodelElementList() { }

        public SubmodelElementList(SubmodelElement src, bool shallowCopy = false)
            : base(src, shallowCopy)
        {
            if (!(src is SubmodelElementList sml))
                return;

            OrderRelevant = sml.OrderRelevant;

            if (sml.SemanticIdListElement != null)
                SemanticIdListElement = new SemanticId(sml.SemanticIdListElement);

            TypeValueListElement = sml.TypeValueListElement;
            ValueTypeListElement = sml.ValueTypeListElement;
        }
    }
}

