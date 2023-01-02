
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class Descriptor
    {
        [DataMember(Name="endpoints")]
        public List<Endpoint> Endpoints { get; set; }
    }
}
