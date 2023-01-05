
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Runtime.Serialization;

    [DataContract]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum AssetKind
    {
        [EnumMember(Value = "Type")]
        Type = 0,

        [EnumMember(Value = "Instance")]
        Instance = 1
    }
}
