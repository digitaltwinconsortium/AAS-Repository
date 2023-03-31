
namespace AdminShell
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class SubmodelElementList : SubmodelElementCollection
    {
        [DataMember(Name = "semanticIdValues")]
        [XmlElement(ElementName = "semanticIdValues")]
        public Reference SemanticIdValues { get; set; } = new();

        [DataMember(Name = "submodelElementTypeValues")]
        [XmlElement(ElementName = "submodelElementTypeValues")]
        public ModelTypes SubmodelElementTypeValues { get; set; } = new();

        [DataMember(Name = "valueTypeValues")]
        [XmlElement(ElementName = "valueTypeValues")]
        public string ValueTypeValues { get; set; }

        [DataMember(Name = "orderRelevant")]
        [XmlElement(ElementName = "orderRelevant")]
        public bool OrderRelevant = false;

        [DataMember(Name = "semanticIdListElement")]
        [XmlElement(ElementName = "semanticIdListElement")]
        public SemanticId SemanticIdListElement { get; set; } = new();

        [DataMember(Name = "typeValueListElement")]
        [XmlElement(ElementName = "typeValueListElement")]
        public string TypeValueListElement { get; set; }

        [DataMember(Name = "valueTypeListElement")]
        [XmlElement(ElementName = "valueTypeListElement")]
        public string ValueTypeListElement { get; set; }

        public SubmodelElementList()
        {
            ModelType = ModelTypes.SubmodelElementList;
        }

        public SubmodelElementList(SubmodelElement src)
            : base(src)
        {
            if (!(src is SubmodelElementList sml))
            {
                return;
            }

            OrderRelevant = sml.OrderRelevant;
            ModelType = ModelTypes.SubmodelElementList;

            if (sml.SemanticIdListElement != null)
            {
                SemanticIdListElement = new SemanticId(sml.SemanticIdListElement);
            }

            TypeValueListElement = sml.TypeValueListElement;
            ValueTypeListElement = sml.ValueTypeListElement;
        }
    }
}

