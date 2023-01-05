
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class PermissionsPerObject
    {
        [DataMember(Name="object")]
        [XmlElement(ElementName = "object")]
        public Reference Object { get; set; }

        [DataMember(Name="permission")]
        [XmlArray(ElementName = "permission")]
        public List<Permission> Permission { get; set; }

        [DataMember(Name="targetObjectAttributes")]
        [XmlElement(ElementName = "targetObjectAttributes")]
        public ObjectAttributes TargetObjectAttributes { get; set; }
    }
}
