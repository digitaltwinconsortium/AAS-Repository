
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public class Description
    {
        [XmlElement(ElementName = "LangString")]
        public List<LangString> langString = new();

        public Description() { }

        public Description(Description src)
        {
            if (src != null && src.langString != null)
                foreach (var ls in src.langString)
                    langString.Add(new LangString(ls));
        }
    }
}
