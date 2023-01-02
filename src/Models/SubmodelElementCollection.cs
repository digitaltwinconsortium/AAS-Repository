/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;



namespace AdminShell
{
    public partial class AdminShellV30
    {
        /// <summary>
        /// In V2.0, this was the most important SME to hold multiple child SME.
        /// Ib V3.0, this is deprecated and will made abstract.
        /// Use SubmodelElementList, SubmodelElementStruct instead.
        /// </summary>
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
            [XmlIgnore]
            
            // do NOT count children!
            //private SubmodelElementWrapperCollection _value = null;
            private SubmodelElementWrapperCollection _value = new SubmodelElementWrapperCollection();

            
            public SubmodelElementWrapperCollection value
            {
                get { return _value; }
                set { _value = value; _value.Parent = this; }
            }

            [XmlIgnore]
            [JsonProperty(PropertyName = "Value")]
            public SubmodelElement[] JsonValue
            {
                get
                {
                    var res = new ListOfSubmodelElement();
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

            public EnumerationPlacmentBase GetChildrenPlacement(SubmodelElement child)
            {
                return null;
            }

            public object AddChild(SubmodelElementWrapper smw, EnumerationPlacmentBase placement = null)
            {
                if (smw == null)
                    return null;
                if (this.value == null)
                    this.value = new SubmodelElementWrapperCollection();
                if (smw.submodelElement != null)
                    smw.submodelElement.parent = this;
                this.value.Add(smw);
                return smw;
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
                this.value = new SubmodelElementWrapperCollection();
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
                this.value = new SubmodelElementWrapperCollection();
                if (!shallowCopy)
                    foreach (var smw in src.value)
                        value.Add(new SubmodelElementWrapper(smw.submodelElement));
            }

            public SubmodelElementCollection(
                AdminShell.AdminShellV20.SubmodelElement src, bool shallowCopy = false)
                : base(src)
            {
                if (!(src is AdminShell.AdminShellV20.SubmodelElementCollection smc))
                    return;

                this.ordered = smc.ordered;
                this.allowDuplicates = smc.allowDuplicates;
                this.value = new SubmodelElementWrapperCollection();
                if (!shallowCopy)
                    foreach (var smw in smc.value)
                        value.Add(new SubmodelElementWrapper(smw.submodelElement));
            }
#endif

            public static SubmodelElementCollection CreateNew(
                string idShort = null, string category = null, Identifier semanticIdKey = null)
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
                if (index < 0 || index >= value.Count)
                    return;
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
                return new AasElementSelfDescription("SubmodelElementCollection", "SMC",
                    SubmodelElementWrapper.AdequateElementEnum.SubmodelElementCollection);
            }

            // Recursing

            /// <summary>
            /// Recurses on all Submodel elements of a Submodel or SME, which allows children.
            /// The <c>state</c> object will be passed to the lambda function in order to provide
            /// stateful approaches. 
            /// </summary>
            /// <param name="state">State object to be provided to lambda. Could be <c>null.</c></param>
            /// <param name="lambda">The lambda function as <c>(state, parents, SME)</c>
            /// The lambda shall return <c>TRUE</c> in order to deep into recursion.
            /// </param>
            public void RecurseOnSubmodelElements(
                object state, Func<object, ListOfReferable, SubmodelElement, bool> lambda)
            {
                this.value?.RecurseOnReferables(state, null, (o, par, rf) =>
                {
                    if (rf is SubmodelElement sme)
                        return lambda(o, par, sme);
                    else
                        return true;
                });
            }

            /// <summary>
            /// Recurses on all Submodel elements of a Submodel or SME, which allows children.
            /// The <c>state</c> object will be passed to the lambda function in order to provide
            /// stateful approaches. Include this element, as well. 
            /// </summary>
            /// <param name="state">State object to be provided to lambda. Could be <c>null.</c></param>
            /// <param name="lambda">The lambda function as <c>(state, parents, SME)</c>
            /// The lambda shall return <c>TRUE</c> in order to deep into recursion.</param>
            /// <param name="includeThis">Include this element as well. <c>parents</c> will then 
            /// include this element as well!</param>
            public override void RecurseOnReferables(
                object state, Func<object, ListOfReferable, Referable, bool> lambda,
                bool includeThis = false)
            {
                var parents = new ListOfReferable();
                if (includeThis)
                {
                    lambda(state, null, this);
                    parents.Add(this);
                }
                this.value?.RecurseOnReferables(state, parents, lambda);
            }
        }

    }
}
