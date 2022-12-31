/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Dynamic;
using System.Xml.Serialization;


//namespace AdminShell
namespace AdminShell_V30
{
    public partial class AdminShellV30
    {
        //
        // Relationships
        //

        public class RelationshipElement : DataElement
        {
            // for JSON only
            [XmlIgnore]
            [JsonProperty(PropertyName = "modelType")]
            public new JsonModelTypeWrapper JsonModelType
            {
                get { return new JsonModelTypeWrapper(GetElementName()); }
            }

            // members

            public ModelReference first = new ModelReference();
            public ModelReference second = new ModelReference();

            // constructors

            public RelationshipElement() { }

            public RelationshipElement(SubmodelElement src)
                : base(src)
            {
                if (!(src is RelationshipElement rel))
                    return;

                if (rel.first != null)
                    this.first = new ModelReference(rel.first);
                if (rel.second != null)
                    this.second = new ModelReference(rel.second);
            }

#if !DoNotUseAasxCompatibilityModels
            public RelationshipElement(AasxCompatibilityModels.AdminShellV10.RelationshipElement src)
                : base(src)
            {
                if (src == null)
                    return;

                if (src.first != null)
                    this.first = new ModelReference(src.first);
                if (src.second != null)
                    this.second = new ModelReference(src.second);
            }

            public RelationshipElement(AasxCompatibilityModels.AdminShellV20.RelationshipElement src)
                : base(src)
            {
                if (src == null)
                    return;

                if (src.first != null)
                    this.first = new ModelReference(src.first);
                if (src.second != null)
                    this.second = new ModelReference(src.second);
            }
#endif

            public static RelationshipElement CreateNew(
                string idShort = null, string category = null,
                Identifier semanticIdKey = null, ModelReference first = null,
                ModelReference second = null)
            {
                var x = new RelationshipElement();
                x.CreateNewLogic(idShort, category, semanticIdKey);
                x.first = first;
                x.second = second;
                return (x);
            }

            public void Set(ModelReference first = null, ModelReference second = null)
            {
                this.first = first;
                this.second = second;
            }

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("RelationshipElement", "Rel",
                    SubmodelElementWrapper.AdequateElementEnum.RelationshipElement);
            }

            public override object ToValueOnlySerialization()
            {
                var output = new Dictionary<string, object>();

                var listFirst = new List<Dictionary<string, string>>();
                foreach (var key in first.Keys)
                {
                    var valueDict = new Dictionary<string, string>
                    {
                        { "type", key.type },
                        { "value", key.value }
                    };
                    listFirst.Add(valueDict);
                }

                var listSecond = new List<Dictionary<string, string>>();
                foreach (var key in second.Keys)
                {
                    var valueDict = new Dictionary<string, string>
                    {
                        { "type", key.type },
                        { "value", key.value }
                    };
                    listSecond.Add(valueDict);
                }

                dynamic relObj = new ExpandoObject();
                relObj.first = listFirst;
                relObj.second = listSecond;
                output.Add(idShort, relObj);
                return output;
            }
        }

    }
}
