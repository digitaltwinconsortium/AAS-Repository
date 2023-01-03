
namespace AdminShell
{
    using System.Xml.Serialization;

    public class SemanticId : Reference
    {
        [XmlIgnore]
        public new int Count { get { if (keys == null) return 0; return keys.Count; } }

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
    }
}
