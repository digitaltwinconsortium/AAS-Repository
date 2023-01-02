
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class ObjectAttributes
    {
        [DataMember(Name="objectAttribute")]
        public List<Property> ObjectAttribute { get; set; }
    }
}
