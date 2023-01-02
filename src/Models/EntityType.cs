
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Runtime.Serialization;

    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum EntityType
    {
        [EnumMember(Value = "CoManagedEntity")]
        CoManagedEntityEnum = 0,

        [EnumMember(Value = "SelfManagedEntity")]
        SelfManagedEntityEnum = 1,

        [EnumMember(Value = "Undefined")]
        UndefinedEnum = 3
    }
}
