
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Runtime.Serialization;

    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum LevelType
    {
        [EnumMember(Value = "Min")]
        MinEnum = 0,

        [EnumMember(Value = "Max")]
        MaxEnum = 1,

        [EnumMember(Value = "Nom")]
        NomEnum = 2,

        [EnumMember(Value = "Typ")]
        TypEnum = 3
    }
}
