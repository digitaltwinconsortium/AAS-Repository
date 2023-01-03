
namespace AdminShell
{
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
    }
}

