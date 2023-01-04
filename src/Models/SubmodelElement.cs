
namespace AdminShell
{
    using Jose;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class SubmodelElement : Referable
    {
        // constants
        public static Type[] PROP_MLP = new Type[] {
        typeof(MultiLanguageProperty), typeof(Property) };

        [DataMember(Name = "hasDataSpecification")]
        public HasDataSpecification HasDataSpecification { get; set; }

        [DataMember(Name = "semanticId")]
        public Reference SemanticId { get; set; } = new();

        [DataMember(Name = "qualifiers")]
        public List<Qualifier> Qualifiers { get; set; }

        [DataMember(Name = "Kind")]
        public ModelingKind Kind { get; set; } = new();

        public SubmodelElement()
            : base() { }

        public SubmodelElement(SubmodelElement src)
            : base(src)
        {
            if (src == null)
                return;

            if (src.HasDataSpecification != null)
                HasDataSpecification = new HasDataSpecification(src.HasDataSpecification);

            SemanticId = src.SemanticId;

            Kind = src.Kind;

            if (src.Qualifiers != null)
            {
                if (Qualifiers == null)
                    Qualifiers = new List<Qualifier>();

                foreach (var q in src.Qualifiers)
                    Qualifiers.Add(new Qualifier(q));
            }
        }

        public virtual string ValueAsText(string defaultLang = null)
        {
            return string.Empty;
        }
    }
}

