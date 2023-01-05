
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class Descriptor
    {
        [DataMember(Name="endpoints")]
        [XmlArray(ElementName = "endpoints")]
        public List<Endpoint> Endpoints { get; set; }
    }
}
