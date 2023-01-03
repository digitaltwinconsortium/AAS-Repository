
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class SubmodelElementList : SubmodelElementCollection
    {
        [DataMember(Name = "semanticIdValues")]
        public Reference SemanticIdValues { get; set; }

        [DataMember(Name = "submodelElementTypeValues")]
        public ModelType SubmodelElementTypeValues { get; set; }

        [DataMember(Name = "Value")]
        public List<SubmodelElement> Value { get; set; }

        [DataMember(Name = "valueTypeValues")]
        public ValueTypeEnum ValueTypeValues { get; set; }

        public bool orderRelevant = false;

        public SemanticId semanticIdListElement = null;

        public string typeValueListElement = null;

        public string valueTypeListElement = null;

        public SubmodelElementList() { }

        public SubmodelElementList(SubmodelElement src, bool shallowCopy = false)
            : base(src, shallowCopy)
        {
            if (!(src is SubmodelElementList sml))
                return;

            orderRelevant = sml.orderRelevant;

            if (sml.semanticIdListElement != null)
                semanticIdListElement = new SemanticId(sml.semanticIdListElement);

            typeValueListElement = sml.typeValueListElement;
            valueTypeListElement = sml.valueTypeListElement;
        }
    }
}

