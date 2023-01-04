
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;

    public class KeyList : List<Key>
    {
        [XmlIgnore]
        public bool IsEmpty { get { return this.Count < 1; } }

        public KeyList() { }

        public string ToString(int format = 0, string delimiter = ",")
        {
            return string.Join(delimiter, this.Select((x) => x.ToString(format)));
        }
    }
}

