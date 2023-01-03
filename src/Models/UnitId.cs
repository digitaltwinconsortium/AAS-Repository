
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public class UnitId
    {
        [XmlIgnore]
        public KeyList keys = new KeyList();


        [XmlArray("keys")]
        [XmlArrayItem("key")]
        public List<Key> Keys { get { return keys?.Keys; } }

        [XmlIgnore]
        public bool IsEmpty { get { return keys == null || keys.IsEmpty; } }

        [XmlIgnore]
        public int Count { get { if (keys == null) return 0; return keys.Count; } }

        [XmlIgnore]
        public Key this[int index] { get { return keys.Keys[index]; } }

        public UnitId() { }

        public UnitId(UnitId src)
        {
            if (src.keys != null)
                foreach (var k in src.Keys)
                    this.keys.Add(new Key(k));
        }

        public static UnitId CreateNew(string type, bool local, string idType, string value)
        {
            var u = new UnitId();
            u.keys.Keys.Add(Key.CreateNew(type, local, idType, value));
            return u;
        }

        public static UnitId CreateNew(Reference src)
        {
            var res = new UnitId();
            if (src != null && src.Keys != null)
                foreach (var k in src.Keys)
                    res.keys.Add(k);
            return res;
        }
    }
}
