
namespace AdminShell
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class AssetAdministrationShell : Identifiable
    {
        [DataMember(Name = "hasDataSpecification")]
        public HasDataSpecification HasDataSpecification { get; set; } = new();

        [DataMember(Name="embeddedDataSpecifications")]
        public List<EmbeddedDataSpecification> EmbeddedDataSpecifications { get; set; } = new();

        [DataMember(Name="derivedFrom")]
        public Reference DerivedFrom { get; set; } = new();

        [Required]
        [DataMember(Name="assetInformation")]
        public AssetInformation AssetInformation { get; set; } = new();

        [DataMember(Name = "asset")]
        public AssetRef Asset { get; set; } = new();

        [DataMember(Name="security")]
        public Security Security { get; set; } = new();

        [DataMember(Name="Submodels")]
        public List<Reference> Submodels { get; set; } = new();

        [DataMember(Name="views")]
        public List<View> Views { get; set; } = new();

        [DataMember(Name = "conceptDictionaries")]
        public List<ConceptDictionary> ConceptDictionaries { get; set; } = new();

        public string GetElementName()
        {
            return "AssetAdminShell";
        }

        public void AddView(View v)
        {
            if (Views == null)
                Views = new List<View>();

            Views.Add(v);
        }

        public void AddDataSpecification(Identifier id)
        {
            var r = new Reference();
            r.Keys.Add(k);
            HasDataSpecification.Add(new EmbeddedDataSpecification(r));
        }

        public AasElementSelfDescription GetSelfDescription()
        {
            return new AasElementSelfDescription("AssetAdministrationShell", "AAS");
        }

        public Tuple<string, string> ToCaptionInfo()
        {
            var caption = AdminShellUtil.EvalToNonNullString("\"{0}\" ", idShort, "\"AAS\"");
            if (administration != null)
                caption += "V" + administration.Version + "." + administration.Revision;

            var info = "";
            if (id != null)
                info = $"[{id.value}]";
            return Tuple.Create(caption, info);
        }

        public override string ToString()
        {
            var ci = ToCaptionInfo();
            return string.Format("{0}{1}", ci.Item1, (ci.Item2 != "") ? " / " + ci.Item2 : "");
        }

        public SubmodelRef FindSubmodelRef(Identifier refid)
        {
            if (this.submodelRefs == null)
                return null;
            foreach (var r in this.submodelRefs)
                if (r.Matches(refid))
                    return r;
            return null;
        }

        public bool HasSubmodelRef(SubmodelRef newref)
        {
            // check, if existing
            if (this.submodelRefs == null)
                return false;
            var found = false;
            foreach (var r in this.submodelRefs)
                if (r.Matches(newref))
                    found = true;

            return found;
        }

        public void AddSubmodelRef(SubmodelRef newref)
        {
            if (this.submodelRefs == null)
                this.submodelRefs = new List<SubmodelRef>();
            this.submodelRefs.Add(newref);
        }

        public IEnumerable<LocatedReference> FindAllReferences()
        {
            if (this.assetRef != null)
                yield return this.assetRef;

            // Submodel references
            if (this.submodelRefs != null)
                foreach (var r in this.submodelRefs)
                    yield return new LocatedReference(this, r);

            if (this.views?.views != null)
                foreach (var vw in this.views.views)
                    if (vw?.containedElements?.reference != null)
                        foreach (var r in vw.containedElements.reference)
                            yield return r;
        }

        public ModelReference GetReference()
        {
            return ModelReference.CreateNew(new Key(GetElementName(), "" + id?.value));
        }
    }
}
