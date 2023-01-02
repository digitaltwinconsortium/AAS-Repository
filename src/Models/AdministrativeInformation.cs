
namespace AdminShell
{
    using System.Runtime.Serialization;

    [DataContract]
    public partial class AdministrativeInformation
    {
        public AdministrativeInformation(AdministrativeInformation administration)
        {
            Revision= administration.Revision;
            Version= administration.Version;
        }

        [DataMember(Name="Revision")]
        public string Revision { get; set; }

        [DataMember(Name="Version")]
        public string Version { get; set; }
    }
}
