
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

        public new static SubmodelElementList CreateNew(
            string idShort = null, string category = null, Identifier semanticIdKey = null)
        {
            var x = new SubmodelElementList();
            x.CreateNewLogic(idShort, category, semanticIdKey);
            return (x);
        }

        public class ConstraintStat
        {
            /// <summary>
            /// Constraint AASd-107: If a first level child element in a SubmodelElementList has a semanticId
            /// it shall be identical to SubmodelElementList/semanticIdListElement.
            /// </summary>
            public bool AllChildSemIdMatch = true;

            /// <summary>
            /// Constraint AASd-108: All first level child elements in a SubmodelElementList shall have the
            /// same submodel element Type as specified in SubmodelElementList/typeValueListElement.
            /// </summary>
            public bool AllChildSmeTypeMatch = true;

            /// <summary>
            /// Constraint AASd-109: If SubmodelElementList/typeValueListElement equal to Property or Range,
            /// SubmodelElementList/valueTypeListElement shall be set and all first level child elements in
            /// the SubmodelElementList shall have the the Value Type as specified in
            /// SubmodelElementList/valueTypeListElement
            /// </summary>
            public bool AllChildValueTypeMatch = true;
        }

        public ConstraintStat EvalConstraintStat()
        {
            // access
            var res = new ConstraintStat();
            if (value == null)
                return res;

            // prepare SME Type
            var smeTypeToCheck = SubmodelElementWrapper.GetAdequateEnum2(typeValueListElement);

            // prepare Value Type
            var valueTypeToCheck = valueTypeListElement?.Trim().ToLower();

            // eval
            foreach (var smw in value)
            {
                // access
                var sme = smw?.submodelElement;
                if (sme == null)
                    continue;

                // need self description
                var smesd = sme.GetSelfDescription();
                if (smesd == null)
                    continue;

                // sem id?
                if (res.AllChildSemIdMatch
                    && semanticIdListElement?.IsValid == true
                    && sme.semanticId?.IsValid == true
                    && !semanticIdListElement.Matches(sme.semanticId))
                    res.AllChildSemIdMatch = false;

                // Type of SME?
                if (smeTypeToCheck != SubmodelElementWrapper.AdequateElementEnum.Unknown
                    && res.AllChildSmeTypeMatch
                    && smesd.ElementEnum != smeTypeToCheck)
                    res.AllChildSmeTypeMatch = false;

                // Value Type to check
                // TODO (MIHO, 2022-01-08): GetValueType() worth the effort to implement?
                if (valueTypeToCheck != null && valueTypeToCheck.Length > 0
                    && res.AllChildValueTypeMatch
                    && sme is AdminShell.Property prop
                    && prop.valueType != null && prop.value.Trim().Length > 0
                    && prop.value.Trim().ToLower() != valueTypeToCheck)
                    res.AllChildValueTypeMatch = false;

                if (valueTypeToCheck != null && valueTypeToCheck.Length > 0
                    && res.AllChildValueTypeMatch
                    && sme is AdminShell.Property range
                    && range.valueType != null && range.value.Trim().Length > 0
                    && range.value.Trim().ToLower() != valueTypeToCheck)
                    res.AllChildValueTypeMatch = false;
            }

            // ok
            return res;
        }

        public override AasElementSelfDescription GetSelfDescription()
        {
            return new AasElementSelfDescription("SubmodelElementList", "SML",
                SubmodelElementWrapper.AdequateElementEnum.SubmodelElementList);
        }
    }
}

