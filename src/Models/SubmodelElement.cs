
namespace AdminShell
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    [DataContract]
    public class SubmodelElement : Referable
    {
        // constants
        public static Type[] PROP_MLP = new Type[] {
        typeof(MultiLanguageProperty), typeof(Property) };

        [DataMember(Name = "embeddedDataSpecifications")]
        public List<EmbeddedDataSpecification> EmbeddedDataSpecifications { get; set; }

        [DataMember(Name = "semanticId")]
        public Reference SemanticId { get; set; } = new();

        [DataMember(Name = "qualifiers")]
        public List<Qualifier> Qualifiers { get; set; }

        [DataMember(Name = "kind")]
        public ModelingKind Kind { get; set; } = new();

        public SubmodelElement()
            : base() { }

        public SubmodelElement(SubmodelElement src)
            : base(src)
        {
            if (src == null)
                return;

            if (src.HasDataSpecification != null)
                hasDataSpecification = new HasDataSpecification(src.hasDataSpecification);

            if (src.semanticId != null)
                semanticId = new SemanticId(src.semanticId);

            if (src.kind != null)
                kind = new ModelingKind(src.kind);

            if (src.Qualifiers != null)
            {
                if (Qualifiers == null)
                    Qualifiers = new List<Qualifier>();

                foreach (var q in src.Qualifiers)
                    Qualifiers.Add(new Qualifier(q));
            }
        }

        public IEnumerable<Referable> FindAllParents(
            Predicate<Referable> p,
            bool includeThis = false, bool includeSubmodel = false,
            bool passOverMiss = false)
        {
            // call for this?
            if (includeThis)
            {
                if (p == null || p.Invoke(this))
                    yield return this;
                else
                    if (!passOverMiss)
                    yield break;
            }

            // daisy chain all parents ..
            if (this.parent != null)
            {
                if (this.parent is SubmodelElement psme)
                {
                    foreach (var q in psme.FindAllParents(p, includeThis: true,
                        passOverMiss: passOverMiss))
                        yield return q;
                }
                else if (includeSubmodel && this.parent is Submodel psm)
                {
                    if (p == null || p.Invoke(psm))
                        yield return this;
                }
            }
        }

        public virtual string ValueAsText(string defaultLang = null)
        {
            return "";
        }
    }
}

