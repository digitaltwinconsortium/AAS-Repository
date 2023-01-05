
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class PolicyInformationPoints
    {
        [Required]
        [DataMember(Name="externalInformationPoint")]
        [XmlElement(ElementName = "externalInformationPoint")]
        public bool? ExternalInformationPoint { get; set; }

        [DataMember(Name="internalInformationPoint")]
        [XmlArray(ElementName = "internalInformationPoint")]
        public List<Reference> InternalInformationPoint { get; set; }
    }
}
