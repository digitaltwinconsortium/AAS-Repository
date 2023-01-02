
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class Referable : HasExtensions
    {
        [DataMember(Name="category")]
        public string Category { get; set; }

        [DataMember(Name="description")]
        public List<LangString> Description { get; set; }

        [DataMember(Name="displayName")]
        public string DisplayName { get; set; }

        [Required]
        [DataMember(Name="idShort")]
        public string IdShort { get; set; }

        [Required]
        [DataMember(Name="modelType")]
        public ModelType ModelType { get; set; }
    }
}
