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
        public class Asset : Identifiable, IGetReference
        {
            // for JSON only
            [XmlIgnore]
            [JsonProperty(PropertyName = "modelType")]
            public JsonModelTypeWrapper JsonModelType { get { return new JsonModelTypeWrapper(GetElementName()); } }

            // from hasDataSpecification:
            [XmlElement(ElementName = "hasDataSpecification")]
            public HasDataSpecification hasDataSpecification = null;

            // from this very class
            [XmlElement(ElementName = "assetIdentificationModelRef")]
            public SubmodelRef assetIdentificationModelRef = null;

            [XmlElement(ElementName = "billOfMaterialRef")]
            public SubmodelRef billOfMaterialRef = null;

            // from HasKind
            [XmlElement(ElementName = "kind")]
            
            public AssetKind kind = new AssetKind();
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
                        kind = new AssetKind();
                    kind.kind = value;
                }
            }

            // constructors

            public Asset() { }

            public Asset(string idShort) : base(idShort) { }

            public Asset(Asset src)
                : base(src)
            {
                if (src != null)
                {
                    if (src.hasDataSpecification != null)
                        this.hasDataSpecification = new HasDataSpecification(src.hasDataSpecification);
                    if (src.kind != null)
                        this.kind = new AssetKind(src.kind);
                    if (src.assetIdentificationModelRef != null)
                        this.assetIdentificationModelRef = new SubmodelRef(src.assetIdentificationModelRef);
                }
            }

#if !DoNotUseAasxCompatibilityModels
            public Asset(AasxCompatibilityModels.AdminShellV10.Asset src)
                : base(src)
            {
                if (src != null)
                {
                    if (src.hasDataSpecification != null)
                        this.hasDataSpecification = new HasDataSpecification(src.hasDataSpecification);
                    if (src.kind != null)
                        this.kind = new AssetKind(src.kind);
                    if (src.assetIdentificationModelRef != null)
                        this.assetIdentificationModelRef = new SubmodelRef(src.assetIdentificationModelRef);
                }
            }
#endif

            // Getter & setters

            public AssetRef GetAssetReference()
            {
                var r = new AssetRef();
                r.Keys.Add(
                    Key.CreateNew(
                        this.GetElementName(), true, this.identification.idType, this.identification.id));
                return r;
            }

            public Reference GetReference()
            {
                return GetAssetReference();
            }

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("Asset", "Asset");
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

            public IEnumerable<Reference> FindAllReferences()
            {
                if (this.assetIdentificationModelRef != null)
                    yield return this.assetIdentificationModelRef;
                if (this.billOfMaterialRef != null)
                    yield return this.billOfMaterialRef;
            }
        }



    }

    #endregion
}

