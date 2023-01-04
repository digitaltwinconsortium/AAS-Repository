
namespace AdminShell
{
    using System.Runtime.Serialization;

    [DataContract]
    public partial class Message
    {
        [DataMember(Name="code")]
        public string Code { get; set; }

        [DataMember(Name="messageType")]
        public MessageTypeEnum? MessageType { get; set; }

        [DataMember(Name="text")]
        public string Text { get; set; }

        [DataMember(Name="timestamp")]
        public string Timestamp { get; set; }
    }
}
