
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public class LangStringSetIEC61360 : List<LangString>
    {
        [XmlIgnore]
        public bool IsEmpty { get { return Count < 1; } }

        public LangStringSetIEC61360() { }

        public LangStringSetIEC61360(List<LangString> lol) : base(lol) { }

        public LangStringSetIEC61360(LangStringSetIEC61360 src)
        {
            if (src == null)
                return;

            foreach (var ls in src)
                Add(new LangString(ls));
        }

        public LangStringSetIEC61360(string lang, string str)
        {
            if (str == null || str.Trim() == "")
                return;

            Add(new LangString(lang, str));
        }
    }
}
