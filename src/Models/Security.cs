
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Security.Cryptography.X509Certificates;
    using System.Xml.Serialization;

    [DataContract]
    public class Security
    {
        [Required]
        [DataMember(Name="accessControlPolicyPoints")]
        [XmlElement(ElementName = "accessControlPolicyPoints")]
        public AccessControlPolicyPoints AccessControlPolicyPoints { get; set; }

        [DataMember(Name="certificate")]
        [XmlElement(ElementName = "certificate")]
        public X509Certificate Certificate { get; set; }

        [DataMember(Name="requiredCertificateExtension")]
        [XmlElement(ElementName = "requiredCertificateExtension")]
        public List<Reference> RequiredCertificateExtension { get; set; }
    }
}
