
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class Blob : DataElement
    {
        [Required]
        [DataMember(Name = "mimeType")]
        [XmlElement(ElementName = "mimeType")]
        [MetaModelName("Blob.MimeType")]
        public string MimeType { get; set; } = string.Empty;

        [DataMember(Name = "value")]
        [XmlElement(ElementName = "value")]
        [MetaModelName("Blob.Value")]
        public string Value { get; set; } = string.Empty;

        public Blob()
        {
            ModelType.Name = ModelTypes.Blob;
        }

        public Blob(SubmodelElement src)
            : base(src)
        {
            if (!(src is Blob blb))
            {
                return;
            }

            MimeType = blb.MimeType;
            Value = blb.Value;
            ModelType.Name = ModelTypes.Blob;
        }
    }
}
