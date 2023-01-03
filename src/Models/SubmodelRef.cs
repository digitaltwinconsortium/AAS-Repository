
namespace AdminShell
{
    using System.Xml.Serialization;

    [XmlType(TypeName = "submodelRef")]
    public class SubmodelRef : ModelReference
    {
        public SubmodelRef() : base() { }

        public SubmodelRef(SubmodelRef src) : base(src) { }

        public SubmodelRef(ModelReference src) : base(src) { }

        public static SubmodelRef CreateNew(string type, string value)
        {
            var r = new SubmodelRef();
            r.Keys.Add(Key.CreateNew(type, value));
            return r;
        }

        public static SubmodelRef CreateNew(ModelReference src)
        {
            if (src == null || src.Keys == null)
                return null;
            var r = new SubmodelRef();
            r.Keys.AddRange(src.Keys);
            return r;
        }

        public new AasElementSelfDescription GetSelfDescription()
        {
            return new AasElementSelfDescription("SubmodelRef", "SMRef");
        }
    }
}
