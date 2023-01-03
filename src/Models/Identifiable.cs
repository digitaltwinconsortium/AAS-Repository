
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Text.RegularExpressions;

    [DataContract]
    public class Identifiable : Referable
    {
        // V3.0 made id a simple string

        [DataMember(Name = "administration")]
        public AdministrativeInformation Administration { get; set; }

        [Required]
        [DataMember(Name = "identification")]
        public string Identification { get; set; }

        public Identifier id = new Identifier();

        public Identifiable() : base() { }

        public Identifiable(Identifiable src)
            : base(src)
        {
            if (src == null)
                return;

            if (src.id != null)
                id = new Identifier(src.id);

            if (src.Administration != null)
                Administration = new AdministrativeInformation(src.Administration);
        }

        public string GetFriendlyName()
        {
            if (id != null && id.value != "")
                return Regex.Replace(id.value, @"[^a-zA-Z0-9\-_]", "_");

            return Regex.Replace(IdShort, @"[^a-zA-Z0-9\-_]", "_");
        }
    }
}
