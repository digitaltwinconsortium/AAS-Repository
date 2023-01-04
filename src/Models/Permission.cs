
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public partial class Permission
    {
        [Required]
        [DataMember(Name="kindOfPermission")]
        public KindOfPermissionEnum? KindOfPermission { get; set; }

        [Required]
        [DataMember(Name="permission")]
        public Reference _Permission { get; set; }
    }
}
