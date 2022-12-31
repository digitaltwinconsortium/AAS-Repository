
namespace AdminShell
{
    using Newtonsoft.Json;
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

        public void AddContainedElement(Key k)
        {
            if (containedElements == null)
                containedElements = new ContainedElements();
            var r = new ContainedElementRef();
            r.Keys.Add(k);
            containedElements.reference.Add(r);
        }

        public void AddContainedElement(List<Key> keys)
        {
            if (containedElements == null)
                containedElements = new ContainedElements();
            var r = new ContainedElementRef();
            foreach (var k in keys)
                r.Keys.Add(k);
            containedElements.reference.Add(r);
        }

        public void AddContainedElement(Reference r)
        {
            if (containedElements == null)
                containedElements = new ContainedElements();
            containedElements.reference.Add(ContainedElementRef.CreateNew(r));
        }

        public void AddContainedElement(List<Reference> rlist)
        {
            if (containedElements == null)
                containedElements = new ContainedElements();
            foreach (var r in rlist)
                containedElements.reference.Add(ContainedElementRef.CreateNew(r));
        }

        public override AasElementSelfDescription GetSelfDescription()
        {
            return new AasElementSelfDescription("View", "View");
        }

    }
}
