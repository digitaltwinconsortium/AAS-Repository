
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public partial class OperationResult
    {
        [DataMember(Name="executionResult")]
        [XmlElement(ElementName = "executionResult")]
        public Result ExecutionResult { get; set; }

        [DataMember(Name="executionState")]
        [XmlElement(ElementName = "executionState")]
        public ExecutionStateEnum? ExecutionState { get; set; }

        [DataMember(Name="inoutputArguments")]
        [XmlElement(ElementName = "inoutputArguments")]
        public List<OperationVariable> InoutputArguments { get; set; }

        [DataMember(Name="outputArguments")]
        [XmlElement(ElementName = "outputArguments")]
        public List<OperationVariable> OutputArguments { get; set; }

        [DataMember(Name="requestId")]
        [XmlElement(ElementName = "requestId")]
        public string RequestId { get; set; }
    }
}
