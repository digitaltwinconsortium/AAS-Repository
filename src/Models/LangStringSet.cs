
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

        public LangStringSet(List<LangString> src)
        {
            if (src != null)
                foreach (var ls in src)
                    LangString.Add(new LangString(ls));
        }

        public LangStringSet(string lang, string str)
        {
            if (str == null || str.Trim() == "")
                return;

            LangString.Add(new LangString(lang, str));
        }

        public LangString Add(LangString ls)
        {
            LangString.Add(ls);
            return ls;
        }

        public LangString Add(string lang, string str)
        {
            var ls = new LangString(lang, str);
            LangString.Add(ls);
            return ls;
        }

        public string GetDefaultStr(string defaultLang = null)
        {
            return LangString?.GetDefaultStr(defaultLang);
        }
    }
}
