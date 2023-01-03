
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    public class ConceptDescription : Identifiable
    {
        [DataMember(Name = "embeddedDataSpecifications")]
        public List<EmbeddedDataSpecification> EmbeddedDataSpecifications { get; set; }

        [DataMember(Name = "isCaseOf")]
        public List<Reference> IsCaseOf { get; set; }

        [XmlIgnore]
        private List<ModelReference> isCaseOf = null;

        public ConceptDescription() : base() { }

        public ConceptDescription(ConceptDescription src)
            : base(src)
        {
            if (src.EmbeddedDataSpecifications != null)
                EmbeddedDataSpecifications = new List<EmbeddedDataSpecification>(src.EmbeddedDataSpecifications);

            if (src.isCaseOf != null)
                foreach (var ico in src.isCaseOf)
                {
                    if (isCaseOf == null)
                        isCaseOf = new List<ModelReference>();

                    isCaseOf.Add(new ModelReference(ico));
                }
        }

        [XmlIgnore]
        public DataSpecificationIEC61360 IEC61360Content
        {
            get
            {
                return EmbeddedDataSpecifications?[0].DataSpecificationContent.dataSpecificationIEC61360;
            }
            set
            {
                // search existing first?
                var eds = EmbeddedDataSpecifications?[0];
                if (eds != null)
                {
                    // replace this
                    eds.DataSpecificationContent.dataSpecificationIEC61360 = value;
                    return;
                }
                // no? .. add!
                var edsnew = new EmbeddedDataSpecification();
                edsnew.DataSpecificationContent.dataSpecificationIEC61360 = value;
                EmbeddedDataSpecifications.Add(edsnew);
            }
        }
    }
}

