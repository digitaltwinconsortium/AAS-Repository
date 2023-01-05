
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public partial class Permission
    {
        [Required]
        [DataMember(Name="kindOfPermission")]
        [XmlElement(ElementName = "kindOfPermission")]
        public KindOfPermissionEnum? KindOfPermission { get; set; }

        [Required]
        [DataMember(Name="permission")]
        [XmlElement(ElementName = "permission")]
        public Reference _Permission { get; set; }
    }
}
