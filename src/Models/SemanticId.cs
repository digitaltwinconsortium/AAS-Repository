
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public class SemanticId : Reference
    {
        [XmlIgnore]
        private KeyList keys = new KeyList();

        [XmlArray("keys")]
        [XmlArrayItem("key")]
        public List<Key> Keys { get { return keys?.Keys; } }

        [XmlIgnore]
        public new bool IsEmpty { get { return keys == null || keys.IsEmpty; } }

        [XmlIgnore]
        public new int Count { get { if (keys == null) return 0; return keys.Count; } }

        [XmlIgnore]
        public new Key this[int index] { get { return keys[index]; } }

        public override string ToString()
        {
            return Key.KeyListToString(keys.Keys);
        }

        public SemanticId(){  }

        public SemanticId(SemanticId src)
        {
            if (src != null)
                foreach (var k in src.Keys)
                    keys.Add(k);
        }

        public static SemanticId CreateFromKey(Key key)
        {
            if (key == null)
                return null;
            var res = new SemanticId();
            res.Keys.Add(key);
            return res;
        }

        public static SemanticId CreateFromKeys(List<Key> keys)
        {
            if (keys == null)
                return null;
            var res = new SemanticId();
            res.Keys.AddRange(keys);
            return res;
        }

        public bool Matches(string type, bool local, string idType, string value)
        {
            if (this.Count == 1
                && this.keys[0].Type.ToLower().Trim() == type.ToLower().Trim()
                && this.keys[0].local == local
                && this.keys[0].idType.ToLower().Trim() == idType.ToLower().Trim()
                && this.keys[0].Value.ToLower().Trim() == value.ToLower().Trim())
                return true;
            return false;
        }
    }
}
