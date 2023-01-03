
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Dynamic;
    using System.Runtime.Serialization;

    public class RelationshipElement : DataElement
    {
        [Required]
        [DataMember(Name = "first")]
        public ModelReference First { get; set; } = new();

        [Required]
        [DataMember(Name = "second")]
        public ModelReference Second { get; set; } = new();

        public RelationshipElement() { }

        public RelationshipElement(SubmodelElement src)
            : base(src)
        {
            if (!(src is RelationshipElement rel))
                return;

            if (rel.First != null)
                First = new ModelReference(rel.First);

            if (rel.Second != null)
                Second = new ModelReference(rel.Second);
        }
    }
}
