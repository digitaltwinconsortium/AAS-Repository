
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class View : Referable
    {
        [DataMember(Name = "embeddedDataSpecifications")]
        public List<EmbeddedDataSpecification> EmbeddedDataSpecifications { get; set; } = new();

        [XmlIgnore]
        [DataMember(Name = "containedElements")]
        public List<Reference> ContainedElements { get; set; } = new();

        [XmlIgnore]
        [DataMember(Name = "modelType")]
        public JsonModelTypeWrapper JsonModelType { get { return new JsonModelTypeWrapper(GetElementName()); } }

        [DataMember(Name = "semanticId")]
        public SemanticId SemanticId { get; set; } = new();

        [DataMember(Name = "hasDataSpecification")]
        public HasDataSpecification HasDataSpecification { get; set; } = new();

        [XmlIgnore]
        public bool IsEmpty { get { return ContainedElements.Count == 0; } }

        [XmlIgnore]
        public int Count { get { return ContainedElements.Count; } }

        public Reference this[int index]
        {
            get
            {
                if ((index >= ContainedElements.Count) || (index < 0))
                {
                    return null;
                }
                else
                {
                    return ContainedElements[index];
                }
            }
        }

        public string GetElementName()
        {
            return "View";
        }

        public void AddDataSpecification(Key k)
        {
            if (hasDataSpecification == null)
                hasDataSpecification = new HasDataSpecification();
            var r = new Reference();
            r.Keys.Add(k);
            hasDataSpecification.Add(new EmbeddedDataSpecification(r));
        }

        public AasElementSelfDescription GetSelfDescription()
        {
            return new AasElementSelfDescription("View", "View");
        }

    }
}
