
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Runtime.Serialization;

    [DataContract]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum ExecutionStateEnum
    {
        [EnumMember(Value = "Initiated")]
        Initiated = 0,

        [EnumMember(Value = "Running")]
        Running = 1,

        [EnumMember(Value = "Completed")]
        Completed = 2,

        [EnumMember(Value = "Canceled")]
        Canceled = 3,

        [EnumMember(Value = "Failed")]
        Failed = 4,

        [EnumMember(Value = "Timeout")]
        Timeout = 5
    }
}
