/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;
using System.Xml.Serialization;



namespace AdminShell
{
    public partial class AdminShellV30
    {
        public class SubmodelElementList : SubmodelElementCollection
        {
            // for JSON only
            [XmlIgnore]
            [JsonProperty(PropertyName = "modelType")]
            public new JsonModelTypeWrapper JsonModelType
            {
                get { return new JsonModelTypeWrapper(GetElementName()); }
            }

            // members
            // (from "old" SMC, there are a lot members!)

            public bool orderRelevant = false;

            public SemanticId semanticIdListElement = null;

            // Note MIHO: I was attempted to have the Type of SubmodelElementWrapper.AdequateElementEnum,
            // however basic approach of this SDK is to have everything as string to be open
            public string typeValueListElement = null;

            public string valueTypeListElement = null;

            // constructors

            public SubmodelElementList() { }

            public SubmodelElementList(SubmodelElement src, bool shallowCopy = false)
                : base(src, shallowCopy)
            {
                if (!(src is SubmodelElementList sml))
                    return;

                this.orderRelevant = sml.orderRelevant;

                if (sml.semanticIdListElement != null)
                    this.semanticIdListElement = new SemanticId(sml.semanticIdListElement);

                this.typeValueListElement = sml.typeValueListElement;
                this.valueTypeListElement = sml.valueTypeListElement;
            }

#if !DoNotUseAdminShell
            // new in V3.0
#endif

            public new static SubmodelElementList CreateNew(
                string idShort = null, string category = null, Identifier semanticIdKey = null)
            {
                var x = new SubmodelElementList();
                x.CreateNewLogic(idShort, category, semanticIdKey);
                return (x);
            }

            // advanced checks

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

            // self description

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("SubmodelElementList", "SML",
                    SubmodelElementWrapper.AdequateElementEnum.SubmodelElementList);
            }


        }

    }
}
