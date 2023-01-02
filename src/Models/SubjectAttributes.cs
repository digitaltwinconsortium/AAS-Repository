
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class SubjectAttributes
    {
        [DataMember(Name="subjectAttributes")]
        public List<Reference> _SubjectAttributes { get; set; }
    }
}
