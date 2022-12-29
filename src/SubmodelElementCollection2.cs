#define UseAasxCompatibilityModels

using System.Collections.Generic;
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
        public class SubmodelElementCollection : SubmodelElement
        {
            // for JSON only
            [XmlIgnore]
            [JsonProperty(PropertyName = "modelType")]
            public new JsonModelTypeWrapper JsonModelType { get { return new JsonModelTypeWrapper(GetElementName()); } }

            // members
            [JsonIgnore]
            public List<SubmodelElementWrapper> value = new List<SubmodelElementWrapper>();

            [XmlIgnore]
            [JsonProperty(PropertyName = "value")]
            /*
            public IEnumerator<SubmodelElement> JsonValue
            {
                get
                {
                    /*
                    if (value == null)
                        yield return null;
                    var sme = new Property();
                    sme.idShort = "test";
                    yield return sme;
                    yield break;
                }
            }
            */
            public SubmodelElement[] JsonValue
            {
                get
                {
                    var res = new List<SubmodelElement>();
                    if (value != null)
                        foreach (var smew in value)
                            res.Add(smew.submodelElement);
                    return res.ToArray();
                }
                set
                {
                    if (value != null)
                    {
                        this.value = new List<SubmodelElementWrapper>();
                        foreach (var x in value)
                        {
                            var smew = new SubmodelElementWrapper();
                            smew.submodelElement = x;
                            this.value.Add(smew);
                        }
                    }
                }
            }
            // public List<SubmodelElement> JsonSubmodelElements = new List<SubmodelElement>();

            // further members
            public bool ordered = false;
            public bool allowDuplicates = false;

            // constructors

            public SubmodelElementCollection() { }

            public SubmodelElementCollection(SubmodelElementCollection src, bool shallowCopy = false)
                : base(src)
            {
                this.ordered = src.ordered;
                this.allowDuplicates = src.allowDuplicates;
                if (!shallowCopy)
                    foreach (var smw in src.value)
                        value.Add(new SubmodelElementWrapper(smw.submodelElement));
            }

            public static SubmodelElementCollection CreateNew(string idShort = null, string category = null, Key semanticIdKey = null)
            {
                var x = new SubmodelElementCollection();
                x.CreateNewLogic(idShort, category, semanticIdKey);
                return (x);
            }

            public void Set(bool allowDuplicates = false, bool ordered = false)
            {
                this.allowDuplicates = allowDuplicates;
                this.ordered = ordered;
            }

            public void Add(SubmodelElement se)
            {
                if (value == null)
                    value = new List<SubmodelElementWrapper>();
                var sew = new SubmodelElementWrapper();
                se.parent = this; // track parent here!
                sew.submodelElement = se;
                value.Add(sew);
            }

            public SubmodelElementWrapper FindSubmodelElementWrapper(string idShort)
            {
                if (this.value == null)
                    return null;
                foreach (var smw in this.value)
                    if (smw.submodelElement != null)
                        if (smw.submodelElement.idShort.Trim().ToLower() == idShort.Trim().ToLower())
                            return smw;
                return null;
            }

            public override string GetElementName()
            {
                return "SubmodelElementCollection";
            }
        }

    }

    #endregion
}

#endif