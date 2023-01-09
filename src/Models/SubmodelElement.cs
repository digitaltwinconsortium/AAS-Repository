
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
        public HasDataSpecification HasDataSpecification { get; set; } = new();

        [DataMember(Name = "semanticId")]
        [XmlElement(ElementName = "semanticId")]
        public Reference SemanticId { get; set; } = new();

        [DataMember(Name = "qualifier")]
        [XmlArray(ElementName = "qualifier")]
        [XmlArrayItem(ElementName = "qualifier")]
        public List<Qualifier> Qualifiers { get; set; } = new();

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

            foreach (var q in src.Qualifiers)
            {
                Qualifiers.Add(new Qualifier(q));
            }
        }
    }
}

