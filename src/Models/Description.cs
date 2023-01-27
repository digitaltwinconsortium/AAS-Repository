
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class Description
    {
        [DataMember(Name = "langString")]
        [XmlArray(ElementName = "langString")]
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
