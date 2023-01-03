
namespace AdminShell
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ReferenceElement : DataElement
    {
        [DataMember(Name = "Value")]
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
    }
}
