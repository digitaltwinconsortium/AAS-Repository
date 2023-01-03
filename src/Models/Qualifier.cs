
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;

    [DataContract]
    public class Qualifier : IAasElement
    {
        [DataMember(Name = "value")]
        [MetaModelName("Qualifier.value")]
        public string Value { get; set; }

        [DataMember(Name = "valueId")]
        public GlobalReference ValueId { get; set; }

        [DataMember(Name = "valueType")]
        [MetaModelName("Qualifier.valueType")]
        public ValueTypeEnum ValueType { get; set; }

        [Required]
        [DataMember(Name = "type")]
        [MetaModelName("Qualifier.type")]
        public string Type { get; set; }

        [XmlElement(ElementName = "semanticId")]
        public SemanticId semanticId = null;

        public SemanticId GetSemanticId() { return semanticId; }

        public Qualifier() { }

        public Qualifier(Qualifier src)
        {
            if (src.semanticId != null)
                semanticId = new SemanticId(src.semanticId);

            this.Type = src.Type;

            this.Value = src.Value;

            if (src.ValueId != null)
                this.ValueId = new GlobalReference(src.ValueId);
        }

        public AasElementSelfDescription GetSelfDescription()
        {
            return new AasElementSelfDescription("Qualifier", "Qfr");
        }

        public string GetElementName()
        {
            return this.GetSelfDescription()?.ElementName;
        }

        public string ToString(int format = 0, string delimiter = ",")
        {
            var res = "" + Type;
            if (res == "")
                res += "" + semanticId?.ToString(format, delimiter);

            if (Value != null)
                res += " = " + Value;
            else if (ValueId != null)
                res += " = " + ValueId?.ToString(format, delimiter);

            return res;
        }

        public override string ToString()
        {
            return this.ToString(0);
        }

        public static Qualifier Parse(string input)
        {
            var m = Regex.Match(input, @"\s*([^,]*)(,[^=]+){0,1}\s*=\s*([^,]*)(,.+){0,1}\s*");
            if (!m.Success)
                return null;

            return new Qualifier()
            {
                Type = m.Groups[1].ToString().Trim(),
                semanticId = SemanticId.Parse(m.Groups[1].ToString().Trim()),
                Value = m.Groups[3].ToString().Trim(),
                ValueId = GlobalReference.Parse(m.Groups[1].ToString().Trim())
            };
        }
    }
}
