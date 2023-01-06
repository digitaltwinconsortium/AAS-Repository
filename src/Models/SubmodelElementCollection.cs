
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    /// <summary>
    /// In V2.0, this was the most important SME to hold multiple child SMEs.
    /// Ib V3.0, this is deprecated. Use SubmodelElementList, SubmodelElementStruct instead.
    /// </summary>
    [DataContract]
    public class SubmodelElementCollection : SubmodelElement
    {
        // values == SMEs
        [DataMember(Name ="value")]
        [XmlArray(ElementName = "value")]
        public List<SubmodelElementWrapper> Value { get; set; } = new();

        [XmlIgnore]
        public bool Ordered = false;

        [XmlIgnore]
        public bool AllowDuplicates = false;

        public SubmodelElementCollection() { }

        public SubmodelElementCollection(SubmodelElement src, bool shallowCopy = false)
            : base(src)
        {
            if (!(src is SubmodelElementCollection smc))
                return;

            Ordered = smc.Ordered;
            AllowDuplicates = smc.AllowDuplicates;

            if (!shallowCopy)
                foreach (var sme in smc.Value)
                    Value.Add(sme);
        }
    }
}
