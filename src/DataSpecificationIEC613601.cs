#define UseAasxCompatibilityModels

using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

/* Copyright (c) 2018-2019 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>, author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/


#if UseAasxCompatibilityModels

namespace AasxCompatibilityModels
{

    #region Utils

    #endregion


    #region AdminShell_V1_0

    public partial class AdminShellV10
    {
        [XmlRoot(Namespace = "http://www.admin-shell.io/IEC61360/1/0")]
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
            public LangStringIEC61360 preferredName = new LangStringIEC61360();
            public string shortName = "";
            public string unit = "";
            public UnitId unitId = null;
            public string valueFormat = null;
            public List<LangStr> sourceOfDefinition = new List<LangStr>();
            public string symbol = null;
            public string dataType = "";
            // public List<LangStr> definition = new List<LangStr>();    
            public LangStringIEC61360 definition = new LangStringIEC61360();

            // getter / setters 

            // constructors

            public DataSpecificationIEC61360() { }

            public DataSpecificationIEC61360(DataSpecificationIEC61360 src)
            {
                if (src.preferredName != null)
                    this.preferredName = new LangStringIEC61360(src.preferredName);
                this.shortName = src.shortName;
                this.unit = src.unit;
                if (src.unitId != null)
                    this.unitId = new UnitId(src.unitId);
                this.valueFormat = src.valueFormat;
                if (src.sourceOfDefinition != null)
                    foreach (var sod in src.sourceOfDefinition)
                        this.sourceOfDefinition.Add(sod);
                this.symbol = src.symbol;
                this.dataType = src.dataType;
                if (src.definition != null)
                    this.definition = new LangStringIEC61360(src.definition);
            }

            public static DataSpecificationIEC61360 CreateNew(
                string[] preferredName = null,
                string shortName = "",
                string unit = "",
                UnitId unitId = null,
                string valueFormat = null,
                string[] sourceOfDefinition = null,
                string symbol = null,
                string dataType = "",
                string[] definition = null
            )
            {
                var d = new DataSpecificationIEC61360();
                if (preferredName != null)
                    d.preferredName.langString = LangStr.CreateManyFromStringArray(preferredName);
                d.shortName = shortName;
                d.unit = unit;
                d.unitId = unitId;
                d.valueFormat = valueFormat;
                if (sourceOfDefinition != null)
                    d.sourceOfDefinition = LangStr.CreateManyFromStringArray(sourceOfDefinition);
                d.symbol = symbol;
                d.dataType = dataType;
                if (definition != null)
                    d.definition.langString = LangStr.CreateManyFromStringArray(definition);
                return (d);
            }
        }

    }

    #endregion
}

#endif