/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace AasxCompatibilityModels
{
    #region AdminShell_V2_0

    public partial class AdminShellV20
    {
        public class Submodel : Identifiable, IManageSubmodelElements,
                                    System.IDisposable, IGetReference, IEnumerateChildren, IFindAllReferences,
                                    IGetSemanticId
        {
            // for JSON only
            [XmlIgnore]
            [JsonProperty(PropertyName = "modelType")]
            public JsonModelTypeWrapper JsonModelType { get { return new JsonModelTypeWrapper(GetElementName()); } }

            // members

            // do this in order to be IDisposable, that is: suitable for (using)
            void System.IDisposable.Dispose() { }
            public void GetData() { }

            // from HasKind
            [XmlElement(ElementName = "kind")]
            
            public ModelingKind kind = new ModelingKind();
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
                        kind = new ModelingKind();
                    kind.kind = value;
                }
            }

            // from hasSemanticId:
            [XmlElement(ElementName = "semanticId")]
            public SemanticId semanticId = new SemanticId();
            public SemanticId GetSemanticId() { return semanticId; }

            // from Qualifiable:
            [XmlArray("qualifier")]
            [XmlArrayItem("qualifier")]
            public QualifierCollection qualifiers = null;

            // from hasDataSpecification:
            [XmlElement(ElementName = "embeddedDataSpecification")]
            public HasDataSpecification hasDataSpecification = null;

            // from this very class
            
            public SubmodelElementWrapperCollection submodelElements = null;

            [XmlIgnore]
            [JsonProperty(PropertyName = "submodelElements")]

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
                        this.submodelElements = new SubmodelElementWrapperCollection();
                        foreach (var x in value)
                        {
                            var smew = new SubmodelElementWrapper() { submodelElement = x };
                            this.submodelElements.Add(smew);
                        }
                    }
                }
            }

            // getter / setter

            // constructors / creators

            public Submodel() : base() { }

            public Submodel(Submodel src, bool shallowCopy = false)
                : base(src)
            {
                if (src == null)
                    return;
                if (src.hasDataSpecification != null)
                    this.hasDataSpecification = new HasDataSpecification(src.hasDataSpecification);
                if (src.semanticId != null)
                    this.semanticId = new SemanticId(src.semanticId);
                if (src.kind != null)
                    this.kind = new ModelingKind(src.kind);
                if (!shallowCopy && src.submodelElements != null)
                {
                    if (this.submodelElements == null)
                        this.submodelElements = new SubmodelElementWrapperCollection();
                    foreach (var smw in src.submodelElements)
                        this.submodelElements.Add(new SubmodelElementWrapper(smw.submodelElement, shallowCopy: false));
                }
            }

#if !DoNotUseAasxCompatibilityModels
            public Submodel(AasxCompatibilityModels.AdminShellV10.Submodel src, bool shallowCopy = false)
                : base(src)
            {
                if (src.hasDataSpecification != null)
                    this.hasDataSpecification = new HasDataSpecification(src.hasDataSpecification);
                if (src.semanticId != null)
                    this.semanticId = new SemanticId(src.semanticId);
                if (src.kind != null)
                    this.kind = new ModelingKind(src.kind);
                if (src.qualifiers != null)
                    this.qualifiers = new QualifierCollection(src.qualifiers);
                if (!shallowCopy && src.submodelElements != null)
                {
                    if (this.submodelElements == null)
                        this.submodelElements = new SubmodelElementWrapperCollection();
                    foreach (var smw in src.submodelElements)
                        this.submodelElements.Add(new SubmodelElementWrapper(smw.submodelElement, shallowCopy: false));
                }
            }
