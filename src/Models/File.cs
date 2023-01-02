
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class File : DataElement
    {
        [Required]
        [DataMember(Name = "mimeType")]
        [MetaModelName("File.mimeType")]
        public string MimeType { get; set; }

        [DataMember(Name = "Value")]
        [MetaModelName("File.Value")]
        public string Value { get; set; }


        public File() { }

        public File(SubmodelElement src)
            : base(src)
        {
            if (!(src is File file))
                return;

            MimeType = file.MimeType;
            Value = file.Value;
        }

        public static File CreateNew(string idShort = null, string category = null, Identifier semanticIdKey = null)
        {
            var x = new File();
            x.CreateNewLogic(idShort, category, semanticIdKey);
            return (x);
        }

        public void Set(string mimeType = "", string value = "")
        {
            this.mimeType = mimeType;
            this.value = value;
        }

        public override AasElementSelfDescription GetSelfDescription()
        {
            return new AasElementSelfDescription("File", "File",
                SubmodelElementWrapper.AdequateElementEnum.File);
        }

        public string ValueAsText(string defaultLang = null)
        {
            return "" + value;
        }

        public object ToValueOnlySerialization()
        {
            var output = new Dictionary<string, Dictionary<string, string>>();
            var valueDict = new Dictionary<string, string>
            {
                { "mimeType", mimeType },
                { "Value", value }
            };

            output.Add(idShort, valueDict);
            return output;
        }
    }
}

