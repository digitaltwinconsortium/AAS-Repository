
namespace AdminShell
{
    public class SubmodelElementWrapperCollection : BaseSubmodelElementWrapperCollection<SubmodelElement>
    {
        public SubmodelElementWrapperCollection() : base() { }

        public SubmodelElementWrapperCollection(SubmodelElementWrapper smw) : base(smw) { }

        public SubmodelElementWrapperCollection(SubmodelElement sme) : base(sme) { }

        public SubmodelElementWrapperCollection(SubmodelElementWrapperCollection other) : base(other) { }

    }
}
