
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Runtime.Serialization;

    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum AssetKind
    {
        [EnumMember(Value = "Type")]
        TypeEnum = 0,

        [EnumMember(Value = "Instance")]
        InstanceEnum = 1
    }
}
