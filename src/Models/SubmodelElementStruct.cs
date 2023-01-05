
namespace AdminShell
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class SubmodelElementStruct : SubmodelElementCollection
    {
        [DataMember(Name = "value")]
        [XmlElement(ElementName = "value")]
        public new SubmodelElement Value { get; set; }

        public SubmodelElementStruct() { }

        public SubmodelElementStruct(SubmodelElement src, bool shallowCopy = false)
            : base(src, shallowCopy)
        {
        }
    }
}
