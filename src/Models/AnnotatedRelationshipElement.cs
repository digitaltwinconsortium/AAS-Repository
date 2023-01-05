﻿
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class AnnotatedRelationshipElement : RelationshipElement
    {
        [DataMember(Name = "annotations")]
        [XmlElement(ElementName = "annotations")]
        public List<DataElement> Annotations { get; set; } = new();

        public AnnotatedRelationshipElement() { }

        public AnnotatedRelationshipElement(AnnotatedRelationshipElement src) : base(src)
        {
            Annotations = src.Annotations;
        }
    }
}
