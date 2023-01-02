
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    public class Blob : DataElement
    {
        [Required]
        [DataMember(Name = "mimeType")]
        [MetaModelName("Blob.mimeType")]
        public string MimeType { get; set; } = string.Empty;

        [DataMember(Name = "Value")]
        [MetaModelName("Blob.Value")]
        public string Value { get; set; } = string.Empty;

        public Blob() { }

        public Blob(SubmodelElement src)
            : base(src)
        {
            if (!(src is Blob blb))
                return;

            this.mimeType = blb.mimeType;
            this.value = blb.value;
        }

        public static Blob CreateNew(string idShort = null, string category = null, Identifier semanticIdKey = null)
        {
            var x = new Blob();
            x.CreateNewLogic(idShort, category, semanticIdKey);
            return (x);
        }

        public void Set(string mimeType = "", string value = "")
        {
            this.mimeType = mimeType;
            this.value = value;
        }

        public AasElementSelfDescription GetSelfDescription()
        {
            return new AasElementSelfDescription("Blob", "Blob",
                SubmodelElementWrapper.AdequateElementEnum.Blob);
        }

        public object ToValueOnlySerialization()
        {
            var output = new Dictionary<string, Dictionary<string, string>>();
            var valueDict = new Dictionary<string, string>
            {
                { "mimeType", mimeType },
            };

            output.Add(idShort, valueDict);
            return output;
        }
    }
}
