#define UseAdminShell

using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

/* Copyright (c) 2018-2019 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>, author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/


#if UseAdminShell

namespace AdminShell
{

    #region Utils

    #endregion


    #region AdminShell_V1_0

    public partial class AdminShellV10
    {
        public class Submodel : Identifiable, System.IDisposable, IGetReference
        {
            // for JSON only
            [XmlIgnore]
            [JsonProperty(PropertyName = "modelType")]
            public JsonModelTypeWrapper JsonModelType { get { return new JsonModelTypeWrapper(GetElementName()); } }

            // members

            // do this in order to be IDisposable, that is: suitable for (using)
            void System.IDisposable.Dispose() { }
            public void GetData() { }
            // from hasDataSpecification:
            [XmlElement(ElementName = "hasDataSpecification")]
            public HasDataSpecification hasDataSpecification = null;
            // from hasSemanticId:
            [XmlElement(ElementName = "semanticId")]
            public SemanticId semanticId = new SemanticId();
            // from Kindable
            [XmlElement(ElementName = "kind")]
            
            public Kind kind = new Kind();
            [XmlIgnore]
            [JsonProperty(PropertyName = "kind")]
            public string JsonKind
            {
                get
                {
                    if (kind == null)
                        return null;
                    return kind.kind;
                }
                set
                {
                    if (kind == null)
                        kind = new Kind();
                    kind.kind = value;
                }
            }
            // from Qualifiable:
            [XmlArray("qualifier")]
            [XmlArrayItem("qualifier")]
            public List<Qualifier> qualifiers = null;

            // from this very class     
            
            public List<SubmodelElementWrapper> submodelElements = null;
            [XmlIgnore]
            [JsonProperty(PropertyName = "submodelElements")]
            /*
            public IEnumerator<SubmodelElement> JsonSubmodelElements
            {
                get
                {
                    if (submodelElements == null)
                        yield return null;
                    foreach (var smew in submodelElements)
                        yield return smew.submodelElement;
                }
            }
            */
            public SubmodelElement[] JsonSubmodelElements
            {
                get
                {
                    var res = new List<SubmodelElement>();
                    if (submodelElements != null)
                        foreach (var smew in submodelElements)
                            res.Add(smew.submodelElement);
                    return res.ToArray();
                }
                set
                {
                    if (value != null)
                    {
                        this.submodelElements = new List<SubmodelElementWrapper>();
                        foreach (var x in value)
                        {
                            var smew = new SubmodelElementWrapper();
                            smew.submodelElement = x;
                            this.submodelElements.Add(smew);
                        }
                    }
                }
            }
            // public List<SubmodelElement> JsonSubmodelElements = new List<SubmodelElement>();

            // getter / setter

            // constructors / creators

            public Submodel() : base() { }

            public Submodel(Submodel src, bool shallowCopy = false)
                : base(src)
            {
                if (src.hasDataSpecification != null)
                    this.hasDataSpecification = new HasDataSpecification(src.hasDataSpecification);
                if (src.semanticId != null)
                    this.semanticId = new SemanticId(src.semanticId);
                if (src.kind != null)
                    this.kind = new Kind(src.kind);
                if (!shallowCopy && src.submodelElements != null)
                {
                    if (this.submodelElements == null)
                        this.submodelElements = new List<SubmodelElementWrapper>();
                    foreach (var smw in src.submodelElements)
                        this.submodelElements.Add(new SubmodelElementWrapper(smw.submodelElement, shallowCopy));
                }
            }

            public static Submodel CreateNew(string idType, string id, string version = null, string revision = null)
            {
                var s = new Submodel();
                s.identification.idType = idType;
                s.identification.id = id;
                if (version != null)
                {
                    if (s.administration == null)
                        s.administration = new Administration();
                    s.administration.Version = version;
                    s.administration.Revision = revision;
                }
                return (s);
            }

            public override string GetElementName()
            {
                return "Submodel";
            }

            public Reference GetReference()
            {
                SubmodelRef l = new SubmodelRef();
                l.Keys.Add(Key.CreateNew(this.GetElementName(), true, this.identification.idType, this.identification.id));
                return l;
            }

            public void Add(SubmodelElement se)
            {
                if (submodelElements == null)
                    submodelElements = new List<SubmodelElementWrapper>();
                var sew = new SubmodelElementWrapper();
                se.parent = this; // track parent here!
                sew.submodelElement = se;
                submodelElements.Add(sew);
            }

            public void AddDataSpecification(Key k)
            {
                if (hasDataSpecification == null)
                    hasDataSpecification = new HasDataSpecification();
                var r = new Reference();
                r.Keys.Add(k);
                hasDataSpecification.reference.Add(r);
            }

            public SubmodelElementWrapper FindSubmodelElementWrapper(string idShort)
            {
                if (this.submodelElements == null)
                    return null;
                foreach (var smw in this.submodelElements)
                    if (smw.submodelElement != null)
                        if (smw.submodelElement.idShort.Trim().ToLower() == idShort.Trim().ToLower())
                            return smw;
                return null;
            }

            public Tuple<string, string> ToCaptionInfo()
            {
                var caption = AdminShellUtil.EvalToNonNullString("\"{0}\" ", idShort, "<no idShort!>");
                if (administration != null)
                    caption += "V" + administration.Version + "." + administration.Revision;
                var info = "";
                if (identification != null)
                    info = $"[{identification.idType}, {identification.id}]";
                return Tuple.Create(caption, info);
            }

            public override string ToString()
            {
                var ci = ToCaptionInfo();
                return string.Format("{0}{1}", ci.Item1, (ci.Item2 != "") ? " / " + ci.Item2 : "");
            }

            // Recursing

            private void RecurseOnSubmodelElementsRecurse(List<SubmodelElementWrapper> wrappers, object state, List<SubmodelElement> parents, Action<object, List<SubmodelElement>, SubmodelElement> lambda)
            {
                // trivial
                if (wrappers == null || parents == null || lambda == null)
                    return;

                // over all elements
                foreach (var smw in wrappers)
                {
                    var current = smw.submodelElement;
                    if (current == null)
                        continue;

                    // call lambda for this element
                    lambda(state, parents, current);

                    // add to parents
                    parents.Add(current);

                    // dive into?
                    if (current is SubmodelElementCollection)
                    {
                        var smc = current as SubmodelElementCollection;
                        RecurseOnSubmodelElementsRecurse(smc.value, state, parents, lambda);
                    }

                    if (current is Operation)
                    {
                        var op = current as Operation;
                        for (int i = 0; i < 2; i++)
                            RecurseOnSubmodelElementsRecurse(Operation.GetWrappers(op[i]), state, parents, lambda);
                    }

                    // remove from parents
                    parents.RemoveAt(parents.Count - 1);
                }
            }

            public void RecurseOnSubmodelElements(object state, Action<object, List<SubmodelElement>, SubmodelElement> lambda)
            {
                RecurseOnSubmodelElementsRecurse(this.submodelElements, state, new List<SubmodelElement>(), lambda);
            }

            // Parents stuff

            private static void SetParentsForSME(Referable parent, SubmodelElement se)
            {
                se.parent = parent;
                var smc = se as SubmodelElementCollection;
                if (smc != null)
                    foreach (var sme in smc.value)
                        SetParentsForSME(se, sme.submodelElement);
            }

            public void SetAllParents()
            {
                if (this.submodelElements != null)
                    foreach (var sme in this.submodelElements)
                        SetParentsForSME(this, sme.submodelElement);
            }

        }

    }

    #endregion
}

#endif