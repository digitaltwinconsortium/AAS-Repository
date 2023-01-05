
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Runtime.Serialization;

    [DataContract]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum LevelType
    {
        [EnumMember(Value = "Min")]
        Min = 0,

        [EnumMember(Value = "Max")]
        Max = 1,

        [EnumMember(Value = "Nom")]
        Nom = 2,

        [EnumMember(Value = "Typ")]
        Typ = 3
    }
}
