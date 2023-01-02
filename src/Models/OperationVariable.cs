
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace AdminShell
{
    [DataContract]
    public class OperationVariable
    {
        [Required]
        [DataMember(Name="Value")]
        public OneOfOperationVariableValue Value { get; set; }
    }
}
