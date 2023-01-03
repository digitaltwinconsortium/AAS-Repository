
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class File : DataElement
    {
        [Required]
        [DataMember(Name = "mimeType")]
        [MetaModelName("File.mimeType")]
        public string MimeType { get; set; }

        [DataMember(Name = "Value")]
        [MetaModelName("File.Value")]
        public string Value { get; set; }


        public File() { }

        public File(SubmodelElement src)
            : base(src)
        {
            if (!(src is File file))
                return;

            MimeType = file.MimeType;
            Value = file.Value;
        }
    }
}

