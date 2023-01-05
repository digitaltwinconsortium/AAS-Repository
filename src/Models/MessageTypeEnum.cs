
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Runtime.Serialization;

    [DataContract]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum MessageTypeEnum
    {
        [EnumMember(Value = "Undefined")]
        Undefined = 0,

        [EnumMember(Value = "Info")]
        Info = 1,

        [EnumMember(Value = "Warning")]
        Warning = 2,

        [EnumMember(Value = "Error")]
        Error = 3,

        [EnumMember(Value = "Exception")]
        Exception = 4
    }
}
