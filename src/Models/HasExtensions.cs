
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public partial class HasExtensions
    {
        [DataMember(Name="extensions")]
        public List<Extension> Extensions { get; set; }
    }
}
