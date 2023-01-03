
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class OperationRequest
    {
        [DataMember(Name="inoutputArguments")]
        public List<OperationVariable> InoutputArguments { get; set; }

        [DataMember(Name="inputArguments")]
        public List<OperationVariable> InputArguments { get; set; }

        [DataMember(Name="requestId")]
        public string RequestId { get; set; }

        [DataMember(Name="timeout")]
        public int? Timeout { get; set; }
    }
}
