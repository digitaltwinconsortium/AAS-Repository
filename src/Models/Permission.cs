
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class Permission
    {
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public enum KindOfPermissionEnum
        {
            [EnumMember(Value = "Allow")]
            AllowEnum = 0,

            [EnumMember(Value = "Deny")]
            DenyEnum = 1,

            [EnumMember(Value = "NotApplicable")]
            NotApplicableEnum = 2,

            [EnumMember(Value = "Undefined")]
            UndefinedEnum = 3
        }

        [Required]
        [DataMember(Name="kindOfPermission")]
        public KindOfPermissionEnum? KindOfPermission { get; set; }

        [Required]
        [DataMember(Name="permission")]
        public Reference _Permission { get; set; }
    }
}
