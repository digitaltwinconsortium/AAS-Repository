
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Text.RegularExpressions;

    [DataContract]
    public class Identifiable : Referable
    {
        // V3.0 made Id a simple string

        [DataMember(Name = "administration")]
        public AdministrativeInformation Administration { get; set; }

        [Required]
        [DataMember(Name = "identification")]
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
