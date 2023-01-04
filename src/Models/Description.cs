
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public class Description
    {
        [XmlElement(ElementName = "langString")]
        public List<LangString> LangString { get; set; } = new();

        public Description() { }

        public Description(Description src)
        {
            if (src != null && src.LangString != null)
                foreach (var ls in src.LangString)
                    LangString.Add(new LangString(ls));
        }
    }
}
