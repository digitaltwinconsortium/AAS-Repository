
namespace AdminShell
{
    using System.Xml.Serialization;

    [XmlType(TypeName = "assetRef")]
    public class AssetRef : GlobalReference
    {
        public AssetRef() : base() { }

        public AssetRef(GlobalReference r) : base(r) { }

        public AasElementSelfDescription GetSelfDescription()
        {
            return new AasElementSelfDescription("AssetRef", "AssetRef");
        }
    }

}
