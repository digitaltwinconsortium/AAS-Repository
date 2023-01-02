
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class SubmodelElement : Referable
    {
        [DataMember(Name="embeddedDataSpecifications")]
        public List<EmbeddedDataSpecification> EmbeddedDataSpecifications { get; set; }

        [DataMember(Name="semanticId")]
        public Reference SemanticId { get; set; }

        [DataMember(Name="qualifiers")]
        public List<Constraint> Qualifiers { get; set; }

        [DataMember(Name="kind")]
        public ModelingKind Kind { get; set; }
    }
}
