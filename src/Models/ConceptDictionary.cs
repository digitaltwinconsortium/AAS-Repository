﻿
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml;
    using System.Xml.Serialization;

    [DataContract]
    public class ConceptDictionary : Referable
    {
        [DataMember(Name="conceptDescriptions")]
        [XmlElement(ElementName = "conceptDescriptions")]
        public List<Reference> ConceptDescriptionsRefs { get; set; } = new();
    }
}
