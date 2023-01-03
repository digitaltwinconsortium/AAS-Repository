
namespace AdminShell
{
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

        [XmlElement(ElementName = "assetKind")]
        public AssetKind assetKind = new AssetKind();
    }
}