#endif

            public static Submodel CreateNew(string idType, string id, string version = null, string revision = null)
            {
                var s = new Submodel() { identification = new Identification(idType, id) };
                if (version != null)
                {
                    if (s.administration == null)
                        s.administration = new Administration();
                    s.administration.version = version;
                    s.administration.revision = revision;
                }
                return (s);
            }

            
            [XmlIgnore]
            public SubmodelElementWrapperCollection SmeForWrite
            {
                get
                {
                    if (this.submodelElements == null)
                        this.submodelElements = new SubmodelElementWrapperCollection();
                    return this.submodelElements;
                }
            }

            // from IEnumarateChildren
            public IEnumerable<SubmodelElementWrapper> EnumerateChildren()
            {
                if (this.submodelElements != null)
                    foreach (var smw in this.submodelElements)
                        yield return smw;
            }

            public void AddChild(SubmodelElementWrapper smw)
            {
                if (smw == null)
                    return;
                if (this.submodelElements == null)
                    this.submodelElements = new SubmodelElementWrapperCollection();
                this.submodelElements.Add(smw);
            }

            // from IManageSubmodelElements
            public void Add(SubmodelElement sme)
            {
                if (submodelElements == null)
                    submodelElements = new SubmodelElementWrapperCollection();
                var sew = new SubmodelElementWrapper();
                sme.parent = this; // track parent here!
                sew.submodelElement = sme;
                submodelElements.Add(sew);
            }

            public void Insert(int index, SubmodelElement sme)
            {
                if (submodelElements == null)
                    submodelElements = new SubmodelElementWrapperCollection();
                var sew = new SubmodelElementWrapper();
                sme.parent = this; // track parent here!
                sew.submodelElement = sme;
                submodelElements.Insert(index, sew);
            }

            public void Remove(SubmodelElement sme)
            {
                if (submodelElements != null)
                    submodelElements.Remove(sme);
            }

            // further

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("Submodel", "SM");
            }

            public Reference GetReference()
            {
                SubmodelRef l = new SubmodelRef();
                l.Keys.Add(
                    Key.CreateNew(
                        this.GetElementName(), true, this.identification.idType, this.identification.id));
                return l;
            }

            /// <summary>
            ///  If instance, return semanticId as on key.
            ///  If template, return identification as key.
            /// </summary>
            /// <returns></returns>
            public Key GetSemanticKey()
            {
                if (true == this.kind?.IsTemplate)
                    return new Key(this.GetElementName(), true, this.identification?.idType, this.identification?.id);
                else
                    return this.semanticId?.GetAsExactlyOneKey();
            }

            public void AddDataSpecification(Key k)
            {
                if (hasDataSpecification == null)
                    hasDataSpecification = new HasDataSpecification();
                var r = new Reference();
                r.Keys.Add(k);
                hasDataSpecification.Add(new EmbeddedDataSpecification(r));
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

            public IEnumerable<T> FindDeep<T>(Predicate<T> match = null) where T : SubmodelElement
            {
                if (this.submodelElements == null)
                    yield break;
                foreach (var x in this.submodelElements.FindDeep<T>(match))
                    yield return x;
            }

            public Tuple<string, string> ToCaptionInfo()
            {
                var caption = AdminShell.AdminShellUtil.EvalToNonNullString("\"{0}\" ", idShort, "<no idShort!>");
                if (administration != null)
                    caption += "V" + administration.version + "." + administration.revision;
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

            public void RecurseOnSubmodelElements(
                object state, Action<object, List<SubmodelElement>, SubmodelElement> lambda)
            {
                this.submodelElements?.RecurseOnSubmodelElements(state, null, lambda);
            }

            // Parents stuff

            private static void SetParentsForSME(Referable parent, SubmodelElement se)
            {
                if (se == null)
                    return;

                se.parent = parent;

                // via interface enumaration
                if (se is IEnumerateChildren)
                {
                    var childs = (se as IEnumerateChildren).EnumerateChildren();
                    if (childs != null)
                        foreach (var c in childs)
                            SetParentsForSME(se, c.submodelElement);
                }
            }

            public void SetAllParents()
            {
                if (this.submodelElements != null)
                    foreach (var sme in this.submodelElements)
                        SetParentsForSME(this, sme.submodelElement);
            }

            private static void SetParentsForSME(Referable parent, SubmodelElement se, DateTime timeStamp)
            {
                if (se == null)
                    return;

                se.parent = parent;
                se.TimeStamp = timeStamp;
                se.TimeStampCreate = timeStamp;

                // via interface enumaration
                if (se is IEnumerateChildren)
                {
                    var childs = (se as IEnumerateChildren).EnumerateChildren();
                    if (childs != null)
                        foreach (var c in childs)
                            SetParentsForSME(se, c.submodelElement, timeStamp);
                }
            }

            public void SetAllParents(DateTime timeStamp)
            {
                if (this.submodelElements != null)
                    foreach (var sme in this.submodelElements)
                        SetParentsForSME(this, sme.submodelElement, timeStamp);
            }

            public T CreateSMEForCD<T>(ConceptDescription cd, string category = null, string idShort = null,
                string idxTemplate = null, int maxNum = 999, bool addSme = false) where T : SubmodelElement, new()
            {
                if (this.submodelElements == null)
                    this.submodelElements = new SubmodelElementWrapperCollection();
                return this.submodelElements.CreateSMEForCD<T>(cd, category, idShort, idxTemplate, maxNum, addSme);
            }

            public T CreateSMEForIdShort<T>(string idShort, string category = null,
                string idxTemplate = null, int maxNum = 999, bool addSme = false) where T : SubmodelElement, new()
            {
                if (this.submodelElements == null)
                    this.submodelElements = new SubmodelElementWrapperCollection();
                return this.submodelElements.CreateSMEForIdShort<T>(idShort, category, idxTemplate, maxNum, addSme);
            }

            // find

            public IEnumerable<Reference> FindAllReferences()
            {
                // not nice: use temp list
                var temp = new List<Reference>();

                // recurse
                this.RecurseOnSubmodelElements(null, (state, parents, sme) =>
                {
                    if (sme is ReferenceElement re)
                        if (re.value != null)
                            temp.Add(re.value);
                    if (sme is RelationshipElement rl)
                    {
                        if (rl.first != null)
                            temp.Add(rl.first);
                        if (rl.second != null)
                            temp.Add(rl.second);
                    }
                });

                // now, give back
                foreach (var r in temp)
                    yield return r;
            }
        }



    }

    #endregion
}

