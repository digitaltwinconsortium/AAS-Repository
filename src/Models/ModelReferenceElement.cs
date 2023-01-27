
namespace AdminShell
{
    public class ModelReferenceElement : ReferenceElement
    {
        public ModelReferenceElement()
        {
            ModelType.Name = ModelTypes.ModelReferenceElement;
        }

        public ModelReferenceElement(SubmodelElement src)
            : base(src)
        {
            if (!(src is ModelReferenceElement mre))
            {
                return;
            }

            ModelType.Name = ModelTypes.ModelReferenceElement;

            if (mre.Value != null)
            {
                Value = new Reference(mre.Value);
            }
        }
    }
}
