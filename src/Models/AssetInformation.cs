
namespace AdminShell
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Serialization;

    public class AssetInformation : IAasElement
    {
        // as for V3RC02, Asset in no Referable anymore
        [XmlIgnore]
        public IAasElement parent = null;

        // V3RC02: instead of Identification
        public GlobalReference globalAssetId;

        // new in V3RC02
        public List<IdentifierKeyValuePair> specificAssetId = null;

        // new in V3RC02
        public Resource defaultThumbnail = null;

        // some fake information
        [XmlIgnore]
        public string fakeIdShort => Key.AssetInformation;

        [XmlIgnore]
        public Description fakeDescription => null;

        [XmlElement(ElementName = "assetKind")]
        public AssetKind assetKind = new AssetKind();

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
    }
}
