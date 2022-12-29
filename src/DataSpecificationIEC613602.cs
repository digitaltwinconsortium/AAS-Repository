/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using System.Xml.Serialization;
using AdminShellNS;
using Newtonsoft.Json;

//namespace AdminShellNS
namespace AdminShell_V30
{
    public partial class AdminShellV30
    {
        [XmlRoot(Namespace = "http://www.admin-shell.io/IEC61360/3/0")]
        public class DataSpecificationIEC61360
        {
            // static member
            [XmlIgnore]
            [JsonIgnore]
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
                "DATE" };

            // members
            // TODO (MIHO, 2020-08-27): According to spec, cardinality is [1..1][1..n]
            // these cardinalities are NOT MAINTAINED in ANY WAY by the system
            public LangStringSetIEC61360 preferredName = new LangStringSetIEC61360();

            // TODO (MIHO, 2020-08-27): According to spec, cardinality is [0..1][1..n]
            // these cardinalities are NOT MAINTAINED in ANY WAY by the system
            public LangStringSetIEC61360 shortName = null;

            [MetaModelName("DataSpecificationIEC61360.unit")]
            [TextSearchable]
            [CountForHash]
            public string unit = "";

            public UnitId unitId = null;

            [MetaModelName("DataSpecificationIEC61360.valueFormat")]
            [TextSearchable]
            [CountForHash]
            public string valueFormat = null;

            [MetaModelName("DataSpecificationIEC61360.sourceOfDefinition")]
            [TextSearchable]
            [CountForHash]
            public string sourceOfDefinition = null;

            [MetaModelName("DataSpecificationIEC61360.symbol")]
            [TextSearchable]
            [CountForHash]
            public string symbol = null;

            [MetaModelName("DataSpecificationIEC61360.dataType")]
            [TextSearchable]
            [CountForHash]
            public string dataType = "";

            // TODO (MIHO, 2020-08-27): According to spec, cardinality is [0..1][1..n]
            // these cardinalities are NOT MAINTAINED in ANY WAY by the system
            public LangStringSetIEC61360 definition = null;

            // getter / setters

            // constructors

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

#if !DoNotUseAasxCompatibilityModels
            public DataSpecificationIEC61360(AasxCompatibilityModels.AdminShellV10.DataSpecificationIEC61360 src)
            {
                if (src.preferredName != null)
                    this.preferredName = new LangStringSetIEC61360(src.preferredName);
                this.shortName = new LangStringSetIEC61360("EN?", src.shortName);
                this.unit = src.unit;
                if (src.unitId != null)
                    this.unitId = new UnitId(src.unitId);
                this.valueFormat = src.valueFormat;
                if (src.sourceOfDefinition != null && src.sourceOfDefinition.Count > 0)
                    this.sourceOfDefinition = src.sourceOfDefinition[0].str;
                this.symbol = src.symbol;
                this.dataType = src.dataType;
                if (src.definition != null)
                    this.definition = new LangStringSetIEC61360(src.definition);
            }

            public DataSpecificationIEC61360(AasxCompatibilityModels.AdminShellV20.DataSpecificationIEC61360 src)
            {
                if (src.preferredName != null)
                    this.preferredName = new LangStringSetIEC61360(src.preferredName);
                this.shortName = new LangStringSetIEC61360(src.shortName);
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
#endif

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

            // "constants"

            public static Key GetKey()
            {
                return Key.CreateNew(
                            "GlobalReference",
                            "http://admin-shell.io/DataSpecificationTemplates/DataSpecificationIEC61360/3/0");
            }

            public static Identifier GetIdentifier()
            {
                return new Identifier(
                            "http://admin-shell.io/DataSpecificationTemplates/DataSpecificationIEC61360/3/0");
            }

            // validation

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
                        "ConceptDescription: existing shortName with missing langString",
                        () =>
                        {
                            this.shortName = null;
                        }));

                if (this.definition != null && this.definition.Count < 1)
                    results.Add(new AasValidationRecord(
                        AasValidationSeverity.SchemaViolation, cd,
                        "ConceptDescription: existing definition with missing langString",
                        () =>
                        {
                            this.definition = null;
                        }));

                // check data type
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
}
