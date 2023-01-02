
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class SubmodelDescriptor : Descriptor
    {
        [DataMember(Name="administration")]
        public AdministrativeInformation Administration { get; set; }

        [DataMember(Name="description")]
        public List<LangString> Description { get; set; }

        [DataMember(Name="idShort")]
        public string IdShort { get; set; }

        [Required]
        [DataMember(Name="identification")]
        public string Identification { get; set; }

        [DataMember(Name="semanticId")]
        public Reference SemanticId { get; set; }
    }
}
