
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class ReferenceElement : DataElement
    {
        [DataMember(Name = "value")]
        public Reference Value { get; set; } = new();

        public ReferenceElement() { }

        public ReferenceElement(SubmodelElement src)
            : base(src)
        {
            if (!(src is ReferenceElement re))
                return;

            if (re.Value != null)
                Value = new Reference(re.Value);
        }

        public static ReferenceElement CreateNew(
            string idShort = null, string category = null, Key semanticIdKey = null)
        {
            var x = new ReferenceElement();
            x.CreateNewLogic(idShort, category, semanticIdKey);
            return (x);
        }

        public void Set(Reference value = null)
        {
            Value = value;
        }

        public AasElementSelfDescription GetSelfDescription()
        {
            return new AasElementSelfDescription("ReferenceElement", "Ref");
        }

        public object ToValueOnlySerialization()
        {
            var output = new Dictionary<string, List<Dictionary<string, string>>>();

            var list = new List<Dictionary<string, string>>();
            foreach (var key in Value.Keys)
            {
                var valueDict = new Dictionary<string, string>
                {
                    { "Type", key.Type },
                    { "Value", key.Value }
                };
                list.Add(valueDict);
            }

            output.Add(IdShort, list);
            return output;
        }
    }
}
