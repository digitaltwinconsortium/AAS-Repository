/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/



//namespace AdminShellNS
namespace AdminShell_V30
{
    public partial class AdminShellV30
    {
        //
        // Derived from SubmodelElements
        //

        public class DataElement : SubmodelElement
        {
            public static string ValueType_STRING = "string";
            public static string ValueType_DATE = "date";
            public static string ValueType_BOOLEAN = "boolean";

            public static string[] ValueTypeItems = new string[] {
                    "anyURI", "base64Binary",
                    "boolean", "date", "dateTime",
                    "dateTimeStamp", "decimal", "integer", "long", "int", "short", "byte", "nonNegativeInteger",
                    "positiveInteger",
                    "unsignedLong", "unsignedInt", "unsignedShort", "unsignedByte",
                    "nonPositiveInteger", "negativeInteger",
                    "double", "duration",
                    "dayTimeDuration", "yearMonthDuration", "float", "hexBinary", "string", "langString", "time" };

            public static string[] ValueTypes_Number = new[] {
                    "decimal", "integer", "long", "int", "short", "byte", "nonNegativeInteger",
                    "positiveInteger",
                    "unsignedLong", "unsignedShort", "unsignedByte", "nonPositiveInteger", "negativeInteger",
                    "double", "float" };

            public DataElement() { }

            public DataElement(SubmodelElement src) : base(src) { }

            public DataElement(DataElement src) : base(src) { }

#if !DoNotUseAasxCompatibilityModels
            public DataElement(AasxCompatibilityModels.AdminShellV10.DataElement src)
                : base(src)
            { }

            public DataElement(AasxCompatibilityModels.AdminShellV20.DataElement src) : base(src) { }
#endif

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("DataElement", "DE");
            }
        }

    }
}
