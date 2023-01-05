
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class File : DataElement
    {
        [Required]
        [DataMember(Name = "mimeType")]
        [XmlElement(ElementName = "mimeType")]
        [MetaModelName("File.mimeType")]
        public string MimeType { get; set; }

        [DataMember(Name = "value")]
        [XmlElement(ElementName = "value")]
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

