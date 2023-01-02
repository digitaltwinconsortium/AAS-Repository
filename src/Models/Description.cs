
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public class Description
    {
        [XmlElement(ElementName = "LangString")]
        public List<LangStr> langString = new();

        public Description() { }

        public Description(Description src)
        {
            if (src != null && src.langString != null)
                foreach (var ls in src.langString)
                    langString.Add(new LangStr(ls));
        }
 
        [XmlIgnore]
        public bool IsValid { get { return langString != null && langString.Count >= 1; } }

        public string GetDefaultStr(string defaultLang = null)
        {
            return this.langString?.GetDefaultStr(defaultLang);
        }
    }
}
