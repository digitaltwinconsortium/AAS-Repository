/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;
using System.Xml;
using System.Xml.Serialization;

namespace AasxCompatibilityModels
{
    #region AdminShell_V2_0

    public partial class AdminShellV20
    {
        public class Qualifier : IAasElement
        {
            // for JSON only
            [XmlIgnore]
            [JsonProperty(PropertyName = "modelType")]
            public JsonModelTypeWrapper JsonModelType { get { return new JsonModelTypeWrapper(GetElementName()); } }

            // member
            // from hasSemantics:
            [XmlElement(ElementName = "semanticId")]
            public SemanticId semanticId = null;

            // this class
            // TODO (Michael Hoffmeister, 2020-08-01): check, if Json has Qualifiers or not

            [MetaModelName("Qualifier.type")]
            
            
            public string type = "";

            [MetaModelName("Qualifier.valueType")]
            
            
            public string valueType = "";

            
            public Reference valueId = null;

            [MetaModelName("Qualifier.value")]
            
            
            public string value = null;

            // dead-csharp off
            // Remark: due to publication of v2.0.1, the order of elements has changed!!!
            // from hasSemantics:
            // [XmlElement(ElementName = "semanticId")]
            // 
            // public SemanticId semanticId = null;
            // dead-csharp on

            // constructors

            public Qualifier() { }

            public Qualifier(Qualifier src)
            {
                if (src.semanticId != null)
                    this.semanticId = new SemanticId(src.semanticId);
                this.type = src.type;
                this.value = src.value;
                if (src.valueId != null)
                    this.valueId = new Reference(src.valueId);
            }

#if !DoNotUseAasxCompatibilityModels
            public Qualifier(AasxCompatibilityModels.AdminShellV10.Qualifier src)
            {
                if (src.semanticId != null)
                    this.semanticId = new SemanticId(src.semanticId);
                this.type = src.qualifierType;
                this.value = src.qualifierValue;
                if (src.qualifierValueId != null)
                    this.valueId = new Reference(src.qualifierValueId);
            }
#endif

            public AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("Qualifier", "Qfr");
            }

            public string GetElementName()
            {
                return this.GetSelfDescription()?.ElementName;
            }

            // ReSharper disable MethodOverloadWithOptionalParameter .. this seems to work, anyhow
            // ReSharper disable RedundantArgumentDefaultValue
            public string ToString(int format = 0, string delimiter = ",")
            {
                var res = "" + type;
                if (res == "")
                    res += "" + semanticId?.ToString(format, delimiter);

                if (value != null)
                    res += " = " + value;
                else if (valueId != null)
                    res += " = " + valueId?.ToString(format, delimiter);

                return res;
            }

            public override string ToString()
            {
                return this.ToString(0);
            }
            // ReSharper enable MethodOverloadWithOptionalParameter
            // ReSharper enable RedundantArgumentDefaultValue
        }



    }

    #endregion
}

