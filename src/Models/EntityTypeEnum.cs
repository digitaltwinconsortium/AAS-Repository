
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Runtime.Serialization;

    [DataContract]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum EntityTypeEnum
    {
        [EnumMember(Value = "CoManagedEntity")]
        CoManagedEntity = 0,

        [EnumMember(Value = "SelfManagedEntity")]
        SelfManagedEntity = 1,

        [EnumMember(Value = "Undefined")]
        Undefined = 3
    }
}
