
namespace AdminShell
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "AssetRef")]
    public class AssetReference : GlobalReference
    {
    }
}
