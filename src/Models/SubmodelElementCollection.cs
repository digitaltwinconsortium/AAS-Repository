
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// In V2.0, this was the most important SME to hold multiple child SMEs.
    /// In V3.0, this is deprecated. Use SubmodelElementList, SubmodelElementStruct instead.
    /// </summary>
    [DataContract]
    public class SubmodelElementCollection : SubmodelElement
    {
        // values == SMEs
        // Important note: XML serialization uses SubModel Element Wrappers while JSON serialization does not!
        // So we have to first deserialize into a placeholder Json member and then copy the contents into the correct member
        [XmlArray(ElementName = "value")]
        public List<SubmodelElementWrapper> Value { get; set; } = new();

        [XmlIgnore]
        [DataMember(Name = "value")]
        public SubmodelElement[] JsonValue
        {
            get
            {
                var submodelElements = new List<SubmodelElement>();

                foreach (SubmodelElementWrapper smew in Value)
                {
                    submodelElements.Add(smew.SubmodelElement);
                }

                return submodelElements.ToArray();
            }

            set
            {
                if (value != null)
                {
                    Value.Clear();

                    foreach (SubmodelElement sme in value)
                    {
                        Value.Add(new SubmodelElementWrapper() { SubmodelElement = sme });
                    }
                }
            }
        }

        [XmlIgnore]
        public bool Ordered = false;

        [XmlIgnore]
        public bool AllowDuplicates = false;

        public SubmodelElementCollection()
        {
            ModelType = ModelTypes.SubmodelElementCollection;
        }

        public SubmodelElementCollection(SubmodelElement src)
            : base(src)
        {
            if (!(src is SubmodelElementCollection smc))
            {
                return;
            }

            Ordered = smc.Ordered;
            AllowDuplicates = smc.AllowDuplicates;
            ModelType = ModelTypes.SubmodelElementCollection;

            foreach (var sme in smc.Value)
            {
                Value.Add(sme);
            }
        }
    }
}
