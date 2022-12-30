/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;
using System.Xml.Serialization;

namespace AasxCompatibilityModels
{
    #region AdminShell_V2_0

    public partial class AdminShellV20
    {
        [XmlRoot(Namespace = "http://www.admin-shell.io/IEC61360/2/0")]
        public class DataSpecificationIEC61360
        {
            // static member
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
                "DATE" };

            // members
            // TODO (MIHO, 2020-08-27): According to spec, cardinality is [1..1][1..n]
            // these cardinalities are NOT MAINTAINED in ANY WAY by the system
            public LangStringSetIEC61360 preferredName = new LangStringSetIEC61360();

            // TODO (MIHO, 2020-08-27): According to spec, cardinality is [0..1][1..n]
            // these cardinalities are NOT MAINTAINED in ANY WAY by the system
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
                            "GlobalReference", false, "IRI",
                            "http://admin-shell.io/DataSpecificationTemplates/DataSpecificationIEC61360/2/0");
            }
        }



    }

    #endregion
}

