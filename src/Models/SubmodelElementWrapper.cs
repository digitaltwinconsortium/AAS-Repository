
namespace AdminShell
{
    using System.Runtime.Serialization;
    using System.Xml;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "SubmodelElement")]
    public class SubmodelElementWrapper
    {
        [DataMember]
        [XmlElement(ElementName = "property", Type = typeof(Property))]
        [XmlElement(ElementName = "multiLanguageProperty", Type = typeof(MultiLanguageProperty))]
        [XmlElement(ElementName = "range", Type = typeof(Range))]
        [XmlElement(ElementName = "file", Type = typeof(File))]
        [XmlElement(ElementName = "blob", Type = typeof(Blob))]
        [XmlElement(ElementName = "referenceElement", Type = typeof(ReferenceElement))]
        [XmlElement(ElementName = "relationshipElement", Type = typeof(RelationshipElement))]
        [XmlElement(ElementName = "annotatedRelationshipElement", Type = typeof(AnnotatedRelationshipElement))]
        [XmlElement(ElementName = "capability", Type = typeof(Capability))]
        [XmlElement(ElementName = "submodelElementCollection", Type = typeof(SubmodelElementCollection))]
        [XmlElement(ElementName = "operation", Type = typeof(Operation))]
        [XmlElement(ElementName = "basicEvent", Type = typeof(BasicEvent))]
        [XmlElement(ElementName = "entity", Type = typeof(Entity))]
        [XmlElement(ElementName = "submodelElementList", Type = typeof(SubmodelElementList))]
        [XmlElement(ElementName = "submodelElementStruct", Type = typeof(SubmodelElementStruct))]
        [XmlElement(ElementName = "globalReferenceElement", Type = typeof(GlobalReferenceElement))]
        [XmlElement(ElementName = "modelReferenceElement", Type = typeof(ModelReferenceElement))]
        public SubmodelElement SubmodelElement { get; set; }

        [XmlIgnore]
        public static string MetaModelVersionCoarse = "AAS3.0";

        [XmlIgnore]
        public static string MetaModelVersionFine = "RC02";

        public enum AdequateElementEnum
        {
            Unknown = 0, SubmodelElementCollection, Property, MultiLanguageProperty, Range, File, Blob,
            ReferenceElement, RelationshipElement, AnnotatedRelationshipElement, Capability, Operation,
            BasicEvent, Entity, SubmodelElementList, SubmodelElementStruct,
            ModelReferenceElement, GlobalReferenceElement
        }

        public SubmodelElementWrapper() { }

        public SubmodelElementWrapper(SubmodelElement src, bool shallowCopy = false)
        {
            if (src is SubmodelElementCollection)
                SubmodelElement = new SubmodelElementCollection(
                    src as SubmodelElementCollection, shallowCopy: shallowCopy);
            if (src is Property)
                SubmodelElement = new Property(src as Property);
            if (src is MultiLanguageProperty)
                SubmodelElement = new MultiLanguageProperty(src as MultiLanguageProperty);
            if (src is Range)
                SubmodelElement = new Range(src as Range);
            if (src is File)
                SubmodelElement = new File(src as File);
            if (src is Blob)
                SubmodelElement = new Blob(src as Blob);
            if (src is ReferenceElement)
                SubmodelElement = new ReferenceElement(src as ReferenceElement);
            if (src is RelationshipElement)
                SubmodelElement = new RelationshipElement(src as RelationshipElement);
            if (src is AnnotatedRelationshipElement)
                SubmodelElement = new AnnotatedRelationshipElement(src as AnnotatedRelationshipElement);
            if (src is Capability)
                SubmodelElement = new Capability(src as Capability);
            if (src is Operation)
                SubmodelElement = new Operation(src as Operation);
            if (src is BasicEvent)
                SubmodelElement = new BasicEvent(src as BasicEvent);
            if (src is Entity)
                SubmodelElement = new Entity(src as Entity);

            // care abuot elements, which could be derived from (not so) abstract base types
            if (src is SubmodelElementList)
                SubmodelElement = new SubmodelElementList(src as SubmodelElementList);
            else if (src is SubmodelElementStruct)
                SubmodelElement = new SubmodelElementStruct(src as SubmodelElementStruct);
            else if (src is SubmodelElementCollection)
                SubmodelElement = new SubmodelElementCollection(
                    src as SubmodelElementCollection, shallowCopy: shallowCopy);

            // again
            if (src is AnnotatedRelationshipElement)
                SubmodelElement = new AnnotatedRelationshipElement(src as AnnotatedRelationshipElement);
            else if (src is RelationshipElement)
                SubmodelElement = new RelationshipElement(src as RelationshipElement);

            // again
            if (src is ModelReferenceElement)
                SubmodelElement = new ModelReferenceElement(src as ModelReferenceElement);
            else if (src is GlobalReferenceElement)
                SubmodelElement = new GlobalReferenceElement(src as GlobalReferenceElement);
            else if (src is ReferenceElement)
                SubmodelElement = new ReferenceElement(src as ReferenceElement);

        }
    }
}
