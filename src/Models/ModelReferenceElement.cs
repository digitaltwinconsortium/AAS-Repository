
namespace AdminShell
{
    public class ModelReferenceElement : ReferenceElement
    {
        public ModelReference value = new ModelReference();

        public ModelReferenceElement() { }

        public ModelReferenceElement(SubmodelElement src)
            : base(src)
        {
            if (!(src is ModelReferenceElement mre))
                return;

            if (mre.value != null)
                this.value = new ModelReference(mre.value);
        }

        public AasElementSelfDescription GetSelfDescription()
        {
            return new AasElementSelfDescription("ModelReferenceElement", "RefM", SubmodelElementWrapper.AdequateElementEnum.ModelReferenceElement);
        }
    }
}

