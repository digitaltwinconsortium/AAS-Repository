
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum  ValueTypeEnum
    {
        [EnumMember(Value = "anyUri")]
        AnyUri = 0,

        [EnumMember(Value = "base64Binary")]
        Base64Binary = 1,

        [EnumMember(Value = "boolean")]
        Boolean = 2,

        [EnumMember(Value = "date")]
        Date = 3,

        [EnumMember(Value = "dateTime")]
        DateTime = 4,

        [EnumMember(Value = "dateTimeStamp")]
        DateTimeStamp = 5,

        [EnumMember(Value = "decimal")]
        Decimal = 6,

        [EnumMember(Value = "integer")]
        Integer = 7,

        [EnumMember(Value = "long")]
        Long = 8,

        [EnumMember(Value = "int")]
        Int = 9,

        [EnumMember(Value = "short")]
        Short = 10,

        [EnumMember(Value = "byte")]
        Byte = 11,

        [EnumMember(Value = "nonNegativeInteger")]
        NonNegativeInteger = 12,

        [EnumMember(Value = "positiveInteger")]
        PositiveInteger = 13,

        [EnumMember(Value = "unsignedLong")]
        UnsignedLong = 14,

        [EnumMember(Value = "unsignedInt")]
        UnsignedInt = 15,

        [EnumMember(Value = "unsignedShort")]
        UnsignedShort = 16,

        [EnumMember(Value = "unsignedByte")]
        UnsignedByte = 17,

        [EnumMember(Value = "nonPositiveInteger")]
        NonPositiveInteger = 18,

        [EnumMember(Value = "negativeInteger")]
        NegativeInteger = 19,

        [EnumMember(Value = "double")]
        Double = 20,

        [EnumMember(Value = "duration")]
        Duration = 21,

        [EnumMember(Value = "dayTimeDuration")]
        DayTimeDuration = 22,

        [EnumMember(Value = "yearMonthDuration")]
        YearMonthDuration = 23,

        [EnumMember(Value = "float")]
        Float = 24,

        [EnumMember(Value = "gDay")]
        GDay = 25,

        [EnumMember(Value = "gMonth")]
        GMonth = 26,

        [EnumMember(Value = "gMonthDay")]
        GMonthDay = 27,

        [EnumMember(Value = "gYear")]
        GYear = 28,

        [EnumMember(Value = "gYearMonth")]
        GYearMonth = 29,

        [EnumMember(Value = "hexBinary")]
        HexBinary = 30,

        [EnumMember(Value = "NOTATION")]
        NOTATION = 31,

        [EnumMember(Value = "QName")]
        QName = 32,

        [EnumMember(Value = "string")]
        [XmlEnum(Name ="string")]
        String = 33,

        [EnumMember(Value = "normalizedString")]
        NormalizedString = 34,

        [EnumMember(Value = "token")]
        Token = 35,

        [EnumMember(Value = "language")]
        Language = 36,

        [EnumMember(Value = "Name")]
        Name = 37,

        [EnumMember(Value = "NCName")]
        NCName = 38,

        [EnumMember(Value = "ENTITY")]
        ENTITY = 39,

        [EnumMember(Value = "ID")]
        ID = 40,

        [EnumMember(Value = "IDREF")]
        IDREF = 41,

        [EnumMember(Value = "NMTOKEN")]
        NMTOKEN = 42,

        [EnumMember(Value = "time")]
        Time = 43
    }
}
