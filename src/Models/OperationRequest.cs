
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class OperationRequest
    {
        [DataMember(Name="inoutputArguments")]
        [XmlElement(ElementName = "inoutputArguments")]
        public List<OperationVariable> InoutputArguments { get; set; }

        [DataMember(Name="inputArguments")]
        [XmlElement(ElementName = "inputArguments")]
        public List<OperationVariable> InputArguments { get; set; }

        [DataMember(Name="requestId")]
        [XmlElement(ElementName = "requestId")]
        public string RequestId { get; set; }

        [DataMember(Name="timeout")]
        [XmlElement(ElementName = "timeout")]
        public int? Timeout { get; set; }
    }
}
