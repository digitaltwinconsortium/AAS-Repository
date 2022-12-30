#define UseAasxCompatibilityModels

using System.Xml.Serialization;
using Newtonsoft.Json;

/* Copyright (c) 2018-2019 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>, author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/


#if UseAasxCompatibilityModels

namespace AasxCompatibilityModels
{

    #region Utils

    #endregion


    #region AdminShell_V1_0

    public partial class AdminShellV10
    {
        public class Property : DataElement
        {
            // for JSON only
            [XmlIgnore]
            [JsonProperty(PropertyName = "modelType")]
            public new JsonModelTypeWrapper JsonModelType { get { return new JsonModelTypeWrapper(GetElementName()); } }

            // members

            
            public string valueType = "";
            [XmlIgnore]
            [JsonProperty(PropertyName = "valueType")]
            public JsonValueTypeCast JsonValueType
            {
                get { return new JsonValueTypeCast(this.valueType); }
                set { this.valueType = value?.dataObjectType?.name; }
            }


            public string value = "";
            public Reference valueId = null;

            // constructors

            public Property() { }

            public Property(Property src)
                : base(src)
            {
                this.valueType = src.valueType;
                this.value = src.value;
                if (src.valueId != null)
                    src.valueId = new Reference(src.valueId);
            }

            public static Property CreateNew(string idShort = null, string category = null, Key semanticIdKey = null)
            {
                var x = new Property();
                x.CreateNewLogic(idShort, category, semanticIdKey);
                return (x);
            }

            public void Set(string valueType = "", string value = "")
            {
                this.valueType = valueType;
                this.value = value;
            }

            public void Set(string type, bool local, string idType, string value)
            {
                this.valueId = Reference.CreateNew(Key.CreateNew(type, local, idType, value));
            }

            public override string GetElementName()
            {
                return "Property";
            }
        }

    }

    #endregion
}

#endif