
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum ValueTypeEnum
    {
        [EnumMember(Value = "anyUri")]
        [XmlEnum(Name= "anyUri")]
        AnyUri = 0,

        [EnumMember(Value = "base64Binary")]
        [XmlEnum(Name= "base64Binary")]
        Base64Binary = 1,

        [EnumMember(Value = "boolean")]
        [XmlEnum(Name= "boolean")]
        Boolean = 2,

        [EnumMember(Value = "date")]
        [XmlEnum(Name= "date")]
        Date = 3,

        [EnumMember(Value = "dateTime")]
        [XmlEnum(Name= "dateTime")]
        DateTime = 4,

        [EnumMember(Value = "dateTimeStamp")]
        [XmlEnum(Name = "dateTimeStamp")]
        DateTimeStamp = 5,

        [EnumMember(Value = "decimal")]
        [XmlEnum(Name = "decimal")]
        Decimal = 6,

        [EnumMember(Value = "integer")]
        [XmlEnum(Name = "integer")]
        Integer = 7,

        [EnumMember(Value = "long")]
        [XmlEnum(Name = "long")]
        Long = 8,

        [EnumMember(Value = "int")]
        [XmlEnum(Name = "int")]
        Int = 9,

        [EnumMember(Value = "short")]
        [XmlEnum(Name = "short")]
        Short = 10,

        [EnumMember(Value = "byte")]
        [XmlEnum(Name = "byte")]
        Byte = 11,

        [EnumMember(Value = "nonNegativeInteger")]
        [XmlEnum(Name = "nonNegativeInteger")]
        NonNegativeInteger = 12,

        [EnumMember(Value = "positiveInteger")]
        [XmlEnum(Name = "positiveInteger")]
        PositiveInteger = 13,

        [EnumMember(Value = "unsignedLong")]
        [XmlEnum(Name = "unsignedLong")]
        UnsignedLong = 14,

        [EnumMember(Value = "unsignedInt")]
        [XmlEnum(Name = "unsignedInt")]
        UnsignedInt = 15,

        [EnumMember(Value = "unsignedShort")]
        [XmlEnum(Name = "unsignedShort")]
        UnsignedShort = 16,

        [EnumMember(Value = "unsignedByte")]
        [XmlEnum(Name = "unsignedByte")]
        UnsignedByte = 17,

        [EnumMember(Value = "nonPositiveInteger")]
        [XmlEnum(Name = "nonPositiveInteger")]
        NonPositiveInteger = 18,

        [EnumMember(Value = "negativeInteger")]
        [XmlEnum(Name = "negativeInteger")]
        NegativeInteger = 19,

        [EnumMember(Value = "double")]
        [XmlEnum(Name = "double")]
        Double = 20,

        [EnumMember(Value = "duration")]
        [XmlEnum(Name = "duration")]
        Duration = 21,

        [EnumMember(Value = "dayTimeDuration")]
        [XmlEnum(Name = "dayTimeDuration")]
        DayTimeDuration = 22,

        [EnumMember(Value = "yearMonthDuration")]
        [XmlEnum(Name = "yearMonthDuration")]
        YearMonthDuration = 23,

        [EnumMember(Value = "float")]
        [XmlEnum(Name = "float")]
        Float = 24,

        [EnumMember(Value = "gDay")]
        [XmlEnum(Name = "gDay")]
        GDay = 25,

        [EnumMember(Value = "gMonth")]
        [XmlEnum(Name = "gMonth")]
        GMonth = 26,

        [EnumMember(Value = "gMonthDay")]
        [XmlEnum(Name = "dateTime")]
        GMonthDay = 27,

        [EnumMember(Value = "gYear")]
        [XmlEnum(Name = "gYear")]
        GYear = 28,

        [EnumMember(Value = "gYearMonth")]
        [XmlEnum(Name = "gYearMonth")]
        GYearMonth = 29,

        [EnumMember(Value = "hexBinary")]
        [XmlEnum(Name = "hexBinary")]
        HexBinary = 30,

        [EnumMember(Value = "NOTATION")]
        [XmlEnum(Name = "NOTATION")]
        NOTATION = 31,

        [EnumMember(Value = "QName")]
        [XmlEnum(Name = "QName")]
        QName = 32,

        [EnumMember(Value = "string")]
        [XmlEnum(Name ="string")]
        String = 33,

        [EnumMember(Value = "normalizedString")]
        [XmlEnum(Name = "normalizedString")]
        NormalizedString = 34,

        [EnumMember(Value = "token")]
        [XmlEnum(Name = "token")]
        Token = 35,

        [EnumMember(Value = "language")]
        [XmlEnum(Name = "language")]
        Language = 36,

        [EnumMember(Value = "Name")]
        [XmlEnum(Name = "Name")]
        Name = 37,

        [EnumMember(Value = "NCName")]
        [XmlEnum(Name = "NCName")]
        NCName = 38,

        [EnumMember(Value = "ENTITY")]
        [XmlEnum(Name = "ENTITY")]
        ENTITY = 39,

        [EnumMember(Value = "ID")]
        [XmlEnum(Name = "ID")]
        ID = 40,

        [EnumMember(Value = "IDREF")]
        [XmlEnum(Name = "IDREF")]
        IDREF = 41,

        [EnumMember(Value = "NMTOKEN")]
        [XmlEnum(Name = "NMTOKEN")]
        NMTOKEN = 42,

        [EnumMember(Value = "time")]
        [XmlEnum(Name = "time")]
        Time = 43
    }
}
