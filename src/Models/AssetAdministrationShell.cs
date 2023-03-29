﻿
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class AssetAdministrationShell : Identifiable
    {
        [DataMember(Name="embeddedDataSpecifications")]
        [XmlArray(ElementName = "embeddedDataSpecifications")]
        public List<EmbeddedDataSpecification> EmbeddedDataSpecifications { get; set; } = new();

        [DataMember(Name="derivedFrom")]
        [XmlElement(ElementName = "derivedFrom")]
        public Reference DerivedFrom { get; set; } = new();

        [DataMember(Name="assetInformation")]
        [XmlElement(ElementName = "assetInformation")]
        public AssetInformation AssetInformation { get; set; } = new();

        [DataMember(Name="submodels")]
        [XmlArray(ElementName = "submodels")]
        public List<SubmodelReference> Submodels { get; set; } = new();

        [DataMember(Name="views")]
        [XmlArray(ElementName = "views")]
        public List<View> Views { get; set; } = new();
    }
}
