
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "LangString", Namespace = "http://www.admin-shell.io/3/0")]
    public class LangString
    {
        // constants
        public static string LANG_DEFAULT = "en";

        [Required]
        [DataMember(Name = "language")]
        [XmlAttribute(Namespace = "http://www.admin-shell.io/3/0")]
        [MetaModelName("LangString.lang")]
        public string Language { get; set; }

        [Required]
        [DataMember(Name = "text")]
        [XmlText]
        [MetaModelName("LangString.text")]
        public string Text { get; set; }

        public LangString() { }

        public LangString(LangString src)
        {
            Language = src.Language;
            Text = src.Text;
        }

        public LangString(string lang, string str)
        {
            Language = lang;
            Text = str;
        }

        public static List<LangString> CreateManyFromStringArray(string[] s)
        {
            var r = new List<LangString>();
            var i = 0;
            while ((i + 1) < s.Length)
            {
                r.Add(new LangString(s[i], s[i + 1]));
                i += 2;
            }
            return r;
        }
    }
}

