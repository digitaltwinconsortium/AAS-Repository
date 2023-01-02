
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
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
                this.Observed = new ModelReference(be.Observed);
        }

        public BasicEvent(AdminShell.AdminShellV20.BasicEvent src)
            : base(src)
        {
            if (src.observed != null)
                this.Observed = new ModelReference(src.observed);
        }

        public static BasicEvent CreateNew(
            string idShort = null, string category = null, Identifier semanticIdKey = null)
        {
            var x = new BasicEvent();
            x.CreateNewLogic(idShort, category, semanticIdKey);
            return (x);
        }

        public AasElementSelfDescription GetSelfDescription()
        {
            return new AasElementSelfDescription("BasicEvent", "Evt",
                SubmodelElementWrapper.AdequateElementEnum.BasicEvent);
        }

        public object ToValueOnlySerialization()
        {
            var output = new Dictionary<string, object>();
            var list = new List<Dictionary<string, string>>();
            foreach (var key in Observed.Keys)
            {
                var valueDict = new Dictionary<string, string>
                {
                    { "Type", key.type },
                    { "Value", key.value }
                };
                list.Add(valueDict);
            }

            var observedDict = new Dictionary<string, List<Dictionary<string, string>>>();
            observedDict.Add("Observed", list);
            output.Add(idShort, observedDict);

            return output;
        }
    }
}
