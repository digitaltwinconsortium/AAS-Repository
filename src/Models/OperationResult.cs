
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class OperationResult
    {
        [DataMember(Name="executionResult")]
        public Result ExecutionResult { get; set; }

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

        [DataMember(Name="executionState")]
        public ExecutionStateEnum? ExecutionState { get; set; }

        [DataMember(Name="inoutputArguments")]
        public List<OperationVariable> InoutputArguments { get; set; }

        [DataMember(Name="outputArguments")]
        public List<OperationVariable> OutputArguments { get; set; }

        [DataMember(Name="requestId")]
        public string RequestId { get; set; }
    }
}
