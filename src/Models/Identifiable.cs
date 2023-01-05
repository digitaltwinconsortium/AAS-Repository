
namespace AdminShell
{
    using System.Runtime.Serialization;
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;

    [DataContract]
    public class Identifiable : Referable
    {
        // V3.0 made Id a simple string

        [DataMember(Name = "administration")]
        [XmlElement(ElementName = "administration")]
        public AdministrativeInformation Administration { get; set; }

        [DataMember(Name = "identification")]
        [XmlElement(ElementName = "identification")]
        public string Identification
        {
            get { return Id.Value; }
            set { Id = new Identifier(value); }
        }

        public Identifier Id { get; set; } = new();

        public Identifiable() : base() { }

        public Identifiable(Identifiable src)
            : base(src)
        {
            if (src == null)
                return;

            if (src.Id != null)
                Id = new Identifier(src.Id);

            if (src.Administration != null)
                Administration = new AdministrativeInformation(src.Administration);
        }

        public string GetFriendlyName()
        {
            if (Id != null && Id.Value != "")
                return Regex.Replace(Id.Value, @"[^a-zA-Z0-9\-_]", "_");

            return Regex.Replace(IdShort, @"[^a-zA-Z0-9\-_]", "_");
        }
    }
}
