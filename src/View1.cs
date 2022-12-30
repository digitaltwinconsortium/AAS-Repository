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
        public class View : Referable
        {
            // for JSON only
            [XmlIgnore]
            [JsonProperty(PropertyName = "modelType")]
            public JsonModelTypeWrapper JsonModelType { get { return new JsonModelTypeWrapper(GetElementName()); } }

            // members

            // from hasSemanticId:
            [XmlElement(ElementName = "semanticId")]
            public SemanticId semanticId = null;
            // from hasDataSpecification
            [XmlElement(ElementName = "hasDataSpecification")]
            public HasDataSpecification hasDataSpecification = null;

            // from this very class
            
            public ContainedElements containedElements = null;
            [XmlIgnore]
            [JsonProperty(PropertyName = "containedElements")]
            public ContainedElementRef[] JsonContainedElements
            {
                get { return containedElements?.reference.ToArray(); }
                set { containedElements = ContainedElements.CreateOrSetInner(containedElements, value); }
            }

            // getter / setter

            [XmlIgnore]
            
            public bool IsEmpty { get { return containedElements == null || containedElements.Count < 1; } }
            [XmlIgnore]
            
            public int Count { get { if (containedElements == null) return 0; return containedElements.Count; } }

            public ContainedElementRef this[int index] { get { if (containedElements == null) return null; return containedElements[index]; } }

            // constructors / creators

            public View() { }

            public View(View src)
                : base(src)
            {
                if (src.semanticId != null)
                    this.semanticId = new SemanticId(src.semanticId);
                if (src.hasDataSpecification != null)
                    this.hasDataSpecification = new HasDataSpecification(src.hasDataSpecification);
                if (src.containedElements != null)
                    this.containedElements = new ContainedElements(src.containedElements);
            }

            public static View CreateNew(string idShort)
            {
                var v = new View();
                v.idShort = idShort;
                return (v);
            }

            public void AddDataSpecification(Key k)
            {
                if (hasDataSpecification == null)
                    hasDataSpecification = new HasDataSpecification();
                var r = new Reference();
                r.Keys.Add(k);
                hasDataSpecification.reference.Add(r);
            }

            public void AddContainedElement(Key k)
            {
                if (containedElements == null)
                    containedElements = new ContainedElements();
                var r = new ContainedElementRef();
                r.Keys.Add(k);
                containedElements.reference.Add(r);
            }

            public void AddContainedElement(List<Key> keys)
            {
                if (containedElements == null)
                    containedElements = new ContainedElements();
                var r = new ContainedElementRef();
                foreach (var k in keys)
                    r.Keys.Add(k);
                containedElements.reference.Add(r);
            }

            public void AddContainedElement(Reference r)
            {
                if (containedElements == null)
                    containedElements = new ContainedElements();
                containedElements.reference.Add(ContainedElementRef.CreateNew(r));
            }

            public void AddContainedElement(List<Reference> rlist)
            {
                if (containedElements == null)
                    containedElements = new ContainedElements();
                foreach (var r in rlist)
                    containedElements.reference.Add(ContainedElementRef.CreateNew(r));
            }

            public override string GetElementName()
            {
                return "View";
            }

            public Tuple<string, string> ToCaptionInfo()
            {
                var caption = AdminShellUtil.EvalToNonNullString("\"{0}\" ", idShort, "<no idShort!>");
                var info = "";
                if (this.semanticId != null)
                    info = Key.KeyListToString(this.semanticId.Keys);
                if (this.containedElements != null && this.containedElements.reference != null)
                    info = (info + " ").Trim() + String.Format("({0} elements)", this.containedElements.reference.Count);
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