
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml;
    using System.Xml.Serialization;

    [DataContract]
    public class Submodel : Identifiable
    {
        [DataMember(Name = "embeddedDataSpecifications")]
        public List<EmbeddedDataSpecification> EmbeddedDataSpecifications { get; set; }

        [DataMember(Name = "qualifiers")]
        public List<Qualifier> Qualifiers { get; set; }

        [DataMember(Name = "semanticId")]
        public Reference SemanticId { get; set; }

        [DataMember(Name = "kind")]
        public ModelingKind Kind { get; set; }

        [DataMember(Name = "submodelElements")]
        public List<SubmodelElement> SubmodelElements { get; set; }

        [DataMember(Name = "hasDataSpecification")]
        public HasDataSpecification HasDataSpecification { get; set; }

        public static IEnumerable<SubmodelElement> EnumerateSMEChildren(SubmodelElement sme)
        {
            if (sme != null)
                if (sme is SubmodelElementCollection collection)
                    if (collection.Value != null)
                        foreach (SubmodelElement smeChild in collection.Value)
                            yield return smeChild;
        }

        public static void SetParentsForSMEs(Referable parent, SubmodelElement sme)
        {
            if (sme == null)
                return;

            sme.Parent = parent;

            var children = EnumerateSMEChildren(sme);
            if (children != null)
                foreach (SubmodelElement smeChild in children)
                    SetParentsForSMEs(sme, smeChild);
        }

        public void SetAllParents()
        {
            if (SubmodelElements != null)
                foreach (SubmodelElement sme in SubmodelElements)
                    SetParentsForSMEs(this, sme);
        }
    }
}
