
namespace AdminShell
{
    using System.Runtime.Serialization;

    [DataContract]
    public class SubmodelElementStruct : SubmodelElementCollection
    {
        [DataMember(Name = "Value")]
        public new SubmodelElement Value { get; set; }

        public SubmodelElementStruct() { }

        public SubmodelElementStruct(SubmodelElement src, bool shallowCopy = false)
            : base(src, shallowCopy)
        {
        }
    }
}
