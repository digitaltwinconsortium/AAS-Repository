
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class Identifiable : Referable
    {
        [DataMember(Name = "administration")]
        [XmlElement(ElementName = "administration")]
        public AdministrativeInformation Administration { get; set; }

        [JsonIgnore]
        [XmlElement(ElementName = "identification")]
        public Identifier Identification { get; set; }

        [XmlIgnore]
        [DataMember(Name = "id")]
        public string Id
        {
            get
            {
                return Identification.Id;
            }
            set
            {
                Identification.Id = value;
            }
        }
    }
}
