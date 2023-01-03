
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// In V2.0, this was the most important SME to hold multiple child SMEs.
    /// Ib V3.0, this is deprecated. Use SubmodelElementList, SubmodelElementStruct instead.
    /// </summary>
    public class SubmodelElementCollection : SubmodelElement
    {
        // values == SMEs
        [XmlIgnore]
        public List<SubmodelElement> Value { get; set; } = new();

        public bool ordered = false;
        public bool allowDuplicates = false;

        public SubmodelElementCollection() { }

        public SubmodelElementCollection(SubmodelElement src, bool shallowCopy = false)
            : base(src)
        {
            if (!(src is SubmodelElementCollection smc))
                return;

            ordered = smc.ordered;
            allowDuplicates = smc.allowDuplicates;

            if (!shallowCopy)
                foreach (var sme in smc.Value)
                    Value.Add(new SubmodelElement(sme));
        }
    }
}
