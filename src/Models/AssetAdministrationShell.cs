
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class AssetAdministrationShell : Identifiable
    {
        [DataMember(Name = "hasDataSpecification")]
        [XmlElement(ElementName = "hasDataSpecification")]
        public HasDataSpecification HasDataSpecification { get; set; } = new();

        [DataMember(Name="embeddedDataSpecifications")]
        [XmlElement(ElementName = "embeddedDataSpecifications")]
        public List<EmbeddedDataSpecification> EmbeddedDataSpecifications { get; set; } = new();

        [DataMember(Name="derivedFrom")]
        [XmlElement(ElementName = "derivedFrom")]
        public Reference DerivedFrom { get; set; } = new();

        [DataMember(Name="assetInformation")]
        [XmlElement(ElementName = "assetInformation")]
        public AssetInformation AssetInformation { get; set; } = new();

        [DataMember(Name = "asset")]
        [XmlElement(ElementName = "asset")]
        public AssetReference Asset { get; set; } = new();

        [DataMember(Name="security")]
        [XmlElement(ElementName = "security")]
        public Security Security { get; set; } = new();

        [DataMember(Name="submodels")]
        [XmlElement(ElementName = "submodels")]
        public List<SubmodelReference> Submodels { get; set; } = new();

        [DataMember(Name="views")]
        [XmlElement(ElementName = "views")]
        public List<View> Views { get; set; } = new();

        [DataMember(Name = "conceptDictionaries")]
        [XmlElement(ElementName = "conceptDictionaries")]
        public List<ConceptDictionary> ConceptDictionaries { get; set; } = new();
    }
}
