
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public partial class ModelType
    {
        [Required]
        [DataMember(Name="name")]
        public ModelTypes Name { get; set; }
    }
}
