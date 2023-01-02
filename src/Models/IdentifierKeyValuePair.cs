
namespace AdminShell
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class IdentifierKeyValuePair : HasSemantics, IAasElement
    {
        [Required]
        [DataMember(Name = "key")]
        [MetaModelName("IdentifierKeyValuePair.key")]
        public string Key { get; set; }

        [Required]
        [DataMember(Name = "Value")]
        [MetaModelName("IdentifierKeyValuePair.Value")]
        public string Value { get; set; }

        [Required]
        [DataMember(Name = "subjectId")]
        public Reference SubjectId { get; set; }

        [Required]
        [DataMember(Name = "externalSubjectId")]
        public GlobalReference ExternalSubjectId { get; set; }

        public IdentifierKeyValuePair() { }

        public IdentifierKeyValuePair(IdentifierKeyValuePair src)
        {
            if (src.SemanticId != null)
                SemanticId = new SemanticId(src.SemanticId);

            Key = src.Key;
            Value = src.Value;

            if (src.ExternalSubjectId != null)
                ExternalSubjectId = new GlobalReference(src.ExternalSubjectId);
        }

        public AasElementSelfDescription GetSelfDescription()
        {
            return new AasElementSelfDescription("IdentifierKeyValuePair", "IKV");
        }

        public string GetElementName()
        {
            return this.GetSelfDescription()?.ElementName;
        }
    }
}
