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

namespace AasxCompatibilityModels
{
    #region AdminShell_V2_0

    public partial class AdminShellV20
    {
        public class AnnotatedRelationshipElement : RelationshipElement, IManageSubmodelElements, IEnumerateChildren
        {
            // for JSON only
            [XmlIgnore]
            [JsonProperty(PropertyName = "modelType")]
            public new JsonModelTypeWrapper JsonModelType
            {
                get { return new JsonModelTypeWrapper(GetElementName()); }
            }

            // members

            // from this very class

            
             // do NOT count children!
            [XmlArray("annotations")]
            [XmlArrayItem("dataElement")]
            public DataElementWrapperCollection annotations = null;

            [XmlIgnore]
            [JsonProperty(PropertyName = "annotations")]
            public DataElement[] JsonAnotations
            {
                get
                {
                    var res = new List<DataElement>();
                    if (annotations != null)
                        foreach (var smew in annotations)
                            if (smew.submodelElement is DataElement de)
                                res.Add(de);
                    return res.ToArray();
                }
                set
                {
                    if (value != null)
                    {
                        this.annotations = new DataElementWrapperCollection();
                        foreach (var x in value)
                        {
                            var smew = new SubmodelElementWrapper() { submodelElement = x };
                            this.annotations.Add(smew);
                        }
                    }
                }
            }

            // constructors

            public AnnotatedRelationshipElement() { }

            public AnnotatedRelationshipElement(SubmodelElement src)
                : base(src)
            {
                if (!(src is AnnotatedRelationshipElement arel))
                    return;
                if (arel.first != null)
                    this.first = new Reference(arel.first);
                if (arel.second != null)
                    this.second = new Reference(arel.second);
                if (arel.annotations != null)
                    this.annotations = new DataElementWrapperCollection(arel.annotations);
            }

            public new static AnnotatedRelationshipElement CreateNew(
                string idShort = null, string category = null, Key semanticIdKey = null,
                Reference first = null, Reference second = null)
            {
                var x = new AnnotatedRelationshipElement();
                x.CreateNewLogic(idShort, category, semanticIdKey);
                x.first = first;
                x.second = second;
                return (x);
            }

            // enumerates its children

            public IEnumerable<SubmodelElementWrapper> EnumerateChildren()
            {
                if (this.annotations != null)
                    foreach (var smw in this.annotations)
                        yield return smw;
            }

            public void AddChild(SubmodelElementWrapper smw)
            {
                if (smw == null || !(smw.submodelElement is DataElement))
                    return;
                if (this.annotations == null)
                    this.annotations = new DataElementWrapperCollection();
                this.annotations.Add(smw);
            }

            // from IManageSubmodelElements
            public void Add(SubmodelElement sme)
            {
                if (annotations == null)
                    annotations = new DataElementWrapperCollection();
                var sew = new SubmodelElementWrapper();
                sme.parent = this; // track parent here!
                sew.submodelElement = sme;
                annotations.Add(sew);
            }

            public void Remove(SubmodelElement sme)
            {
                if (annotations != null)
                    annotations.Remove(sme);
            }

            // further 

            public new void Set(Reference first = null, Reference second = null)
            {
                this.first = first;
                this.second = second;
            }

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("AnnotatedRelationshipElement", "RelA");
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

                List<object> valueOnlyAnnotations = new List<object>();

                foreach (var sme in annotations)
                {
                    valueOnlyAnnotations.Add(sme.submodelElement.ToValueOnlySerialization());
                }

                dynamic relObj = new ExpandoObject();
                relObj.first = listFirst;
                relObj.second = listSecond;
                relObj.annotation = valueOnlyAnnotations;
                output.Add(idShort, relObj);
                return output;
            }
        }



    }

    #endregion
}

