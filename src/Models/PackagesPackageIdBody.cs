
namespace AdminShell
{
    using System.Runtime.Serialization;
    using System.Collections.Generic;

    [DataContract]
    public class PackagesPackageIdBody
    {
        [DataMember(Name="aasIds")]
        public List<string> AasIds { get; set; }

        [DataMember(Name="file")]
        public byte[] File { get; set; }

        [DataMember(Name="fileName")]
        public string FileName { get; set; }
    }
}
