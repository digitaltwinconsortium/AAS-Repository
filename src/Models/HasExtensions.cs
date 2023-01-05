
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public partial class HasExtensions
    {
        [DataMember(Name="extensions")]
        [XmlElement(ElementName = "extensions")]
        public List<Extension> Extensions { get; set; }
    }
}
