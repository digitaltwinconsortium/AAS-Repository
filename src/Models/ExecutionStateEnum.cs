
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum ExecutionStateEnum
    {
        [EnumMember(Value = "Initiated")]
        [XmlEnum(Name = "Initiated")]
        Initiated = 0,

        [EnumMember(Value = "Running")]
        [XmlEnum(Name = "Running")]
        Running = 1,

        [EnumMember(Value = "Completed")]
        [XmlEnum(Name = "Completed")]
        Completed = 2,

        [EnumMember(Value = "Canceled")]
        [XmlEnum(Name = "Canceled")]
        Canceled = 3,

        [EnumMember(Value = "Failed")]
        [XmlEnum(Name = "Failed")]
        Failed = 4,

        [EnumMember(Value = "Timeout")]
        [XmlEnum(Name = "Timeout")]
        Timeout = 5
    }
}
