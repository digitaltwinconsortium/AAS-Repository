
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
                this.preferredName = new LangStringSetIEC61360(src.preferredName);
            this.shortName = src.shortName;
            this.unit = src.unit;
            if (src.unitId != null)
                this.unitId = new UnitId(src.unitId);
            this.valueFormat = src.valueFormat;
            this.sourceOfDefinition = src.sourceOfDefinition;
            this.symbol = src.symbol;
            this.dataType = src.dataType;
            if (src.definition != null)
                this.definition = new LangStringSetIEC61360(src.definition);
        }

        public static DataSpecificationIEC61360 CreateNew(
            string[] preferredName = null,
            string shortName = "",
            string unit = "",
            UnitId unitId = null,
            string valueFormat = null,
            string sourceOfDefinition = null,
            string symbol = null,
            string dataType = "",
            string[] definition = null
        )
        {
            var d = new DataSpecificationIEC61360();
            if (preferredName != null)
            {
                d.preferredName = new LangStringSetIEC61360(LangStr.CreateManyFromStringArray(preferredName));
            }
            d.shortName = new LangStringSetIEC61360("EN?", shortName);
            d.unit = unit;
            d.unitId = unitId;
            d.valueFormat = valueFormat;
            d.sourceOfDefinition = sourceOfDefinition;
            d.symbol = symbol;
            d.dataType = dataType;
            if (definition != null)
            {
                if (d.definition == null)
                    d.definition = new LangStringSetIEC61360();
                d.definition = new LangStringSetIEC61360(LangStr.CreateManyFromStringArray(definition));
            }
            return (d);
        }

        public static Key GetKey()
        {
            return Key.CreateNew("GlobalReference", "http://admin-shell.io/DataSpecificationTemplates/DataSpecificationIEC61360/3/0");
        }

        public static Identifier GetIdentifier()
        {
            return new Identifier("http://admin-shell.io/DataSpecificationTemplates/DataSpecificationIEC61360/3/0");
        }

        public void Validate(AasValidationRecordList results, ConceptDescription cd)
        {
            // access
            if (results == null || cd == null)
                return;

            // check IEC61360 spec
            if (this.preferredName == null || this.preferredName.Count < 1)
                results.Add(new AasValidationRecord(
                    AasValidationSeverity.SchemaViolation, cd,
                    "ConceptDescription: missing preferredName",
                    () =>
                    {
                        this.preferredName = new AdminShell.LangStringSetIEC61360("EN?",
                            AdminShellUtil.EvalToNonEmptyString("{0}", cd.idShort, "UNKNOWN"));
                    }));

            if (this.shortName != null && this.shortName.Count < 1)
                results.Add(new AasValidationRecord(
                    AasValidationSeverity.SchemaViolation, cd,
                    "ConceptDescription: existing shortName with missing LangString",
                    () =>
                    {
                        this.shortName = null;
                    }));

            if (this.definition != null && this.definition.Count < 1)
                results.Add(new AasValidationRecord(
                    AasValidationSeverity.SchemaViolation, cd,
                    "ConceptDescription: existing definition with missing LangString",
                    () =>
                    {
                        this.definition = null;
                    }));

            // check data Type
            string foundDataType = null;
            if (this.dataType != null)
                foreach (var dtn in DataTypeNames)
                    if (this.dataType.Trim() == dtn.Trim())
                        foundDataType = this.dataType;
            if (foundDataType == null)
                results.Add(new AasValidationRecord(
                    AasValidationSeverity.SchemaViolation, cd,
                    "ConceptDescription: dataType does not match allowed enumeration values",
                    () =>
                    {
                        this.dataType = "STRING";
                    }));
        }
    }
}
