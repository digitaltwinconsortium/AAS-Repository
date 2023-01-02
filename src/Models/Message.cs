
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Runtime.Serialization;

    [DataContract]
    public class Message
    {
        [DataMember(Name="code")]
        public string Code { get; set; }

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
            ExceptionEnum = 4        }

        [DataMember(Name="messageType")]
        public MessageTypeEnum? MessageType { get; set; }

        [DataMember(Name="text")]
        public string Text { get; set; }

        [DataMember(Name="timestamp")]
        public string Timestamp { get; set; }
    }
}
