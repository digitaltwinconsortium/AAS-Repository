
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class SubmodelElementList : SubmodelElement
    {
        [DataMember(Name="semanticIdValues")]
        public Reference SemanticIdValues { get; set; }

        [DataMember(Name="submodelElementTypeValues")]
        public ModelType SubmodelElementTypeValues { get; set; }

        [DataMember(Name="Value")]
        public List<SubmodelElement> Value { get; set; }

        [DataMember(Name="valueTypeValues")]
        public ValueTypeEnum ValueTypeValues { get; set; }
    }
}
