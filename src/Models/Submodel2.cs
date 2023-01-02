/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using AdminShell;
using Newtonsoft.Json;


namespace AdminShell
{
    public partial class AdminShellV30
    {
        //
        // Submodel
        //

        public class Submodel : Identifiable, IManageSubmodelElements,
                            System.IDisposable, IEnumerateChildren, IFindAllReferences,
                            IGetSemanticId, IGetQualifiers
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
            public QualifierCollection GetQualifiers() => qualifiers;

            // from hasDataSpecification:
            [XmlElement(ElementName = "EmbeddedDataSpecification")]
            public HasDataSpecification hasDataSpecification = null;

            // from this very class
            [XmlIgnore]
            
            private SubmodelElementWrapperCollection _submodelElements = null;

            
            public SubmodelElementWrapperCollection submodelElements
            {
                get { return _submodelElements; }
                set { _submodelElements = value; _submodelElements.Parent = this; }
            }

            [XmlIgnore]
            [JsonProperty(PropertyName = "submodelElements")]
            public SubmodelElement[] JsonSubmodelElements
            {
                get
                {
                    var res = new ListOfSubmodelElement();
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

#if !DoNotUseAdminShell
            public Submodel(AdminShell.AdminShellV10.Submodel src, bool shallowCopy = false)
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

            public Submodel(AdminShell.AdminShellV20.Submodel src, bool shallowCopy = false)
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
#endif

            public static Submodel CreateNew(string idType, string id, string version = null, string revision = null)
            {
                var s = new Submodel() { id = new Identifier(id) };
                if (version != null)
                {
                    if (s.administration == null)
                        s.administration = new Administration();
                    s.administration.Version = version;
                    s.administration.Revision = revision;
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

            public EnumerationPlacmentBase GetChildrenPlacement(SubmodelElement child)
            {
                return null;
            }

            public object AddChild(SubmodelElementWrapper smw, EnumerationPlacmentBase placement = null)
            {
                if (smw == null)
                    return null;
                if (this.submodelElements == null)
                    this.submodelElements = new SubmodelElementWrapperCollection();
                if (smw.submodelElement != null)
                    smw.submodelElement.parent = this;
                this.submodelElements.Add(smw);
                return smw;
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
                if (index < 0 || index > submodelElements.Count)
                    return;
                submodelElements.Insert(index, sew);
            }

            public void Remove(SubmodelElement sme)
            {
                if (submodelElements != null)
                    submodelElements.Remove(sme);
            }

            // further

            public void AddQualifier(
                string qualifierType = null, string qualifierValue = null, KeyList semanticKeys = null,
                GlobalReference qualifierValueId = null)
            {
                QualifierCollection.AddQualifier(
                    ref this.qualifiers, qualifierType, qualifierValue, semanticKeys, qualifierValueId);
            }

            public Qualifier HasQualifierOfType(string qualifierType)
            {
                return QualifierCollection.HasQualifierOfType(this.qualifiers, qualifierType);
            }


            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("Submodel", "SM");
            }

            public SubmodelRef GetSubmodelRef()
                => new SubmodelRef(GetModelReference());

            /// <summary>
            ///  If instance, return semanticId as one key.
            ///  If template, return identification as key.
            /// </summary>
            public Key GetAutoSingleKey()
            {
                if (true == this.kind?.IsTemplate)
                    return new Key(this.GetElementName(), this.id?.value);
                else
                    return this.semanticId?.GetAsExactlyOneKey();
            }

            /// <summary>
            ///  If instance, return semanticId as one key.
            ///  If template, return identification as key.
            /// </summary>
            public Identifier GetAutoSingleId()
            {
                if (true == this.kind?.IsTemplate)
                    return new Identifier(this.id);
                else
                    return this.semanticId?.GetAsIdentifier(strict: true);
            }

            public void AddDataSpecification(Identifier id)
            {
                if (hasDataSpecification == null)
                    hasDataSpecification = new HasDataSpecification();
                hasDataSpecification.Add(new EmbeddedDataSpecification(new GlobalReference(id)));
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
                var caption = AdminShellUtil.EvalToNonNullString("\"{0}\" ", idShort, "<no idShort!>");
                if (administration != null)
                    caption += "V" + administration.Version + "." + administration.Revision;
                var info = "";
                if (id != null)
                    info = $"[{id.value}]";
                return Tuple.Create(caption, info);
            }

            public override string ToString()
            {
                var ci = ToCaptionInfo();
                return string.Format("{0}{1}", ci.Item1, (ci.Item2 != "") ? " / " + ci.Item2 : "");
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
                this.submodelElements?.RecurseOnReferables(state, null, (o, par, rf) =>
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
                this.submodelElements?.RecurseOnReferables(state, parents, lambda);
            }

            // Parents stuff

            public static void SetParentsForSME(Referable parent, SubmodelElement se)
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

            // validation

            public override void Validate(AasValidationRecordList results)
            {
                // access
                if (results == null)
                    return;

                // check
                base.Validate(results);
                ModelingKind.Validate(results, kind, this);
                ListOfIdentifier.Validate(results, semanticId?.Value, this);
            }

            // find

            public IEnumerable<LocatedReference> FindAllReferences()
            {
                // not nice: use temp list
                var temp = new List<ModelReference>();

                // recurse
                this.RecurseOnSubmodelElements(null, (state, parents, sme) =>
                {
                    if (sme is ModelReferenceElement mre)
                        if (mre.value != null)
                            temp.Add(mre.value);
                    if (sme is RelationshipElement rl)
                    {
                        if (rl.first != null)
                            temp.Add(rl.first);
                        if (rl.second != null)
                            temp.Add(rl.second);
                    }
                    // recurse
                    return true;
                });

                // now, give back
                foreach (var r in temp)
                    yield return new LocatedReference(this, r);
            }

            #region Timestamp
            public void SetAllParents(DateTime timeStamp)
            {
                if (this.submodelElements != null)
                    foreach (var sme in this.submodelElements)
                        SetParentsForSME(this, sme.submodelElement, timeStamp);
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

            #endregion
        }

    }
}
