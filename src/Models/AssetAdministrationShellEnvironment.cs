
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

        public AssetAdministrationShell FindAAS(Identifier id)
        {
            if (id == null)
                return null;

            foreach (var aas in AssetAdministrationShells)
                if (aas.Id != null && aas.Id.IsEqual(id))
                    return aas;

            return null;
        }

        public AssetAdministrationShell FindAAS(string idShort)
        {
            if (idShort == null)
                return null;

            foreach (var aas in AssetAdministrationShells)
                if (aas.IdShort != null && aas.IdShort.Trim().ToLower() == idShort.Trim().ToLower())
                    return aas;

            return null;
        }

        public Submodel FindSubmodel(Reference smref)
        {
            // trivial
            if (smref == null)
                return null;

            // can only refs with 1 key
            if (smref.Count != 1)
                return null;

            // and we're picky
            var key = smref.Keys[0];
            if (key.Type.ToString().ToLower().Trim() != "submodel")
                return null;

            // brute force
            foreach (var sm in Submodels)
                if (sm.Id.Value.ToLower().Trim() == key.Value.ToLower().Trim())
                    return sm;

            // uups
            return null;
        }

        public ConceptDescription FindConceptDescription(List<Key> keys)
        {
            // trivial
            if (keys == null)
                return null;

            // can only refs with 1 key
            if (keys.Count != 1)
                return null;

            // and we're picky
            var key = keys[0];

            if (key.Type.ToString().ToLower().Trim() != "conceptdescription")
                return null;

            // brute force
            foreach (var cd in ConceptDescriptions)
                if (cd.Id.Value.ToLower().Trim() == key.Value.ToLower().Trim())
                    return cd;

            // uups
            return null;
        }
    }
}
