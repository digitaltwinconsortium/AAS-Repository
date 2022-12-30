#define UseAasxCompatibilityModels

using System;
using System.Collections.Generic;
using System.Xml;
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
        public class SubmodelElement : Referable, System.IDisposable, IGetReference
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
            // from hasSemantics:
            [XmlElement(ElementName = "semanticId")]
            public SemanticId semanticId = null;
            // from hasKind:
            [XmlElement(ElementName = "kind")]
            
            public Kind kind = null;
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
            [JsonProperty(PropertyName = "constraints")]
            public List<Qualifier> qualifiers = null;

            // getter / setter

            // constructors / creators

            public SubmodelElement()
                : base() { }

            public SubmodelElement(SubmodelElement src)
                : base(src)
            {
                if (src.hasDataSpecification != null)
                    hasDataSpecification = new HasDataSpecification(src.hasDataSpecification);
                if (src.semanticId != null)
                    semanticId = new SemanticId(src.semanticId);
                if (src.kind != null)
                    kind = new Kind(src.kind);
                if (src.qualifiers != null)
                {
                    if (qualifiers == null)
                        qualifiers = new List<Qualifier>();
                    foreach (var q in src.qualifiers)
                        qualifiers.Add(new Qualifier(q));
                }
            }

            public void CreateNewLogic(string idShort = null, string category = null, Key semanticIdKey = null)
            {
                if (idShort != null)
                    this.idShort = idShort;
                if (category != null)
                    this.category = category;
                if (semanticIdKey != null)
                {
                    if (this.semanticId == null)
                        this.semanticId = new SemanticId();
                    this.semanticId.Keys.Add(semanticIdKey);
                }
            }

            public void AddQualifier(string qualifierType = null, string qualifierValue = null, KeyList semanticKeys = null, Reference qualifierValueId = null)
            {
                if (this.qualifiers == null)
                    this.qualifiers = new List<Qualifier>();
                var q = new Qualifier();
                q.qualifierType = qualifierType;
                q.qualifierValue = qualifierValue;
                q.qualifierValueId = qualifierValueId;
                if (semanticKeys != null)
                    q.semanticId = SemanticId.CreateFromKeys(semanticKeys.Keys);
                this.qualifiers.Add(q);
            }

            public Qualifier HasQualifierOfType(string qualifierType)
            {
                if (this.qualifiers == null || qualifierType == null)
                    return null;
                foreach (var q in this.qualifiers)
                    if (q.qualifierType.Trim().ToLower() == qualifierType.Trim().ToLower())
                        return q;
                return null;
            }

            public override string GetElementName()
            {
                return "SubmodelElement";
            }

            public Reference GetReference()
            {
                Reference r = new Reference();
                // this is the tail of our referencing chain ..
                r.Keys.Add(Key.CreateNew(GetElementName(), true, "idShort", this.idShort));
                // try to climb up ..
                var current = this.parent;
                while (current != null)
                {
                    if (current is Identifiable)
                    {
                        // add big information set
                        r.Keys.Insert(0, Key.CreateNew(
                            current.GetElementName(),
                            true,
                            (current as Identifiable).identification.idType,
                            (current as Identifiable).identification.id));
                    }
                    else
                    {
                        // reference via idShort
                        r.Keys.Insert(0, Key.CreateNew(
                            current.GetElementName(),
                            true,
                            "idShort", this.idShort));
                    }
                    current = current.parent;
                }
                return r;
            }

            public Tuple<string, string> ToCaptionInfo()
            {
                var caption = AdminShellUtil.EvalToNonNullString("\"{0}\" ", idShort, "<no idShort!>");
                var info = "";
                if (semanticId != null)
                    AdminShellUtil.EvalToNonEmptyString("\u21e8 {0}", semanticId.ToString(), "");
                return Tuple.Create(caption, info);
            }

            public override string ToString()
            {
                var ci = ToCaptionInfo();
                return string.Format("{0}{1}", ci.Item1, (ci.Item2 != "") ? " / " + ci.Item2 : "");
            }


        }

    }

    #endregion
}

#endif