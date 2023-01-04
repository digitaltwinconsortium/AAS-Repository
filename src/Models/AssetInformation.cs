
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class AssetInformation : IAasElement
    {
        [XmlIgnore]
        public IAasElement Parent { get; set; }

        [DataMember(Name = "globalAssetId")]
        public GlobalReference GlobalAssetId { get; set; }

        [DataMember(Name = "specificAssetIds")]
        public List<IdentifierKeyValuePair> SpecificAssetIds { get; set; }

        [DataMember(Name = "defaultThumbnail")]
        public Resource DefaultThumbnail { get; set; }

        [DataMember(Name = "assetKind")]
        public AssetKind AssetKind { get; set; } = new();
    }
}
