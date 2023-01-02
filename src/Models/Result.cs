
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class Result
    {
        [DataMember(Name="messages")]
        public List<Message> Messages { get; set; }

        [DataMember(Name="success")]
        public bool? Success { get; set; }
    }
}
