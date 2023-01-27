
namespace AdminShell
{
    public class Capability : SubmodelElement
    {
        public Capability()
        {
            ModelType.Name = ModelTypes.Capability;
        }

        public Capability(SubmodelElement src)
            : base(src)
        {
            ModelType.Name = ModelTypes.Capability;
        }
    }
}
