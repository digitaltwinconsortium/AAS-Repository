
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "langString", Namespace = "http://www.admin-shell.io/3/0")]
    public class LangString
    {
        // constants
        public static string LANG_DEFAULT = "en";

        [Required]
        [DataMember(Name = "language")]
        [XmlAttribute(Namespace = "http://www.admin-shell.io/3/0")]
        [MetaModelName("LangString.Language")]
        public string Language { get; set; }

        [Required]
        [DataMember(Name = "text")]
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

