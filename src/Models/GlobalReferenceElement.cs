
namespace AdminShell
{
    using System.Collections.Generic;

    public class GlobalReferenceElement : ReferenceElement
    {
        public GlobalReference value = new GlobalReference();

        public GlobalReferenceElement() { }

        public GlobalReferenceElement(SubmodelElement src)
            : base(src)
        {
            if (!(src is GlobalReferenceElement gre))
                return;

            if (gre.value != null)
                this.value = new GlobalReference(gre.value);
        }

        public static GlobalReferenceElement CreateNew(
            string idShort = null, string category = null, Identifier semanticIdKey = null)
        {
            var x = new GlobalReferenceElement();
            x.CreateNewLogic(idShort, category, semanticIdKey);
            return (x);
        }

        public void Set(GlobalReference value = null)
        {
            this.value = value;
        }

        public AasElementSelfDescription GetSelfDescription()
        {
            return new AasElementSelfDescription("GlobalReferenceElement", "RefG",
                SubmodelElementWrapper.AdequateElementEnum.GlobalReferenceElement);
        }

        public object ToValueOnlySerialization()
        {
            var output = new Dictionary<string, List<string>>();

            var list = new List<string>();
            foreach (var refVal in this.value.Value)
            {
                list.Add(refVal.value);
            }

            output.Add(idShort, list);
            return output;
        }
    }
}

