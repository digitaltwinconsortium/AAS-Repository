
namespace AdminShell
{
    using System.Runtime.Serialization;

    [DataContract]
    public class SubmodelElementStruct : SubmodelElement
    {
        [DataMember(Name="Value")]
        public SubmodelElement Value { get; set; }
    }
}
