
namespace AdminShell
{
    public class LocatedReference
    {
        public Identifiable Identifiable;

        public ModelReference Reference;

        public LocatedReference() { }

        public LocatedReference(Identifiable identifiable, ModelReference reference)
        {
            Identifiable = identifiable;
            Reference = reference;
        }
    }
}
