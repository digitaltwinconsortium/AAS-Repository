
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    public class BasicEvent : SubmodelElement
    {
        [Required]
        [DataMember(Name = "Observed")]
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
