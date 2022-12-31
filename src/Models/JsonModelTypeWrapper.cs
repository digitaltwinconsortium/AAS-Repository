
namespace AdminShell
{
    using System.Runtime.Serialization;

    [DataContract]
    public class JsonModelTypeWrapper
    {
        [DataMember]
        public string Name { get; set; } = string.Empty;

        public JsonModelTypeWrapper(string name = "")
        {
            Name = name;
        }
    }
}
