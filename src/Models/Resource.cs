
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace AdminShell
{
    [DataContract]
    public class Resource
    {
        [DataMember(Name ="path")]
        [XmlElement(ElementName = "path")]
        [MetaModelName("Resource.path")]
        public string Path = string.Empty;

        [DataMember(Name = "contentType")]
        [XmlElement(ElementName = "contentType")]
        [MetaModelName("Resource.contentType")]
        public string ContentType = string.Empty;
    }
}
