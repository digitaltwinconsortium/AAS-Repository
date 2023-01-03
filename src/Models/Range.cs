
namespace AdminShell
{
    using Newtonsoft.Json;
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
    }
}
