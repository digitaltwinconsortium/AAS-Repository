
namespace AdminShell
{
    using System.Runtime.Serialization;

    [DataContract]
    public partial class HasSemantics
    {
        [DataMember(Name="semanticId")]
        public Reference SemanticId { get; set; }
    }
}
