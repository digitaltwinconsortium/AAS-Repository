
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class PackagesBody
    {
        [DataMember(Name="aasIds")]
        [XmlElement(ElementName = "aasIds")]
        public List<string> AasIds { get; set; }

        [DataMember(Name="file")]
        [XmlElement(ElementName = "file")]
        public byte[] File { get; set; }

        [DataMember(Name="fileName")]
        [XmlElement(ElementName = "fileName")]
        public string FileName { get; set; }
    }
}
