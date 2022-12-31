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
using Newtonsoft.Json;

//namespace AdminShell
namespace AdminShell_V30
{
    public partial class AdminShellV30
    {
        //
        // Asset
        //

        public class AssetInformation : IAasElement
        {
            // as for V3RC02, Asset in no Referable anymore
            [XmlIgnore]
            
             // important to skip, as recursion elsewise will go in cycles!
             // important to skip, as recursion elsewise will go in cycles!
            public IAasElement parent = null;

            // V3RC02: instead of Identification
            public GlobalReference globalAssetId;

            // new in V3RC02
            public ListOfIdentifierKeyValuePair specificAssetId = null;

            // new in V3RC02
            public Resource defaultThumbnail = null;

            // some fake information
            [XmlIgnore]
            
            public string fakeIdShort => Key.AssetInformation;

            [XmlIgnore]
            
            public Description fakeDescription => null;

            // from HasKind
            [XmlElement(ElementName = "assetKind")]
            
            public AssetKind assetKind = new AssetKind();
            [XmlIgnore]
            [JsonProperty(PropertyName = "assetKind")]
            public string JsonKind
            {
                get
                {
                    if (assetKind == null)
                        return null;
                    return assetKind.kind;
                }
                set
                {
                    if (assetKind == null)
                        assetKind = new AssetKind();
                    assetKind.kind = value;
                }
            }

            // constructors

            public AssetInformation() { }

            public AssetInformation(string fakeIdShort)
            {
                // empty, because V3RC02 does not foresee storage anymore
            }

            public AssetInformation(AssetInformation src)
            {
                if (src == null)
                    return;

                if (src.assetKind != null)
                    assetKind = new AssetKind(src.assetKind);

                if (src.globalAssetId != null)
                    globalAssetId = new GlobalReference();

                if (src.specificAssetId != null)
                    specificAssetId = new ListOfIdentifierKeyValuePair(src.specificAssetId);

                if (src.defaultThumbnail != null)
                    defaultThumbnail = new Resource(src.defaultThumbnail);
            }

#if !DoNotUseAasxCompatibilityModels
            public AssetInformation(AasxCompatibilityModels.AdminShellV10.Asset src)
            {
                if (src == null)
                    return;

                if (src.kind != null)
                    this.assetKind = new AssetKind(src.kind);

                if (src.identification != null)
                    SetIdentification(src.identification.id);
            }

            public AssetInformation(AasxCompatibilityModels.AdminShellV20.Asset src)
            {
                if (src == null)
                    return;

                if (src.kind != null)
                    this.assetKind = new AssetKind(src.kind);

                if (src.identification != null)
                    SetIdentification(src.identification.id);
            }
#endif

            // Getter & setters

            public AssetRef GetAssetReference() => new AssetRef(globalAssetId);

            public string GetElementName() => Key.AssetInformation;

            public AasElementSelfDescription GetSelfDescription()
                => new AasElementSelfDescription(Key.AssetInformation, "Asset");

            public Tuple<string, string> ToCaptionInfo()
            {
                var caption = Key.AssetInformation;
                var info = "" + globalAssetId.ToString(1);
                return Tuple.Create(caption, info);
            }

            public override string ToString()
            {
                var ci = ToCaptionInfo();
                return string.Format("{0}{1}", ci.Item1, (ci.Item2 != "") ? " / " + ci.Item2 : "");
            }

            public IEnumerable<Reference> FindAllReferences()
            {
                yield break;
            }

            public void AddDescription(string lang, string str)
            {
                // empty, because V3RC02 does not foresee storage anymore
            }

            public void SetIdentification(Identifier id)
            {
                if (id == null)
                    return;
                globalAssetId = new GlobalReference(new Identifier(id));
            }
        }

    }
}
