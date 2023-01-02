
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class Operation : SubmodelElement
    {
        [DataMember(Name="inoutputVariable")]
        public List<OperationVariable> InoutputVariable { get; set; }

        [DataMember(Name="inputVariable")]
        public List<OperationVariable> InputVariable { get; set; }

        [DataMember(Name="outputVariable")]
        public List<OperationVariable> OutputVariable { get; set; }
    }
}
