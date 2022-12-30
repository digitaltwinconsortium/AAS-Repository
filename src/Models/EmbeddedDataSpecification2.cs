/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using System.Xml.Serialization;
using Newtonsoft.Json;

//namespace AdminShellNS
namespace AdminShell_V30
{
    public partial class AdminShellV30
    {
        public class EmbeddedDataSpecification
        {
            // members

            
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

            public EmbeddedDataSpecification(GlobalReference src)
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

            public EmbeddedDataSpecification(AasxCompatibilityModels.AdminShellV20.EmbeddedDataSpecification src)
            {
                if (src.dataSpecification != null)
                    this.dataSpecification = new DataSpecificationRef(src.dataSpecification);
                if (src.dataSpecificationContent != null)
                    this.dataSpecificationContent = new DataSpecificationContent(src.dataSpecificationContent);
            }
#endif

            public static EmbeddedDataSpecification CreateIEC61360WithContent()
            {
                var eds = new EmbeddedDataSpecification(new DataSpecificationRef(), new DataSpecificationContent());

                eds.dataSpecification.Value.Add(DataSpecificationIEC61360.GetIdentifier());

                eds.dataSpecificationContent.dataSpecificationIEC61360 =
                    AdminShell.DataSpecificationIEC61360.CreateNew();

                return eds;
            }

            public DataSpecificationIEC61360 GetIEC61360()
            {
                return this.dataSpecificationContent?.dataSpecificationIEC61360;
            }
        }

    }
}
