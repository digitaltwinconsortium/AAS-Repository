
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Serialization;

    public class LangStringIEC61360
    {
        [XmlElement(ElementName = "LangString", Namespace = "http://www.admin-shell.io/aas/1/0")]
        public List<LangString> langString = new List<LangString>();

        [XmlIgnore]
        public bool IsEmpty { get { return langString == null || langString.Count < 1; } }

        [XmlIgnore]
        public int Count { get { if (langString == null) return 0; return langString.Count; } }

        [XmlIgnore]
        public LangString this[int index] { get { return langString[index]; } }

        public LangStringIEC61360() { }

        public LangStringIEC61360(LangStringIEC61360 src)
        {
            if (src.langString != null)
                foreach (var ls in src.langString)
                    this.langString.Add(new LangString(ls));
        }

        public static LangStringIEC61360 CreateFrom(List<LangString> src)
        {
            var res = new LangStringIEC61360();
            if (src != null)
                foreach (var ls in src)
                    res.langString.Add(new LangString(ls));
            return res;
        }
    }
}
