
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Runtime.Serialization;

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
}
