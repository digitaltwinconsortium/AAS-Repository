
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    public class Blob : DataElement
    {
        [Required]
        [DataMember(Name = "mimeType")]
        [MetaModelName("Blob.mimeType")]
        public string MimeType { get; set; } = string.Empty;

        [DataMember(Name = "Value")]
        [MetaModelName("Blob.Value")]
        public string Value { get; set; } = string.Empty;

        public Blob() { }

        public Blob(SubmodelElement src)
            : base(src)
        {
            if (!(src is Blob blb))
                return;

            MimeType = blb.MimeType;
            Value = blb.Value;
        }
    }
}
