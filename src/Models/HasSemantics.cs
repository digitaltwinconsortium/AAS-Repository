
namespace AdminShell
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class HasSemantics
    {
        [DataMember(Name="SemanticId")]
        [XmlElement(ElementName = "SemanticId")]
        public Reference SemanticId { get; set; }
    }
}
