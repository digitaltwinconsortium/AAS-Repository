
namespace AdminShell
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "aasenv", Namespace = "http://www.admin-shell.io/aas/3/0")]
    [DataContract]
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

        [XmlAttribute(Namespace = System.Xml.Schema.XmlSchema.InstanceNamespace)]
        public string schemaLocation = "http://www.admin-shell.io/aas/3/0 AAS.xsd http://www.admin-shell.io/IEC61360/3/0 IEC61360.xsd";

        [DataMember(Name = "assets")]
        [XmlArray("Assets")]
        [XmlArrayItem("asset")]
        private List<AssetInformation> Assets = new();

        public AssetAdministrationShell FindAAS(Identifier id)
        {
            if (id == null)
                return null;
            foreach (var aas in AssetAdministrationShells)
                if (aas.id != null && aas.id.IsEqual(id))
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
                if (sm.id != null && sm.id.IsEqual(id))
                    return sm;

            return null;
        }

        public Submodel FindSubmodel(SubmodelReference smref)
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
                if (sm.id.value.ToLower().Trim() == key.Value.ToLower().Trim())
                    return sm;

            // uups
            return null;
        }

        public ConceptDescription FindConceptDescription(Reference cdr)
        {
            if (cdr == null)
                return null;

            return FindConceptDescription(cdr.Keys);
        }

        public ConceptDescription FindConceptDescription(SemanticId semId)
        {
            if (semId == null)
                return null;

            return FindConceptDescription(semId.Value);
        }

        public ConceptDescription FindConceptDescription(ModelReference rf)
        {
            if (rf == null)
                return null;

            return FindConceptDescription(rf.Keys);
        }

        public ConceptDescription FindConceptDescription(Identifier id)
        {
            return FindConceptDescription(new Reference(new Key(Key.ConceptDescription, id.value)));
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
                if (cd.id.value.ToLower().Trim() == key.Value.ToLower().Trim())
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
                if (cd.id.value.ToLower().Trim() == id.value.ToLower().Trim())
                    return cd;
            // uups
            return null;
        }

        public ConceptDescription FindConceptDescription(Key key)
        {
            if (key == null)
                return null;
            var l = new List<Key> { key };
            return (FindConceptDescription(l));
        }

        private void CopyConceptDescriptionsFrom(
            AssetAdministrationShellEnvironment srcEnv, SubmodelElement src, bool shallowCopy = false)
        {
            // access
            if (srcEnv == null || src == null || src.SemanticId == null)
                return;
            // check for this SubmodelElement in Source
            var cdSrc = srcEnv.FindConceptDescription(src.SemanticId);
            if (cdSrc == null)
                return;
            // check for this SubmodelElement in Destnation (this!)
            var cdDest = FindConceptDescription(src.SemanticId);
            if (cdDest != null)
                return;
            // copy new
            ConceptDescriptions.Add(new ConceptDescription(cdSrc));
            // recurse?
            if (!shallowCopy && src is SubmodelElementCollection)
                foreach (var m in (src as SubmodelElementCollection).Value)
                    CopyConceptDescriptionsFrom(srcEnv, m.submodelElement, shallowCopy: false);

        }

        private static void CreateFromExistingEnvRecurseForCDs(
            AssetAdministrationShellEnvironment src, List<SubmodelElementWrapper> wrappers,
            ref List<ConceptDescription> filterForCD)
        {
            if (wrappers == null || filterForCD == null)
                return;

            foreach (var w in wrappers)
            {
                // access
                if (w == null)
                    continue;

                // include in filter ..
                if (w.submodelElement.SemanticId != null)
                {
                    var cd = src.FindConceptDescription(w.submodelElement.SemanticId);
                    if (cd != null)
                        filterForCD.Add(cd);
                }

                // recurse?
                if (w.submodelElement is SubmodelElementCollection smec)
                    CreateFromExistingEnvRecurseForCDs(src, smec.Value, ref filterForCD);

                if (w.submodelElement is Operation op)
                    for (int i = 0; i < 2; i++)
                    {
                        var w2s = Operation.GetWrappers(op[i]);
                        CreateFromExistingEnvRecurseForCDs(src, w2s, ref filterForCD);
                    }

                if (w.submodelElement is Entity smee)
                    CreateFromExistingEnvRecurseForCDs(src, smee.Statements, ref filterForCD);

                if (w.submodelElement is AnnotatedRelationshipElement smea)
                    CreateFromExistingEnvRecurseForCDs(src, smea.annotations, ref filterForCD);
            }
        }
    }
}
