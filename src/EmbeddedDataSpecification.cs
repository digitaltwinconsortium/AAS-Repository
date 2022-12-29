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
        public class EmbeddedDataSpecification
        {
            // members

            [JsonIgnore]
            public DataSpecificationContent dataSpecificationContent = null;

            [XmlIgnore]
            [JsonProperty("dataSpecificationContent")]
            public DataSpecificationIEC61360 JsonWrongDataSpec61360
            {
                get { return dataSpecificationContent?.dataSpecificationIEC61360; }
                set
                {
                    if (dataSpecificationContent == null)
                        dataSpecificationContent = new DataSpecificationContent();
                    dataSpecificationContent.dataSpecificationIEC61360 = value;
                }
            }

            public DataSpecificationRef dataSpecification = null;

            // constructors

            public EmbeddedDataSpecification() { }

            public EmbeddedDataSpecification(
                DataSpecificationRef dataSpecification,
                DataSpecificationContent dataSpecificationContent)
            {
                this.dataSpecification = dataSpecification;
                this.dataSpecificationContent = dataSpecificationContent;
            }

            public EmbeddedDataSpecification(EmbeddedDataSpecification src)
            {
                if (src.dataSpecification != null)
                    this.dataSpecification = new DataSpecificationRef(src.dataSpecification);
                if (src.dataSpecificationContent != null)
                    this.dataSpecificationContent = new DataSpecificationContent(src.dataSpecificationContent);
            }

            public EmbeddedDataSpecification(Reference src)
            {
                if (src != null)
                    this.dataSpecification = new DataSpecificationRef(src);
            }

#if !DoNotUseAasxCompatibilityModels
            public EmbeddedDataSpecification(AasxCompatibilityModels.AdminShellV10.EmbeddedDataSpecification src)
            {
                if (src.hasDataSpecification != null)
                    this.dataSpecification = new DataSpecificationRef(src.hasDataSpecification);
                if (src.dataSpecificationContent != null)
                    this.dataSpecificationContent = new DataSpecificationContent(src.dataSpecificationContent);
            }

            public EmbeddedDataSpecification(AasxCompatibilityModels.AdminShellV10.Reference src)
            {
                if (src != null)
                    this.dataSpecification = new DataSpecificationRef(src);
            }
#endif

            public static EmbeddedDataSpecification CreateIEC61360WithContent()
            {
                var eds = new EmbeddedDataSpecification(new DataSpecificationRef(), new DataSpecificationContent());

                eds.dataSpecification.Keys.Add(DataSpecificationIEC61360.GetKey());

                eds.dataSpecificationContent.dataSpecificationIEC61360 =
                    DataSpecificationIEC61360.CreateNew();

                return eds;
            }

            public DataSpecificationIEC61360 GetIEC61360()
            {
                return this.dataSpecificationContent?.dataSpecificationIEC61360;
            }
        }



    }

    #endregion
}

