/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;


//namespace AdminShellNS
namespace AdminShell_V30
{
    public partial class AdminShellV30
    {
        /// <summary>
        /// New V3.0 reference element, specifically targeted at global references
        /// </summary>
        public class GlobalReferenceElement : ReferenceElement
        {
            // for JSON only
            [XmlIgnore]
            [JsonProperty(PropertyName = "modelType")]
            public new JsonModelTypeWrapper JsonModelType
            {
                get { return new JsonModelTypeWrapper(GetElementName()); }
            }

            // members

            public GlobalReference value = new GlobalReference();

            // constructors

            public GlobalReferenceElement() { }

            public GlobalReferenceElement(SubmodelElement src)
                : base(src)
            {
                if (!(src is GlobalReferenceElement gre))
                    return;

                if (gre.value != null)
                    this.value = new GlobalReference(gre.value);
            }

#if !DoNotUseAasxCompatibilityModels
            public GlobalReferenceElement(AasxCompatibilityModels.AdminShellV10.ReferenceElement src)
                : base(src)
            {
                if (src == null)
                    return;

                if (src.value != null)
                    this.value = new GlobalReference(src.value);
            }

            public GlobalReferenceElement(AasxCompatibilityModels.AdminShellV20.ReferenceElement src)
                : base(src)
            {
                if (src == null)
                    return;

                if (src.value != null)
                    this.value = new GlobalReference(src.value);
            }
#endif

            public static GlobalReferenceElement CreateNew(
                string idShort = null, string category = null, Identifier semanticIdKey = null)
            {
                var x = new GlobalReferenceElement();
                x.CreateNewLogic(idShort, category, semanticIdKey);
                return (x);
            }

            public void Set(GlobalReference value = null)
            {
                this.value = value;
            }

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("GlobalReferenceElement", "RefG",
                    SubmodelElementWrapper.AdequateElementEnum.GlobalReferenceElement);
            }

            public override object ToValueOnlySerialization()
            {
                var output = new Dictionary<string, List<string>>();

                var list = new List<string>();
                foreach (var refVal in this.value.Value)
                {
                    list.Add(refVal.value);
                }

                output.Add(idShort, list);
                return output;
            }

        }

    }
}
