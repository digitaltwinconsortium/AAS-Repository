
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "langString")]
    public class LangString
    {
        // constants
        public static string LANG_DEFAULT = "en";

        [Required]
        [DataMember(Name="lang")]
        [XmlAttribute(AttributeName="lang")]
        [MetaModelName("LangString.Language")]
        public string Language { get; set; }

        [Required]
        [DataMember(Name="text")]
        [XmlText]
        [MetaModelName("LangString.Text")]
        public string Text { get; set; }

        public LangString() { }

        public LangString(LangString src)
        {
            Language = src.Language;
            Text = src.Text;
        }
    }
}

