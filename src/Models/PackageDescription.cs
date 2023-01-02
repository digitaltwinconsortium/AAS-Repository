
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class PackageDescription
    {
        [DataMember(Name="aasIds")]
        public List<string> AasIds { get; set; }

        [DataMember(Name="packageId")]
        public string PackageId { get; set; }
    }
}
