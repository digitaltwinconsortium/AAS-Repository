
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Runtime.Serialization;

    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum MessageTypeEnum
    {
        [EnumMember(Value = "Undefined")]
        UndefinedEnum = 0,

        [EnumMember(Value = "Info")]
        InfoEnum = 1,

        [EnumMember(Value = "Warning")]
        WarningEnum = 2,

        [EnumMember(Value = "Error")]
        ErrorEnum = 3,

        [EnumMember(Value = "Exception")]
        ExceptionEnum = 4
    }
}
