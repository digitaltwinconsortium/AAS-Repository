
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class ConceptDescription : Identifiable
    {
        [DataMember(Name = "embeddedDataSpecifications")]
        [XmlArray(ElementName = "embeddedDataSpecifications")]
        public List<EmbeddedDataSpecification> EmbeddedDataSpecifications { get; set; }

        [DataMember(Name = "dataSpecifications")]
        [XmlArray(ElementName = "dataSpecifications")]
        public List<Reference> DataSpecifications { get; set; }

        [DataMember(Name = "isCaseOf")]
        [XmlArray(ElementName = "isCaseOf")]
        public List<Reference> IsCaseOf { get; set; }

        [XmlIgnore]
        public DataSpecificationIEC61360 IEC61360Content
        {
            get
            {
                return EmbeddedDataSpecifications?[0].DataSpecificationContent.DataSpecificationIEC61360;
            }
            set
            {
                var eds = EmbeddedDataSpecifications?[0];
                if (eds != null)
                {
                    eds.DataSpecificationContent.DataSpecificationIEC61360 = value;
                    return;
                }

                var edsnew = new EmbeddedDataSpecification();
                edsnew.DataSpecificationContent.DataSpecificationIEC61360 = value;
                EmbeddedDataSpecifications.Add(edsnew);
            }
        }
    }
}

