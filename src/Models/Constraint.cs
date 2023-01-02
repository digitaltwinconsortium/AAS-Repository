
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class Constraint
    {
        [Required]
        [DataMember(Name = "modelType")]
        public ModelType ModelType { get; set; } = new();
    }
}
