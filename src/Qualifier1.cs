#define UseAasxCompatibilityModels

using System.Xml;
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
        public class Qualifier
        {
            // for JSON only
            [XmlIgnore]
            [JsonProperty(PropertyName = "modelType")]
            public JsonModelTypeWrapper JsonModelType { get { return new JsonModelTypeWrapper(GetElementName()); } }

            // member

            // from hasSemantics:
            [XmlElement(ElementName = "semanticId")]
            // TODO: Qualifiers not working!
            // 190410: test-wise enable them again, everyhing works fine ..
            // [JsonIgnore]
            public SemanticId semanticId = null;
            // this class
            // [JsonIgnore]
            public string qualifierType = null;
            // [JsonIgnore]
            public string qualifierValue = null;
            // [JsonIgnore]
            public Reference qualifierValueId = null;

            // constructors

            public Qualifier() { }

            public Qualifier(Qualifier src)
            {
                if (src.semanticId != null)
                    this.semanticId = new SemanticId(src.semanticId);
                this.qualifierType = src.qualifierType;
                this.qualifierValue = src.qualifierValue;
                if (src.qualifierValueId != null)
                    this.qualifierValueId = new Reference(src.qualifierValueId);
            }

            public string GetElementName()
            {
                return "Qualifier";
            }
        }

    }

    #endregion
}

#endif