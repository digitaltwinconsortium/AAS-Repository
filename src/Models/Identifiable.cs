
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
        public Identifier Identification { get; set; } = new Identifier();

        [XmlIgnore]
        [DataMember(Name = "id")]
        public string Id
        {
            get
            {
                return Identification?.Id;
            }
            set
            {
                if (Identification != null)
                {
                    Identification.Id = value;
                }
            }
        }
    }
}
