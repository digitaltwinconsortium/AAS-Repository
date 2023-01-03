
namespace AdminShell
{
    using System.Runtime.Serialization;

    [DataContract]
    public class MultiLanguageProperty : DataElement
    {
        [DataMember(Name = "value")]
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

        public AasElementSelfDescription GetSelfDescription()
        {
            return new AasElementSelfDescription("MultiLanguageProperty", "MLP",
                SubmodelElementWrapper.AdequateElementEnum.MultiLanguageProperty);
        }

        public MultiLanguageProperty Set(LangString ls)
        {
            if (ls == null)
                return this;

            if (Value?.LangString == null)
                Value = new LangStringSet();

            Value.LangString[ls.Text] = ls.Text;
            return this;
        }

        public MultiLanguageProperty Set(string lang, string str)
        {
            return Set(new LangString(lang, str));
        }
    }
}
