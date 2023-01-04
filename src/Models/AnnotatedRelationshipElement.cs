
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class AnnotatedRelationshipElement : RelationshipElement
    {
        [DataMember(Name = "annotations")]
        public List<DataElement> Annotations { get; set; } = new();

        public AnnotatedRelationshipElement() { }

        public AnnotatedRelationshipElement(AnnotatedRelationshipElement src) : base(src)
        {
            Annotations = src.Annotations;
        }
    }
}
