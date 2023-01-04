
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Runtime.Serialization;

    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum ExecutionStateEnum
    {
        [EnumMember(Value = "Initiated")]
        InitiatedEnum = 0,

        [EnumMember(Value = "Running")]
        RunningEnum = 1,

        [EnumMember(Value = "Completed")]
        CompletedEnum = 2,

        [EnumMember(Value = "Canceled")]
        CanceledEnum = 3,

        [EnumMember(Value = "Failed")]
        FailedEnum = 4,

        [EnumMember(Value = "Timeout")]
        TimeoutEnum = 5
    }
}
