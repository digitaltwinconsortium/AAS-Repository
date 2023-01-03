
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class AnnotatedRelationshipElement : RelationshipElement
    {
        [DataMember(Name = "annotation")]
        public List<DataElement> Annotation { get; set; } = new();

        // do NOT count children!
        [XmlArray("annotations")]
        [XmlArrayItem("dataElement")]
        public SubmodelElementWrapperCollection<DataElement> annotations = new();
    }
}
