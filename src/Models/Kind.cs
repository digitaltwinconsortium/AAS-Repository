
namespace AdminShell
{
    using System.Xml;
    using System.Xml.Serialization;

    public class Kind
    {
        [XmlText]
        public string kind = "Instance";

        [XmlIgnore]
        public bool IsInstance { get { return kind == null || kind.Trim().ToLower() == "instance"; } }

        [XmlIgnore]
        public bool IsType { get { return kind != null && kind.Trim().ToLower() == "Type"; } }

        public Kind() { }

        public Kind(Kind src)
        {
            kind = src.kind;
        }

        public Kind(string kind)
        {
            this.kind = kind;
        }
    }
}
