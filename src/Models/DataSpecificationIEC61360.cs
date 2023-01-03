
namespace AdminShell
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlRoot(Namespace = "http://www.admin-shell.io/IEC61360/3/0")]
    public class DataSpecificationIEC61360 : ValueObject
    {
        [XmlIgnore]
        public static string[] DataTypeNames = {
            "STRING",
            "STRING_TRANSLATABLE",
            "REAL_MEASURE",
            "REAL_COUNT",
            "REAL_CURRENCY",
            "INTEGER_MEASURE",
            "INTEGER_COUNT",
            "INTEGER_CURRENCY",
            "BOOLEAN",
            "URL",
            "RATIONAL",
            "RATIONAL_MEASURE",
            "TIME",
            "TIMESTAMP",
            "DATE"
        };

        [DataMember(Name = "dataType")]
        public string DataType { get; set; }

        [DataMember(Name = "definition")]
        public List<LangString> Definition { get; set; }

        [DataMember(Name = "levelType")]
        public List<LevelType> LevelType { get; set; }

        [Required]
        [DataMember(Name = "preferredName")]
        public List<LangString> PreferredName { get; set; }

        [DataMember(Name = "shortName")]
        public List<LangString> ShortName { get; set; }

        [DataMember(Name = "sourceOfDefinition")]
        public string SourceOfDefinition { get; set; }

        [DataMember(Name = "symbol")]
        public string Symbol { get; set; }

        [DataMember(Name = "unit")]
        public string Unit { get; set; }

        [DataMember(Name = "unitId")]
        public Reference UnitId { get; set; }

        [DataMember(Name = "valueFormat")]
        public string ValueFormat { get; set; }

        [DataMember(Name = "valueList")]
        public ValueList ValueList { get; set; }

        public LangStringSetIEC61360 preferredName = new LangStringSetIEC61360();

        public LangStringSetIEC61360 shortName = null;

        [MetaModelName("DataSpecificationIEC61360.unit")]
        public string unit = "";

        public UnitId unitId = null;

        [MetaModelName("DataSpecificationIEC61360.valueFormat")]
        public string valueFormat = null;

        [MetaModelName("DataSpecificationIEC61360.sourceOfDefinition")]
        public string sourceOfDefinition = null;

        [MetaModelName("DataSpecificationIEC61360.symbol")]
        public string symbol = null;

        [MetaModelName("DataSpecificationIEC61360.dataType")]
        public string dataType = "";

        public LangStringSetIEC61360 definition = null;

        public DataSpecificationIEC61360() { }

        public DataSpecificationIEC61360(DataSpecificationIEC61360 src)
        {
            if (src.preferredName != null)
                preferredName = new LangStringSetIEC61360(src.preferredName);
            shortName = src.shortName;
            unit = src.unit;
            if (src.unitId != null)
                unitId = new UnitId(src.unitId);
            valueFormat = src.valueFormat;
            sourceOfDefinition = src.sourceOfDefinition;
            symbol = src.symbol;
            dataType = src.dataType;
            if (src.definition != null)
                definition = new LangStringSetIEC61360(src.definition);
        }

        public static Identifier GetIdentifier()
        {
            return new Identifier("http://admin-shell.io/DataSpecificationTemplates/DataSpecificationIEC61360/3/0");
        }
    }
}
