
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlType(TypeName = "reference")]
    public class Reference : IAasElement
    {
        [XmlIgnore] // anyway, as it is private
        private KeyList keys = new KeyList();

        [XmlArray("keys")]
        [XmlArrayItem("key")]
        public KeyList Keys { get { return keys; } }

        [XmlIgnore]
        [JsonProperty(PropertyName = "keys")]
        public KeyList JsonKeys
        {
            get
            {
                keys?.NumberIndices();
                return keys;
            }
        }

        [XmlIgnore]
        public bool IsEmpty { get { return keys == null || keys.Count < 1; } }

        [XmlIgnore]
        public int Count { get { if (keys == null) return 0; return keys.Count; } }

        [XmlIgnore]
        public Key this[int index] { get { return keys[index]; } }

        [XmlIgnore]
        public Key First { get { return Count < 1 ? null : keys[0]; } }

        [XmlIgnore]
        public Key Last { get { return Count < 1 ? null : keys[keys.Count - 1]; } }

        public Reference(){ }

        public Reference(Key k)
        {
            if (k != null)
                keys.Add(k);
        }

        public Reference(Reference src)
        {
            if (src != null)
                foreach (var k in src.Keys)
                    keys.Add(new Key(k));
        }

        public Reference(SemanticId src)
        {
            if (src != null)
                foreach (var k in src.Keys)
                    keys.Add(new Key(k));
        }

        public static Reference CreateNew(Key k)
        {
            if (k == null)
                return null;
            var r = new Reference();
            r.keys.Add(k);
            return r;
        }

        public static Reference CreateNew(List<Key> k)
        {
            if (k == null)
                return null;
            var r = new Reference();
            r.keys.AddRange(k);
            return r;
        }

        public static Reference CreateNew(string type, bool local, string idType, string value)
        {
            if (type == null || idType == null || value == null)
                return null;
            var r = new Reference();
            r.keys.Add(Key.CreateNew(type, local, idType, value));
            return r;
        }

        public static Reference CreateIrdiReference(string irdi)
        {
            if (irdi == null)
                return null;
            var r = new Reference();
            r.keys.Add(new Key(Key.GlobalReference, false, Identification.IRDI, irdi));
            return r;
        }

        public static Reference operator +(Reference a, Key b)
        {
            var res = new Reference(a);
            res.Keys?.Add(b);
            return res;
        }

        public static Reference operator +(Reference a, Reference b)
        {
            var res = new Reference(a);
            res.Keys?.AddRange(b?.Keys);
            return res;
        }

        public Key GetAsExactlyOneKey()
        {
            if (keys == null || keys.Count != 1)
                return null;
            var k = keys[0];
            return new Key(k.Type, k.local, k.idType, k.Value);
        }

        public bool MatchesExactlyOneKey(
            string type, bool local, string idType, string id, Key.MatchMode matchMode = Key.MatchMode.Strict)
        {
            if (keys == null || keys.Count != 1)
                return false;
            var k = keys[0];
            return k.Matches(type, local, idType, id, matchMode);
        }

        public bool MatchesExactlyOneKey(Key key, Key.MatchMode matchMode = Key.MatchMode.Strict)
        {
            if (key == null)
                return false;
            return MatchesExactlyOneKey(key.Type, key.local, key.idType, key.Value, matchMode);
        }

        public bool Matches(
            string type, bool local, string idType, string id, Key.MatchMode matchMode = Key.MatchMode.Strict)
        {
            if (Count == 1)
            {
                var k = keys[0];
                return k.Matches(type, local, idType, id, matchMode);
            }
            return false;
        }

        public bool Matches(Key key, Key.MatchMode matchMode = Key.MatchMode.Strict)
        {
            if (Count == 1)
            {
                var k = keys[0];
                return k.Matches(key, matchMode);
            }
            return false;
        }

        public bool Matches(Identification other)
        {
            if (other == null)
                return false;
            if (Count == 1)
            {
                var k = keys[0];
                return k.Matches(Key.GlobalReference, false, other.idType, other.id, Key.MatchMode.Identification);
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

        public bool Matches(SemanticId other, Key.MatchMode matchMode = Key.MatchMode.Strict)
        {
            return Matches(new Reference(other), matchMode);
        }

        public bool Matches(ConceptDescription cd, Key.MatchMode matchMode = Key.MatchMode.Strict)
        {
            return Matches(cd?.GetReference(), matchMode);
        }

        public string ToString(int format = 0, string delimiter = ",")
        {
            return keys?.ToString(format, delimiter);
        }

        public string ListOfValues(string delim)
        {
            string res = "";
            if (Keys != null)
                foreach (var x in Keys)
                {
                    if (x == null)
                        continue;
                    if (res != "") res += delim;
                    res += x.Value;
                }
            return res;
        }

        public virtual AasElementSelfDescription GetSelfDescription()
        {
            return new AasElementSelfDescription("Reference", "Rfc");
        }

        public virtual string GetElementName()
        {
            return GetSelfDescription()?.ElementName;
        }
    }
}
