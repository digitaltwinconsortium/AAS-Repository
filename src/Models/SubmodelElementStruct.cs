
namespace AdminShell
{
    using System.Runtime.Serialization;

    [DataContract]
    public class SubmodelElementStruct : SubmodelElementCollection
    {
        public SubmodelElementStruct()
        {
            ModelType.Name = ModelTypes.SubmodelElementStruct;
        }

        public SubmodelElementStruct(SubmodelElement src, bool shallowCopy = false)
            : base(src, shallowCopy)
        {
            ModelType.Name = ModelTypes.SubmodelElementStruct;
        }
    }
}
