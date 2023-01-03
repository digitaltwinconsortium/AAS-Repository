
namespace AdminShell
{
    using System.Runtime.Serialization;

    [DataContract]
    public class SubmodelElementStruct : SubmodelElementCollection
    {
        [DataMember(Name = "Value")]
        public SubmodelElement Value { get; set; }

        public SubmodelElementStruct() { }

        public SubmodelElementStruct(SubmodelElement src, bool shallowCopy = false)
            : base(src, shallowCopy)
        {
        }

        public new static SubmodelElementStruct CreateNew(
            string idShort = null, string category = null, Identifier semanticIdKey = null)
        {
            var x = new SubmodelElementStruct();
            x.CreateNewLogic(idShort, category, semanticIdKey);
            return (x);
        }

        public override AasElementSelfDescription GetSelfDescription()
        {
            return new AasElementSelfDescription("SubmodelElementStruct", "SMS",
                SubmodelElementWrapper.AdequateElementEnum.SubmodelElementStruct);
        }
    }
}
