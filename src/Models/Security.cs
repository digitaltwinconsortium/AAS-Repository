
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Security.Cryptography.X509Certificates;

    [DataContract]
    public class Security
    {
        [Required]
        [DataMember(Name="accessControlPolicyPoints")]
        public AccessControlPolicyPoints AccessControlPolicyPoints { get; set; }

        [DataMember(Name="certificate")]
        public List<X509Certificate> Certificate { get; set; }

        [DataMember(Name="requiredCertificateExtension")]
        public List<Reference> RequiredCertificateExtension { get; set; }
    }
}
