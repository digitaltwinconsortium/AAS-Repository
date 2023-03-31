
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class OperationVariable : IAasElement
    {
        public enum Direction
        {
            In,
            Out,
            InOut
        };

        [Required]
        [DataMember(Name = "value")]
        [XmlElement(ElementName = "value")]
        public SubmodelElementWrapper Value { get; set; }

        public OperationVariable()
        {
        }

        public OperationVariable(OperationVariable src)
        {
            Value = new SubmodelElementWrapper(src?.Value?.SubmodelElement);
        }
    }
}
