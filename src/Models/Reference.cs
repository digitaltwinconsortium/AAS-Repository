
namespace AdminShell
{
    using System.Xml.Serialization;

    [XmlType(TypeName = "reference")]
    public class Reference : IAasElement
    {
        [XmlIgnore]
        protected KeyList keys = new KeyList();

        [XmlArray("keys")]
        [XmlArrayItem("key")]
        public KeyList Keys { get { return keys; } }

        [XmlIgnore]
        public int Count { get { if (keys == null) return 0; return keys.Count; } }

        public Reference(){ }

        public Reference(Reference src)
        {
            if (src != null)
                foreach (var k in src.Keys)
                    keys.Add(new Key(k));
        }

        public Key GetAsExactlyOneKey()
        {
            if (keys == null || keys.Count != 1)
                return null;

            var k = keys[0];

            return new Key(k.Type.ToString(), k.Value);
        }

        public bool Matches(Identifier other)
        {
            if (other == null)
                return false;

            if (Count == 1)
            {
                var k = keys[0];
                return k.Matches(other.IdType, other.Id, Key.MatchMode.Identification);
            }
            return false;
        }

        public bool Matches(Reference other, Key.MatchMode matchMode = Key.MatchMode.Strict)
        {
            if (keys == null || other == null || other.keys == null || other.Count != Count)
                return false;

            var same = true;
            for (int i = 0; i < Count; i++)
                same = same && keys[i].Matches(other.keys[i], matchMode);

            return same;
        }

        public string ToString(int format = 0, string delimiter = ",")
        {
            return keys?.ToString(format, delimiter);
        }
    }
}
