
namespace AdminShell
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ReferenceElement : SubmodelElement
    {
        [DataMember(Name="Value")]
        public Reference Value { get; set; }
    }
}
