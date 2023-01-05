
namespace AdminShell
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class SubmodelElement : Referable
    {
        public static Type[] PROP_MLP = new Type[]
        {
            typeof(MultiLanguageProperty), typeof(Property)
        };

        [DataMember(Name = "hasDataSpecification")]
        [XmlElement(ElementName = "hasDataSpecification")]
        public HasDataSpecification HasDataSpecification { get; set; }

        [DataMember(Name = "semanticId")]
        [XmlElement(ElementName = "semanticId")]
        public Reference SemanticId { get; set; } = new();

        [DataMember(Name = "qualifiers")]
        [XmlElement(ElementName = "qualifiers")]
        public List<Qualifier> Qualifiers { get; set; }

        [DataMember(Name = "kind")]
        [XmlElement(ElementName = "kind")]
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

