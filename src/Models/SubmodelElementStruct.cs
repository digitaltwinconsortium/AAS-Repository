
namespace AdminShell
{
    using System.Runtime.Serialization;

    [DataContract]
    public class SubmodelElementStruct : SubmodelElementCollection
    {
        public SubmodelElementStruct()
        {
            ModelType = ModelTypes.SubmodelElementStruct;
        }

        public SubmodelElementStruct(SubmodelElement src, bool shallowCopy = false)
            : base(src, shallowCopy)
        {
            ModelType = ModelTypes.SubmodelElementStruct;
        }
    }
}
