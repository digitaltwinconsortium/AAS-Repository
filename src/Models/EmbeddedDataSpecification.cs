
namespace AdminShell
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    [DataContract]
    public class EmbeddedDataSpecification
    {
        [Required]
        [DataMember(Name = "dataSpecification")]
        public Reference DataSpecification { get; set; }

        [Required]
        [DataMember(Name = "dataSpecificationContent")]
        public DataSpecificationContent DataSpecificationContent { get; set; }


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

        public Reference dataSpecification = null;

        public EmbeddedDataSpecification() { }

        public EmbeddedDataSpecification(
            Reference dataSpecification,
            DataSpecificationContent dataSpecificationContent)
        {
            this.dataSpecification = dataSpecification;
            this.dataSpecificationContent = dataSpecificationContent;
        }

        public EmbeddedDataSpecification(EmbeddedDataSpecification src)
        {
            if (src.dataSpecification != null)
                this.dataSpecification = new Reference(src.dataSpecification);
            if (src.dataSpecificationContent != null)
                this.dataSpecificationContent = new DataSpecificationContent(src.dataSpecificationContent);
        }

        public EmbeddedDataSpecification(GlobalReference src)
        {
            if (src != null)
                this.dataSpecification = new Reference(src);
        }

        public static EmbeddedDataSpecification CreateIEC61360WithContent()
        {
            var eds = new EmbeddedDataSpecification(new Reference(), new DataSpecificationContent());

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
