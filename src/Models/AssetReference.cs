
namespace AdminShell
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "assetRef")]
    public class AssetReference : GlobalReference
    {
    }
}
