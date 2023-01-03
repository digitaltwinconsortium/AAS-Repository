
namespace AdminShell
{
    public class DataElement : SubmodelElement
    {
        public static string ValueType_STRING = "string";

        public static string ValueType_DATE = "date";

        public static string ValueType_BOOLEAN = "boolean";

        public static string[] ValueTypeItems = new string[] {
            "anyURI",
            "base64Binary",
            "boolean",
            "date",
            "dateTime",
            "dateTimeStamp",
            "decimal",
            "integer",
            "long",
            "int",
            "short",
            "byte",
            "nonNegativeInteger",
            "positiveInteger",
            "unsignedLong",
            "unsignedInt",
            "unsignedShort",
            "unsignedByte",
            "nonPositiveInteger",
            "negativeInteger",
            "double",
            "duration",
            "dayTimeDuration",
            "yearMonthDuration",
            "float",
            "hexBinary",
            "string",
            "LangString",
            "time"
        };

        public static string[] ValueTypes_Number = new[] {
            "decimal",
            "integer",
            "long",
            "int",
            "short",
            "byte",
            "nonNegativeInteger",
            "positiveInteger",
            "unsignedLong",
            "unsignedShort",
            "unsignedByte",
            "nonPositiveInteger",
            "negativeInteger",
            "double",
            "float"
        };

        public DataElement() { }

        public DataElement(SubmodelElement src) : base(src) { }
    }
}
