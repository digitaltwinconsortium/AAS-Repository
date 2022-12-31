/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using AdminShell;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;


//namespace AdminShell
namespace AdminShell_V30
{
    public partial class AdminShellV30
    {
        [JsonConverter(typeof(AdminShellConverters.JsonAasxConverter))]
        public class SubmodelElement : Referable, System.IDisposable, IGetModelReference, IGetSemanticId, IGetQualifiers
        {
            // constants
            public static Type[] PROP_MLP = new Type[] {
            typeof(AdminShell.MultiLanguageProperty), typeof(AdminShell.Property) };

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
            [JsonProperty(PropertyName = "constraints")]
            public QualifierCollection qualifiers = null;
            public QualifierCollection GetQualifiers() => qualifiers;

            // from hasDataSpecification:
            [XmlElement(ElementName = "embeddedDataSpecification")]
            public HasDataSpecification hasDataSpecification = null;

            // getter / setter

            // constructors / creators

            public SubmodelElement()
                : base() { }

            public SubmodelElement(SubmodelElement src)
                : base(src)
            {
                if (src == null)
                    return;
                if (src.hasDataSpecification != null)
                    hasDataSpecification = new HasDataSpecification(src.hasDataSpecification);
                if (src.semanticId != null)
                    semanticId = new SemanticId(src.semanticId);
                if (src.kind != null)
                    kind = new ModelingKind(src.kind);
                if (src.qualifiers != null)
                {
                    if (qualifiers == null)
                        qualifiers = new QualifierCollection();
                    foreach (var q in src.qualifiers)
                        qualifiers.Add(new Qualifier(q));
                }
            }

#if !DoNotUseAasxCompatibilityModels
            public SubmodelElement(AasxCompatibilityModels.AdminShellV10.SubmodelElement src)
                : base(src)
            {
                if (src.hasDataSpecification != null)
                    hasDataSpecification = new HasDataSpecification(src.hasDataSpecification);
                if (src.semanticId != null)
                    semanticId = new SemanticId(src.semanticId);
                if (src.kind != null)
                    kind = new ModelingKind(src.kind);
                if (src.qualifiers != null)
                {
                    if (qualifiers == null)
                        qualifiers = new QualifierCollection(src.qualifiers);
                    foreach (var q in src.qualifiers)
                        qualifiers.Add(new Qualifier(q));
                }
            }

            public SubmodelElement(AasxCompatibilityModels.AdminShellV20.SubmodelElement src)
                : base(src)
            {
                if (src == null)
                    return;
                if (src.hasDataSpecification != null)
                    hasDataSpecification = new HasDataSpecification(src.hasDataSpecification);
                if (src.semanticId != null)
                    semanticId = new SemanticId(src.semanticId);
                if (src.kind != null)
                    kind = new ModelingKind(src.kind);
                if (src.qualifiers != null)
                {
                    if (qualifiers == null)
                        qualifiers = new QualifierCollection();
                    foreach (var q in src.qualifiers)
                        qualifiers.Add(new Qualifier(q));
                }
            }
#endif

            public static T CreateNew<T>(string idShort = null, string category = null, Reference semanticId = null)
                where T : SubmodelElement, new()
            {
                var res = new T();
                if (idShort != null)
                    res.idShort = idShort;
                if (category != null)
                    res.category = category;
                if (semanticId != null)
                    res.semanticId = new SemanticId(semanticId);
                return res;
            }

            public void CreateNewLogic(string idShort = null, string category = null, Identifier semanticIdKey = null)
            {
                if (idShort != null)
                    this.idShort = idShort;
                if (category != null)
                    this.category = category;
                if (semanticIdKey != null)
                {
                    if (this.semanticId == null)
                        this.semanticId = new SemanticId();
                    this.semanticId.Value.Add(semanticIdKey);
                }
            }

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
                return new AasElementSelfDescription("SubmodelElement", "SME");
            }

            public override ModelReference GetModelReference(bool includeParents = true)
            {
                var r = new ModelReference();
                // semantic id
                r.referredSemanticId = new GlobalReference(semanticId);
                // this is the tail of our referencing chain ..
                r.Keys.Add(Key.CreateNew(GetElementName(), this.idShort));
                // try to climb up ..
                var current = this.parent;
                while (includeParents && current != null)
                {
                    if (current is Identifiable cid)
                    {
                        // add big information set
                        r.Keys.Insert(0, Key.CreateNew(
                            current.GetElementName(),
                            cid.id.value));
                    }
                    else
                    if (current is Referable crf)
                    {
                        // reference via idShort
                        r.Keys.Insert(0, Key.CreateNew(
                            current.GetElementName(),
                            crf.idShort));
                    }

                    if (current is Referable crf2)
                        current = crf2.parent;
                    else
                        current = null;
                }
                return r;
            }

            public IEnumerable<Referable> FindAllParents(
                Predicate<Referable> p,
                bool includeThis = false, bool includeSubmodel = false,
                bool passOverMiss = false)
            {
                // call for this?
                if (includeThis)
                {
                    if (p == null || p.Invoke(this))
                        yield return this;
                    else
                        if (!passOverMiss)
                        yield break;
                }

                // daisy chain all parents ..
                if (this.parent != null)
                {
                    if (this.parent is SubmodelElement psme)
                    {
                        foreach (var q in psme.FindAllParents(p, includeThis: true,
                            passOverMiss: passOverMiss))
                            yield return q;
                    }
                    else if (includeSubmodel && this.parent is Submodel psm)
                    {
                        if (p == null || p.Invoke(psm))
                            yield return this;
                    }
                }
            }

            public IEnumerable<Referable> FindAllParentsWithSemanticId(
                SemanticId semId,
                bool includeThis = false, bool includeSubmodel = false, bool passOverMiss = false)
            {
                return (FindAllParents(
                    (rf) => true == (rf as IGetSemanticId)?.GetSemanticId()?.Matches(semId),
                    includeThis: includeThis, includeSubmodel: includeSubmodel, passOverMiss: passOverMiss));
            }

            public Tuple<string, string> ToCaptionInfo()
            {
                var caption = AdminShellUtil.EvalToNonNullString("\"{0}\" ", idShort, "<no idShort!>");
                var info = "";
                // TODO (MIHO, 2021-07-08): obvious error .. info should receive semanticId .. but would change 
                // display presentation .. therefore to be checked again
                if (semanticId != null)
                    AdminShellUtil.EvalToNonEmptyString("\u21e8 {0}", semanticId.ToString(), "");
                return Tuple.Create(caption, info);
            }

            public override string ToString()
            {
                var ci = ToCaptionInfo();
                return string.Format("{0}{1}", ci.Item1, (ci.Item2 != "") ? " / " + ci.Item2 : "");
            }

            public virtual string ValueAsText(string defaultLang = null)
            {
                return "";
            }

            public virtual double? ValueAsDouble()
            {
                return null;
            }

            public virtual void ValueFromText(string text, string defaultLang = null)
            {
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

            public virtual object ToValueOnlySerialization()
            {
                throw new NotImplementedException();
            }

        }

    }
}
