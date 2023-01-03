
namespace AdminShell
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class Identifiable : Referable
    {
        // this class is complex, because V3.0 made id a simple string and this must be serialized

        [DataMember(Name = "administration")]
        public AdministrativeInformation Administration { get; set; } = new();

        [Required]
        [DataMember(Name = "identification")]
        public string Identification { get; set; }

        public Identifier id = new Identifier();


       public Identifiable() : base() { }

        public Identifiable(string idShort) : base(idShort) { }

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

        public void SetIdentification(string id, string idShort = null)
        {
            this.id.value = id;

            if (idShort != null)
                IdShort = idShort;
        }

        public void SetAdminstration(string version, string revision)
        {
            Administration.Version = version;
            Administration.Revision = revision;
        }

        public string GetFriendlyName()
        {
            if (id != null && id.value != "")
                return AdminShellUtil.FilterFriendlyName(this.id.value);
            return AdminShellUtil.FilterFriendlyName(IdShort);
        }

        public override string ToString()
        {
            return ("" + id?.ToString() + " " + administration?.ToString()).Trim();
        }

        public Key ToKey()
        {
            return new Key(GetElementName(), "" + id?.value);
        }

        public ModelReference GetModelReference(bool includeParents = true)
        {
            var r = new ModelReference();

            if (this is IGetSemanticId igs)
                r.referredSemanticId = igs.GetSemanticId();

            r.Keys.Add(
                Key.CreateNew(this.GetElementName(), this.id.value));

            return r;
        }
    }
}
