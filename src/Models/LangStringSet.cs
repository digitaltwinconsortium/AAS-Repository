
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public class LangStringSet
    {
        [XmlElement(ElementName = "langString", Namespace = "http://www.admin-shell.io/aas/3/0")]
        public List<LangString> LangString = new();

        public LangStringSet() { }

        public LangStringSet(LangStringSet src)
        {
            if (src.LangString != null)
                foreach (var ls in src.LangString)
                    LangString.Add(new LangString(ls));
        }
    }
}
