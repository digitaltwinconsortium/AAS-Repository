
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class Range : DataElement
    {
        [DataMember(Name = "max")]
        [MetaModelName("Range.max")]
        public string Max { get; set; }

        [DataMember(Name = "min")]
        [MetaModelName("Range.min")]
        public string Min { get; set; }

        [Required]
        [MetaModelName("Range.valueType")]
        [DataMember(Name = "valueType")]
        public ValueTypeEnum ValueType { get; set; }

        public Range() { }

        public Range(SubmodelElement src)
            : base(src)
        {
            if (!(src is Range rng))
                return;

            ValueType = rng.ValueType;
            Min = rng.Min;
            Max = rng.Max;
        }

        public static Range CreateNew(
            string idShort = null, string category = null, Identifier semanticIdKey = null)
        {
            var x = new Range();
            x.CreateNewLogic(idShort, category, semanticIdKey);
            return (x);
        }

        public AasElementSelfDescription GetSelfDescription()
        {
            return new AasElementSelfDescription("Range", "Range",
                SubmodelElementWrapper.AdequateElementEnum.Range);
        }

        public string ValueAsText(string defaultLang = null)
        {
            return "" + min + " .. " + max;
        }

        public object ToValueOnlySerialization()
        {
            var output = new Dictionary<string, Dictionary<string, string>>();
            var valueDict = new Dictionary<string, string>
            {
                { "min", Min },
                { "max", Max }
            };

            output.Add(idShort, valueDict);
            return output;
        }
    }
}
