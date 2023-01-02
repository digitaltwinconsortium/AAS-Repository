
namespace AdminShell
{
    public class Capability : SubmodelElement
    {
        public Capability() { }

        public Capability(SubmodelElement src)
            : base(src)
        { }

        public AasElementSelfDescription GetSelfDescription()
        {
            return new AasElementSelfDescription("Capability", "Cap", SubmodelElementWrapper.AdequateElementEnum.Capability);
        }
    }
}
