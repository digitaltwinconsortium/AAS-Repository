/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AdminShell
{
    #region AdminShell_V2_0

    public partial class AdminShellV20
    {
        public class SubmodelElementCollection : SubmodelElement, IManageSubmodelElements, IEnumerateChildren
        {
            // for JSON only
            [XmlIgnore]
            [JsonProperty(PropertyName = "modelType")]
            public new JsonModelTypeWrapper JsonModelType
            {
                get { return new JsonModelTypeWrapper(GetElementName()); }
            }

            // values == SMEs
            
             // do NOT count children!
            public SubmodelElementWrapperCollection value = new SubmodelElementWrapperCollection();

            [XmlIgnore]
            [JsonProperty(PropertyName = "Value")]
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
                        this.value = new SubmodelElementWrapperCollection();
                        foreach (var x in value)
                        {
                            var smew = new SubmodelElementWrapper() { submodelElement = x };
                            this.value.Add(smew);
                        }
                    }
                }
            }

            // constant members
            public bool ordered = false;
            public bool allowDuplicates = false;

            // enumartes its children

            public IEnumerable<SubmodelElementWrapper> EnumerateChildren()
            {
                if (this.value != null)
                    foreach (var smw in this.value)
                        yield return smw;
            }

            public void AddChild(SubmodelElementWrapper smw)
            {
                if (smw == null)
                    return;
                if (this.value == null)
                    this.value = new SubmodelElementWrapperCollection();
                this.value.Add(smw);
            }

            // constructors

            public SubmodelElementCollection() { }

            public SubmodelElementCollection(SubmodelElement src, bool shallowCopy = false)
                : base(src)
            {
                if (!(src is SubmodelElementCollection smc))
                    return;

                this.ordered = smc.ordered;
                this.allowDuplicates = smc.allowDuplicates;
                if (!shallowCopy)
                    foreach (var smw in smc.value)
                        value.Add(new SubmodelElementWrapper(smw.submodelElement));
            }

#if !DoNotUseAdminShell
            public SubmodelElementCollection(
                AdminShell.AdminShellV10.SubmodelElementCollection src, bool shallowCopy = false)
                : base(src)
            {
                if (src == null)
                    return;

                this.ordered = src.ordered;
                this.allowDuplicates = src.allowDuplicates;
                if (!shallowCopy)
                    foreach (var smw in src.value)
                        value.Add(new SubmodelElementWrapper(smw.submodelElement));
            }
#endif

            public static SubmodelElementCollection CreateNew(
                string idShort = null, string category = null, Key semanticIdKey = null)
            {
                var x = new SubmodelElementCollection();
                x.CreateNewLogic(idShort, category, semanticIdKey);
                return (x);
            }

            // from IManageSubmodelElements
            public void Add(SubmodelElement sme)
            {
                if (value == null)
                    value = new SubmodelElementWrapperCollection();
                var sew = new SubmodelElementWrapper();
                sme.parent = this; // track parent here!
                sew.submodelElement = sme;
                value.Add(sew);
            }

            public void Insert(int index, SubmodelElement sme)
            {
                if (value == null)
                    value = new SubmodelElementWrapperCollection();
                var sew = new SubmodelElementWrapper();
                sme.parent = this; // track parent here!
                sew.submodelElement = sme;
                value.Insert(index, sew);
            }

            public void Remove(SubmodelElement sme)
            {
                if (value != null)
                    value.Remove(sme);
            }

            // further

            public void Set(bool allowDuplicates = false, bool ordered = false)
            {
                this.allowDuplicates = allowDuplicates;
                this.ordered = ordered;
            }

            public SubmodelElementWrapper FindFirstIdShort(string idShort)
            {
                return this.value?.FindFirstIdShort(idShort);
            }

            public T CreateSMEForCD<T>(ConceptDescription cd, string category = null, string idShort = null,
                string idxTemplate = null, int maxNum = 999, bool addSme = false) where T : SubmodelElement, new()
            {
                if (this.value == null)
                    this.value = new SubmodelElementWrapperCollection();
                return this.value.CreateSMEForCD<T>(cd, category, idShort, idxTemplate, maxNum, addSme);
            }

            public T CreateSMEForIdShort<T>(string idShort, string category = null,
                string idxTemplate = null, int maxNum = 999, bool addSme = false) where T : SubmodelElement, new()
            {
                if (this.value == null)
                    this.value = new SubmodelElementWrapperCollection();
                return this.value.CreateSMEForIdShort<T>(idShort, category, idxTemplate, maxNum, addSme);
            }


            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("SubmodelElementCollection", "SMC");
            }
        }



    }

    #endregion
}

