
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class ModelReference : Reference
    {
        [DataMember(Name = "referredSemanticId")]
        public Reference ReferredSemanticId { get; set; }

        [Required]
        [DataMember(Name = "keys")]
        public List<Key> Keys { get; set; }

        public GlobalReference referredSemanticId = null;

        [XmlIgnore] // anyway, as it is private/ protected
        protected KeyList keys = new KeyList();


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
        public int Count { get { if (keys == null) return 0; return keys.Count; } }

        [XmlIgnore]
        public Key this[int index] { get { return keys[index]; } }

        [XmlIgnore]
        public Key First { get { return this.Count < 1 ? null : this.keys[0]; } }

        public ModelReference() { }

        public ModelReference(Key k)
        {
            if (k != null)
                keys.Add(k);
        }

        public ModelReference(ModelReference src)
        {
            if (src == null)
                return;

            if (src.referredSemanticId != null)
                referredSemanticId = new GlobalReference(src.referredSemanticId);

            foreach (var k in src.Keys)
                keys.Add(new Key(k));
        }

        public ModelReference(GlobalReference src)
        {
            if (src == null)
                return;

            foreach (var id in src.Value)
                keys.Add(new Key("", id));
        }

        public ModelReference(SemanticId src, string type = null)
        {
            if (type == null)
                type = Key.GlobalReference;
            if (src != null)
                foreach (var id in src.Value)
                    keys.Add(new Key(type, id));
        }

        public static ModelReference CreateNew(Key k)
        {
            if (k == null)
                return null;
            var r = new ModelReference();
            r.keys.Add(k);
            return r;
        }

        public static ModelReference CreateNew(List<Key> k)
        {
            if (k == null)
                return null;
            var r = new ModelReference();
            r.keys.AddRange(k);
            return r;
        }

        public bool MatchesExactlyOneKey(
            string type, string id, Key.MatchMode matchMode = Key.MatchMode.Relaxed)
        {
            if (keys == null || keys.Count != 1)
                return false;
            var k = keys[0];
            return k.Matches(type, id, matchMode);
        }

        public bool MatchesExactlyOneKey(Key key, Key.MatchMode matchMode = Key.MatchMode.Relaxed)
        {
            if (key == null)
                return false;
            return this.MatchesExactlyOneKey(key.Type, key.Value, matchMode);
        }

        public AasElementSelfDescription GetSelfDescription()
        {
            return new AasElementSelfDescription("ModelReference", "MRf");
        }

        public string GetElementName()
        {
            return this.GetSelfDescription()?.ElementName;
        }
    }
}

