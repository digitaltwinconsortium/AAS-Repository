
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class PackageDescription
    {
        [DataMember(Name="aasIds")]
        [XmlArray(ElementName = "aasIds")]
        public List<string> AasIds { get; set; }

        [DataMember(Name="packageId")]
        [XmlElement(ElementName = "packageId")]
        public string PackageId { get; set; }
    }
}
