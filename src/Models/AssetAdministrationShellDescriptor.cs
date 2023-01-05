
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class AssetAdministrationShellDescriptor : Descriptor
    {
        [DataMember(Name="administration")]
        [XmlElement(ElementName = "administration")]
        public AdministrativeInformation Administration { get; set; }

        [DataMember(Name="description")]
        [XmlElement(ElementName = "description")]
        public List<LangString> Description { get; set; }

        [DataMember(Name="globalAssetId")]
        [XmlElement(ElementName = "globalAssetId")]
        public Reference GlobalAssetId { get; set; }

        [DataMember(Name="idShort")]
        [XmlElement(ElementName = "idShort")]
        public string IdShort { get; set; }

        [Required]
        [DataMember(Name="identification")]
        [XmlElement(ElementName = "identification")]
        public string Identification { get; set; }

        [DataMember(Name="specificAssetIds")]
        [XmlElement(ElementName = "specificAssetIds")]
        public List<IdentifierKeyValuePair> SpecificAssetIds { get; set; }

        [DataMember(Name="submodelDescriptors")]
        [XmlElement(ElementName = "submodelDescriptors")]
        public List<SubmodelDescriptor> SubmodelDescriptors { get; set; }
    }
}
