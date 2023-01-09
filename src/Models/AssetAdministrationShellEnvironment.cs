
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(ElementName = "aasenv")]
    public class AssetAdministrationShellEnvironment
    {
        [DataMember(Name = "assetAdministrationShells")]
        [XmlArray("assetAdministrationShells")]
        [XmlArrayItem("assetAdministrationShell")]
        public List<AssetAdministrationShell> AssetAdministrationShells { get; set; } = new();

        [DataMember(Name = "assets")]
        [XmlArray("assets")]
        [XmlArrayItem("asset")]
        public List<Asset> Assets { get; set; } = new();

        [DataMember(Name = "submodels")]
        [XmlArray("submodels")]
        [XmlArrayItem("submodel")]
        public List<Submodel> Submodels { get; set; } = new();

        [DataMember(Name = "conceptDescriptions")]
        [XmlArray("conceptDescriptions")]
        [XmlArrayItem("conceptDescription")]
        public List<ConceptDescription> ConceptDescriptions { get; set; } = new();
    }
}
