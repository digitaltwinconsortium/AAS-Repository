
namespace AdminShell
{
    using System.Runtime.Serialization;

    [DataContract]
    public class MultiLanguageProperty : DataElement
    {
        [DataMember(Name = "Value")]
        public LangStringSet Value { get; set; } = new();

        [DataMember(Name = "valueId")]
        public GlobalReference ValueId { get; set; }

        public MultiLanguageProperty() { }

        public MultiLanguageProperty(SubmodelElement src)
            : base(src)
        {
            if (!(src is MultiLanguageProperty mlp))
                return;

            Value = new LangStringSet(mlp.Value);
            if (mlp.ValueId != null)
                ValueId = new GlobalReference(mlp.ValueId);
        }

        public MultiLanguageProperty Set(LangString ls)
        {
            if (ls == null)
                return this;

            if (Value?.LangString == null)
                Value = new LangStringSet();

            Value.LangString[0] = ls;
            return this;
        }
    }
}
