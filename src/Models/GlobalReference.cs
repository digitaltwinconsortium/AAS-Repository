
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class GlobalReference : Reference
    {
        [Required]
        [XmlArray("values")]
        [XmlArrayItem("value")]
        [DataMember(Name = "value")]
        public List<Identifier> Value { get; set; } = new();

        public GlobalReference() : base() { }

        public GlobalReference(GlobalReference src) : base()
        {
            if (src == null)
                return;

            foreach (var id in src.Value)
                Value.Add(new Identifier(id));
        }

        public GlobalReference(Reference r) : base() { }

        public GlobalReference(Identifier id) : base()
        {
            Value.Add(id);
        }
    }
}
