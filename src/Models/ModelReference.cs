
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

            if (src.ReferredSemanticId != null)
                ReferredSemanticId = new GlobalReference(src.ReferredSemanticId);

            foreach (var k in src.Keys)
                keys.Add(new Key(k));
        }
    }
}
