
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class BasicEvent : SubmodelElement
    {
        [Required]
        [DataMember(Name = "observed")]
        [XmlElement(ElementName = "observed")]
        public Reference Observed { get; set; } = new();

        public BasicEvent() { }

        public BasicEvent(SubmodelElement src)
            : base(src)
        {
            if (!(src is BasicEvent be))
                return;

            if (be.Observed != null)
                Observed = new Reference(be.Observed);
        }
    }
}
