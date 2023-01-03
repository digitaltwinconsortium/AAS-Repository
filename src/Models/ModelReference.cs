
namespace AdminShell
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ModelReference : Reference
    {
        [DataMember(Name = "referredSemanticId")]
        public GlobalReference ReferredSemanticId { get; set; }

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

        public bool MatchesExactlyOneKey(
            string type, string id, Key.MatchMode matchMode = Key.MatchMode.Relaxed)
        {
            if (keys == null || keys.Count != 1)
                return false;

            var k = keys[0];

            return k.Matches(type, id, matchMode);
        }
    }
}
