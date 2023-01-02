
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    // Note: In versions prior to V2.0.1, the SDK has "HasDataSpecification" containing only a Reference.
    // In 2.0.1, theoretically each entity with HasDataSpecification could also contain an EmbeddedDataSpecification.

    [DataContract]
    [XmlType(TypeName = "hasDataSpecification")]
    public class HasDataSpecification
    {
        [DataMember(Name = "embeddedDataSpecifications")]
        public List<EmbeddedDataSpecification> EmbeddedDataSpecifications { get; set; } = new();

        public HasDataSpecification() { }

        public HasDataSpecification(HasDataSpecification src)
        {
            foreach (var r in src)
                this.Add(new EmbeddedDataSpecification(r));
        }

        public HasDataSpecification(IEnumerable<EmbeddedDataSpecification> src)
        {
            foreach (var r in src)
                this.Add(new EmbeddedDataSpecification(r));
        }

        [XmlIgnore]
        public EmbeddedDataSpecification IEC61360
        {
            get
            {
                foreach (var eds in this)
                    if (eds?.dataSpecificationContent?.dataSpecificationIEC61360 != null
                        || eds?.dataSpecification?.Matches(
                            DataSpecificationIEC61360.GetIdentifier(), Key.MatchMode.Identification) == true)
                        return eds;
                return null;
            }
            set
            {
                // search existing first?
                var eds = this.IEC61360;
                if (eds != null)
                {
                    // replace this
                    /* TODO (MIHO, 2020-08-30): this does not prevent the corner case, that we could have
                        * multiple dataSpecificationIEC61360 in this list, which would be an error */
                    this.Remove(eds);
                    this.Add(value);
                    return;
                }

                // no? .. add!
                this.Add(value);
            }
        }

        [XmlIgnore]
        public DataSpecificationIEC61360 IEC61360Content
        {
            get
            {
                return this.IEC61360?.dataSpecificationContent?.dataSpecificationIEC61360;
            }
            set
            {
                // search existing first?
                var eds = this.IEC61360;
                if (eds != null)
                {
                    // replace this
                    eds.dataSpecificationContent.dataSpecificationIEC61360 = value;
                    return;
                }
                // no? .. add!
                var edsnew = new EmbeddedDataSpecification();
                edsnew.dataSpecificationContent.dataSpecificationIEC61360 = value;
                this.Add(edsnew);
            }
        }
    }
}
