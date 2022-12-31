/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;
using System.Xml.Serialization;


//namespace AdminShell
namespace AdminShell_V30
{
    public partial class AdminShellV30
    {
        public class SubmodelElementStruct : SubmodelElementCollection
        {
            // for JSON only
            [XmlIgnore]
            [JsonProperty(PropertyName = "modelType")]
            public new JsonModelTypeWrapper JsonModelType
            {
                get { return new JsonModelTypeWrapper(GetElementName()); }
            }

            // members
            // (no new members compared to SMC of V2.0)

            // constructors

            public SubmodelElementStruct() { }

            public SubmodelElementStruct(SubmodelElement src, bool shallowCopy = false)
                : base(src, shallowCopy)
            {
            }

#if !DoNotUseAasxCompatibilityModels
            // new in V3.0
#endif

            public new static SubmodelElementStruct CreateNew(
                string idShort = null, string category = null, Identifier semanticIdKey = null)
            {
                var x = new SubmodelElementStruct();
                x.CreateNewLogic(idShort, category, semanticIdKey);
                return (x);
            }

            // self description

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("SubmodelElementStruct", "SMS",
                    SubmodelElementWrapper.AdequateElementEnum.SubmodelElementStruct);
            }


        }

    }
}
