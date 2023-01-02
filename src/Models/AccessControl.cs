
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class AccessControl
    {
        [DataMember(Name="accessPermissionRule")]
        public List<AccessPermissionRule> AccessPermissionRule { get; set; }

        [DataMember(Name="defaultEnvironmentAttributes")]
        public Reference DefaultEnvironmentAttributes { get; set; }

        [DataMember(Name="defaultPermissions")]
        public Reference DefaultPermissions { get; set; }

        [DataMember(Name="defaultSubjectAttributes")]
        public Reference DefaultSubjectAttributes { get; set; }

        [DataMember(Name="selectableEnvironmentAttributes")]
        public Reference SelectableEnvironmentAttributes { get; set; }

        [DataMember(Name="selectablePermissions")]
        public Reference SelectablePermissions { get; set; }

        [DataMember(Name="selectableSubjectAttributes")]
        public Reference SelectableSubjectAttributes { get; set; }
    }
}
