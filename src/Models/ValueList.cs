
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class ValueList
    {
        [Required]
        [DataMember(Name="valueReferencePairTypes")]
        [XmlElement(ElementName = "valueReferencePairTypes")]
        public List<ValueObject> ValueReferencePairTypes { get; set; }
    }
}
