/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;


//namespace AdminShell
namespace AdminShell_V30
{
    public partial class AdminShellV30
    {
        public class Range : DataElement
        {
            // for JSON only
            [XmlIgnore]
            [JsonProperty(PropertyName = "modelType")]
            public new JsonModelTypeWrapper JsonModelType
            {
                get { return new JsonModelTypeWrapper(GetElementName()); }
            }

            // members

            [MetaModelName("Range.valueType")]
            
            
            
            public string valueType = "";

            [XmlIgnore]
            [JsonProperty(PropertyName = "valueType")]
            public JsonValueTypeCast JsonValueType
            {
                get { return new JsonValueTypeCast(this.valueType); }
                set { this.valueType = value?.dataObjectType?.name; }
            }

            [MetaModelName("Range.min")]
            
            
            public string min = "";

            [MetaModelName("Range.max")]
            
            
            public string max = "";

            // constructors

            public Range() { }

            public Range(SubmodelElement src)
                : base(src)
            {
                if (!(src is Range rng))
                    return;

                this.valueType = rng.valueType;
                this.min = rng.min;
                this.max = rng.max;
            }

#if !DoNotUseAasxCompatibilityModels
            // not available in V1.0

            public Range(AasxCompatibilityModels.AdminShellV20.Range src)
                : base(src)
            {
                this.valueType = src.valueType;
                this.min = src.min;
                this.max = src.max;
            }
#endif

            public static Range CreateNew(
                string idShort = null, string category = null, Identifier semanticIdKey = null)
            {
                var x = new Range();
                x.CreateNewLogic(idShort, category, semanticIdKey);
                return (x);
            }

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("Range", "Range",
                    SubmodelElementWrapper.AdequateElementEnum.Range);
            }

            public override string ValueAsText(string defaultLang = null)
            {
                return "" + min + " .. " + max;
            }

            public override object ToValueOnlySerialization()
            {
                var output = new Dictionary<string, Dictionary<string, string>>();
                var valueDict = new Dictionary<string, string>
                {
                    { "min", min },
                    { "max", max }
                };

                output.Add(idShort, valueDict);
                return output;
            }
        }

    }
}
