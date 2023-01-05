
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class ModelType
    {
        [Required]
        [DataMember(Name="name")]
        [XmlElement(ElementName = "name")]
        public ModelTypes Name { get; set; }
    }
}
