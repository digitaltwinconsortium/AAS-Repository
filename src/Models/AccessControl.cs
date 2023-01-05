
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class AccessControl
    {
        [DataMember(Name="accessPermissionRule")]
        [XmlElement(ElementName = "accessPermissionRule")]
        public List<AccessPermissionRule> AccessPermissionRule { get; set; }

        [DataMember(Name="defaultEnvironmentAttributes")]
        [XmlElement(ElementName = "defaultEnvironmentAttributes")]
        public Reference DefaultEnvironmentAttributes { get; set; }

        [DataMember(Name="defaultPermissions")]
        [XmlElement(ElementName = "defaultPermissions")]
        public Reference DefaultPermissions { get; set; }

        [DataMember(Name="defaultSubjectAttributes")]
        [XmlElement(ElementName = "defaultSubjectAttributes")]
        public Reference DefaultSubjectAttributes { get; set; }

        [DataMember(Name="selectableEnvironmentAttributes")]
        [XmlElement(ElementName = "selectableEnvironmentAttributes")]
        public Reference SelectableEnvironmentAttributes { get; set; }

        [DataMember(Name="selectablePermissions")]
        [XmlElement(ElementName = "selectablePermissions")]
        public Reference SelectablePermissions { get; set; }

        [DataMember(Name="selectableSubjectAttributes")]
        [XmlElement(ElementName = "selectableSubjectAttributes")]
        public Reference SelectableSubjectAttributes { get; set; }
    }
}
