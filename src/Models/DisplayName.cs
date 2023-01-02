
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public class DisplayName
    {
        [XmlElement(ElementName = "LangString")]
        public List<LangStr> langString = new();

        public DisplayName() { }

        public DisplayName(Description src)
        {
            if (src != null && src.langString != null)
                foreach (var ls in src.langString)
                    langString.Add(new LangStr(ls));
        }

        public DisplayName(LangStringSet src)
        {
            if (src != null && src.LangString != null)
                foreach (var ls in src.LangString)
                    langString.Add(new LangStr(ls));
        }

        [XmlIgnore]
        public bool IsValid { get { return langString != null && langString.Count >= 1; } }

        // single string representation
        public string GetDefaultStr(string defaultLang = null)
        {
            return langString?.GetDefaultStr(defaultLang);
        }
    }
}
