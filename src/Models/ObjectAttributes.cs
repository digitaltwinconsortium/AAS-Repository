
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class ObjectAttributes
    {
        [DataMember(Name="objectAttribute")]
        [XmlElement(ElementName = "objectAttribute")]
        public List<Property> ObjectAttribute { get; set; }
    }
}
