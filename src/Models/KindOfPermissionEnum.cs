
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Runtime.Serialization;

    [DataContract]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum KindOfPermissionEnum
    {
        [EnumMember(Value = "Allow")]
        Allow = 0,

        [EnumMember(Value = "Deny")]
        Deny = 1,

        [EnumMember(Value = "NotApplicable")]
        NotApplicable = 2,

        [EnumMember(Value = "Undefined")]
        Undefined = 3
    }
}
