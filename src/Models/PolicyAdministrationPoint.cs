
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class PolicyAdministrationPoint
    {
        [Required]
        [DataMember(Name="externalAccessControl")]
        [XmlElement(ElementName = "externalAccessControl")]
        public bool? ExternalAccessControl { get; set; }

        [DataMember(Name="localAccessControl")]
        [XmlElement(ElementName = "localAccessControl")]
        public AccessControl LocalAccessControl { get; set; }
    }
}
