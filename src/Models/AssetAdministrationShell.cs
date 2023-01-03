
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

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
        public List<SubmodelReference> Submodels { get; set; } = new();

        [DataMember(Name="views")]
        public List<View> Views { get; set; } = new();

        [DataMember(Name = "conceptDictionaries")]
        public List<ConceptDictionary> ConceptDictionaries { get; set; } = new();
    }
}
