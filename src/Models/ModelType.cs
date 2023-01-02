
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class ModelType
    {
        [Required]
        [DataMember(Name="name")]
        public ModelTypes Name { get; set; }
    }
}
