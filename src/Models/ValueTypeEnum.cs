
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Runtime.Serialization;

    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum ValueTypeEnum
    {
        [EnumMember(Value = "anyUri")]
        AnyUriEnum = 0,

        [EnumMember(Value = "base64Binary")]
        Base64BinaryEnum = 1,

        [EnumMember(Value = "boolean")]
        BooleanEnum = 2,

        [EnumMember(Value = "date")]
        DateEnum = 3,

        [EnumMember(Value = "dateTime")]
        DateTimeEnum = 4,

        [EnumMember(Value = "dateTimeStamp")]
        DateTimeStampEnum = 5,

        [EnumMember(Value = "decimal")]
        DecimalEnum = 6,

        [EnumMember(Value = "integer")]
        IntegerEnum = 7,

        [EnumMember(Value = "long")]
        LongEnum = 8,

        [EnumMember(Value = "int")]
        IntEnum = 9,

        [EnumMember(Value = "short")]
        ShortEnum = 10,

        [EnumMember(Value = "byte")]
        ByteEnum = 11,

        [EnumMember(Value = "nonNegativeInteger")]
        NonNegativeIntegerEnum = 12,

        [EnumMember(Value = "positiveInteger")]
        PositiveIntegerEnum = 13,

        [EnumMember(Value = "unsignedLong")]
        UnsignedLongEnum = 14,

        [EnumMember(Value = "unsignedInt")]
        UnsignedIntEnum = 15,

        [EnumMember(Value = "unsignedShort")]
        UnsignedShortEnum = 16,

        [EnumMember(Value = "unsignedByte")]
        UnsignedByteEnum = 17,

        [EnumMember(Value = "nonPositiveInteger")]
        NonPositiveIntegerEnum = 18,

        [EnumMember(Value = "negativeInteger")]
        NegativeIntegerEnum = 19,

        [EnumMember(Value = "double")]
        DoubleEnum = 20,

        [EnumMember(Value = "duration")]
        DurationEnum = 21,

        [EnumMember(Value = "dayTimeDuration")]
        DayTimeDurationEnum = 22,

        [EnumMember(Value = "yearMonthDuration")]
        YearMonthDurationEnum = 23,

        [EnumMember(Value = "float")]
        FloatEnum = 24,

        [EnumMember(Value = "gDay")]
        GDayEnum = 25,

        [EnumMember(Value = "gMonth")]
        GMonthEnum = 26,

        [EnumMember(Value = "gMonthDay")]
        GMonthDayEnum = 27,

        [EnumMember(Value = "gYear")]
        GYearEnum = 28,

        [EnumMember(Value = "gYearMonth")]
        GYearMonthEnum = 29,

        [EnumMember(Value = "hexBinary")]
        HexBinaryEnum = 30,

        [EnumMember(Value = "NOTATION")]
        NOTATIONEnum = 31,

        [EnumMember(Value = "QName")]
        QNameEnum = 32,

        [EnumMember(Value = "string")]
        StringEnum = 33,

        [EnumMember(Value = "normalizedString")]
        NormalizedStringEnum = 34,

        [EnumMember(Value = "token")]
        TokenEnum = 35,

        [EnumMember(Value = "language")]
        LanguageEnum = 36,

        [EnumMember(Value = "Name")]
        NameEnum = 37,

        [EnumMember(Value = "NCName")]
        NCNameEnum = 38,

        [EnumMember(Value = "ENTITY")]
        ENTITYEnum = 39,

        [EnumMember(Value = "ID")]
        IDEnum = 40,

        [EnumMember(Value = "IDREF")]
        IDREFEnum = 41,

        [EnumMember(Value = "NMTOKEN")]
        NMTOKENEnum = 42,

        [EnumMember(Value = "time")]
        TimeEnum = 43
    }
}
