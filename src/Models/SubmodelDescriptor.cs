
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class SubmodelDescriptor : Descriptor
    {
        [DataMember(Name="administration")]
        [XmlElement(ElementName = "administration")]
        public AdministrativeInformation Administration { get; set; }

        [DataMember(Name="description")]
        [XmlElement(ElementName = "description")]
        public List<LangString> Description { get; set; }

        [DataMember(Name="idShort")]
        [XmlElement(ElementName = "idShort")]
        public string IdShort { get; set; }

        [Required]
        [DataMember(Name="identification")]
        [XmlElement(ElementName = "identification")]
        public string Identification { get; set; }

        [DataMember(Name="semanticId")]
        [XmlElement(ElementName = "semanticId")]
        public Reference SemanticId { get; set; }
    }
}
