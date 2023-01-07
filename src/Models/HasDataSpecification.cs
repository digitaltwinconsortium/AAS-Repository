
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    // Note: In versions prior to V2.0.1, the SDK has "HasDataSpecification" containing only a Reference.
    // In 2.0.1, theoretically each entity with HasDataSpecification could also contain an EmbeddedDataSpecification.

    [DataContract]
    public class HasDataSpecification
    {
        [DataMember(Name = "embeddedDataSpecifications")]
        [XmlArray(ElementName = "embeddedDataSpecifications")]
        public List<EmbeddedDataSpecification> EmbeddedDataSpecifications { get; set; } = new();

        public HasDataSpecification() { }

        public HasDataSpecification(HasDataSpecification src)
        {
            foreach (var r in src.EmbeddedDataSpecifications)
                EmbeddedDataSpecifications.Add(r);
        }

        public HasDataSpecification(IEnumerable<EmbeddedDataSpecification> src)
        {
            foreach (EmbeddedDataSpecification eds in src)
                EmbeddedDataSpecifications.Add(new EmbeddedDataSpecification(eds));
        }

        [XmlIgnore]
        public EmbeddedDataSpecification IEC61360
        {
            get
            {
                foreach (EmbeddedDataSpecification eds in EmbeddedDataSpecifications)
                    if ((eds.DataSpecificationContent?.DataSpecificationIEC61360 != null)
                      || eds.DataSpecification?.Matches(DataSpecificationIEC61360.GetIdentifier()) == true)
                        return eds;

                return null;
            }
            set
            {
                // search existing first?
                var eds = IEC61360;
                if (eds != null)
                {
                    // TODO: Remove all or just one?
                    EmbeddedDataSpecifications.Remove(eds);

                    EmbeddedDataSpecifications.Add(value);

                    return;
                }

                // no? .. add!
                EmbeddedDataSpecifications.Add(value);
            }
        }
    }
}
