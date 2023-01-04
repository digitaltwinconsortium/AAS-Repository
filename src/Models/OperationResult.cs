
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public partial class OperationResult
    {
        [DataMember(Name="executionResult")]
        public Result ExecutionResult { get; set; }

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
