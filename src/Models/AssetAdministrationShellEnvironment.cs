
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

        [DataMember(Name = "ConceptDescriptions")]
        [XmlArray("ConceptDescriptions")]
        [XmlArrayItem("conceptDescription")]
        public List<ConceptDescription> ConceptDescriptions { get; set; } = new();

        [DataMember(Name = "Submodels")]
        [XmlArray("Submodels")]
        [XmlArrayItem("submodel")]
        public List<Submodel> Submodels { get; set; } = new();

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

        public Submodel FindSubmodel(Identifier id)
        {
            if (id == null)
                return null;
            foreach (var sm in Submodels)
                if (sm.Id != null && sm.Id.IsEqual(id))
                    return sm;
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

        public ConceptDescription FindConceptDescription(List<Identifier> loi)
        {
            // trivial
            if (loi == null)
                return null;

            // can only refs with 1 key
            if (loi.Count != 1)
                return null;

            // and we're picky
            var id = loi[0];

            // brute force
            foreach (var cd in ConceptDescriptions)
                if (cd.Id.Value.ToLower().Trim() == id.Value.ToLower().Trim())
                    return cd;

            // uups
            return null;
        }

        public ConceptDescription FindConceptDescription(Key key)
        {
            if (key == null)
                return null;

            var l = new List<Key> { key };

            return FindConceptDescription(l);
        }
    }
}
