#define UseAasxCompatibilityModels

using System;
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
        public class Asset : Identifiable
        {
            // for JSON only
            [XmlIgnore]
            [JsonProperty(PropertyName = "modelType")]
            public JsonModelTypeWrapper JsonModelType { get { return new JsonModelTypeWrapper(GetElementName()); } }

            // from hasDataSpecification:
            [XmlElement(ElementName = "hasDataSpecification")]
            public HasDataSpecification hasDataSpecification = null;
            // from HasKind
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
            // from this very class
            [XmlElement(ElementName = "assetIdentificationModelRef")]
            public SubmodelRef assetIdentificationModelRef = null;

            // constructors

            public Asset() { }

            public Asset(Asset src)
                : base(src)
            {
                if (src.hasDataSpecification != null)
                    this.hasDataSpecification = new HasDataSpecification(src.hasDataSpecification);
                if (src.kind != null)
                    this.kind = new Kind(src.kind);
                if (src.assetIdentificationModelRef != null)
                    this.assetIdentificationModelRef = new SubmodelRef(src.assetIdentificationModelRef);
            }

            // Getter & setters

            public AssetRef GetReference()
            {
                var r = new AssetRef();
                r.Keys.Add(Key.CreateNew(this.GetElementName(), true, this.identification.idType, this.identification.id));
                return r;
            }

            public override string GetElementName()
            {
                return "Asset";
            }

            public Tuple<string, string> ToCaptionInfo()
            {
                var caption = AdminShellUtil.EvalToNonNullString("\"{0}\" ", idShort, "<no idShort!>");
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

        }

    }

    #endregion
}

#endif