
namespace AdminShell
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class ReferenceElement : DataElement
    {
        [DataMember(Name = "value")]
        [XmlElement(ElementName = "value")]
        public Reference Value { get; set; } = new();

        public ReferenceElement()
        {
            ModelType.Name = ModelTypes.ReferenceElement;
        }

        public ReferenceElement(SubmodelElement src)
            : base(src)
        {
            if (!(src is ReferenceElement re))
            {
                return;
            }

            ModelType.Name = ModelTypes.ReferenceElement;

            if (re.Value != null)
            {
                Value = new Reference(re.Value);
            }
        }
    }
}
