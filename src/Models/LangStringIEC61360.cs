
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml;
    using System.Xml.Serialization;

    [DataContract]
    public class LangStringIEC61360
    {
        [DataMember(Name = "langString")]
        [XmlElement(ElementName = "langString")]
        public List<LangString> LangString = new List<LangString>();

        [XmlIgnore]
        public bool IsEmpty { get { return LangString == null || LangString.Count < 1; } }

        [XmlIgnore]
        public int Count { get { if (LangString == null) return 0; return LangString.Count; } }

        [XmlIgnore]
        public LangString this[int index] { get { return LangString[index]; } }

        public LangStringIEC61360() { }

        public LangStringIEC61360(LangStringIEC61360 src)
        {
            if (src.LangString != null)
                foreach (var ls in src.LangString)
                    this.LangString.Add(new LangString(ls));
        }
    }
}
