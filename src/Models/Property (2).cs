/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Serialization;


//namespace AdminShell
namespace AdminShell_V30
{
    public partial class AdminShellV30
    {
        public class Property : DataElement
        {
            // for JSON only
            [XmlIgnore]
            [JsonProperty(PropertyName = "modelType")]
            public new JsonModelTypeWrapper JsonModelType
            {
                get { return new JsonModelTypeWrapper(GetElementName()); }
            }

            // members

            [MetaModelName("Property.valueType")]
            
            
            public string valueType = "";
            [XmlIgnore]
            [JsonProperty(PropertyName = "valueType")]
            public JsonValueTypeCast JsonValueType
            {
                get { return new JsonValueTypeCast(this.valueType); }
                set { this.valueType = value?.dataObjectType?.name; }
            }


            [MetaModelName("Property.value")]
            
            public string value = "";
            public GlobalReference valueId = null;

            // constructors

            public Property() { }

            public Property(SubmodelElement src)
                : base(src)
            {
                if (!(src is Property p))
                    return;
                this.valueType = p.valueType;
                this.value = p.value;
                if (p.valueId != null)
                    valueId = new GlobalReference(p.valueId);
            }

#if !DoNotUseAasxCompatibilityModels
            public Property(AasxCompatibilityModels.AdminShellV10.Property src)
                : base(src)
            {
                if (src == null)
                    return;

                this.valueType = src.valueType;
                this.value = src.value;
                if (src.valueId != null)
                    this.valueId = new GlobalReference(src.valueId);
            }

            public Property(AasxCompatibilityModels.AdminShellV20.Property src)
                : base(src)
            {
                if (src == null)
                    return;

                this.valueType = src.valueType;
                this.value = src.value;
                if (src.valueId != null)
                    this.valueId = new GlobalReference(src.valueId);
            }

#endif

            public static Property CreateNew(
                string idShort = null, string category = null, Identifier semanticIdKey = null)
            {
                var x = new Property();
                x.CreateNewLogic(idShort, category, semanticIdKey);
                return (x);
            }

            public Property Set(string valueType = "", string value = "")
            {
                this.valueType = valueType;
                this.value = value;
                return this;
            }

            public Property SetValueId(string valueId)
            {
                this.valueId = GlobalReference.CreateNew(value);
                return this;
            }

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("Property", "Prop",
                    SubmodelElementWrapper.AdequateElementEnum.Property);
            }

            public override string ValueAsText(string defaultLang = null)
            {
                return "" + value;
            }

            public override void ValueFromText(string text, string defaultLang = null)
            {
                value = "" + text;
            }

            public bool IsTrue()
            {
                if (this.valueType?.Trim().ToLower() == "boolean")
                {
                    var v = "" + this.value?.Trim().ToLower();
                    if (v == "true" || v == "1")
                        return true;
                }
                return false;
            }

            public override double? ValueAsDouble()
            {
                // pointless
                if (this.value == null || this.value.Trim() == "" || this.valueType == null)
                    return null;

                // type?
                var vt = this.valueType.Trim().ToLower();
                if (!DataElement.ValueTypes_Number.Contains(vt))
                    return null;

                // try convert
                if (double.TryParse(this.value, NumberStyles.Any, CultureInfo.InvariantCulture, out double dbl))
                    return dbl;

                // no
                return null;
            }

            public override object ToValueOnlySerialization()
            {
                var valueObject = new Dictionary<string, string>
                {
                    { idShort, value }
                };
                return valueObject;
            }

        }

    }
}
