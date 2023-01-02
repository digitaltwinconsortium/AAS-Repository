
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class AccessPermissionRule : Referable
    {
        [DataMember(Name="qualifiers")]
        public List<Constraint> Qualifiers { get; set; }

        [DataMember(Name="permissionsPerObject")]
        public List<PermissionsPerObject> PermissionsPerObject { get; set; }

        [Required]
        [DataMember(Name="targetSubjectAttributes")]
        public List<SubjectAttributes> TargetSubjectAttributes { get; set; }
    }
}
