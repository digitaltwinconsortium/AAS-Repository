
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class PackagesBody
    {
        [DataMember(Name="aasIds")]
        public List<string> AasIds { get; set; }

        [DataMember(Name="file")]
        public byte[] File { get; set; }

        [DataMember(Name="fileName")]
        public string FileName { get; set; }
    }
}
