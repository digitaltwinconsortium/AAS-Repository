
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum EntityTypeEnum
    {
        [EnumMember(Value = "CoManagedEntity")]
        [XmlEnum(Name = "CoManagedEntity")]
        CoManagedEntity = 0,

        [EnumMember(Value = "SelfManagedEntity")]
        [XmlEnum(Name = "SelfManagedEntity")]
        SelfManagedEntity = 1,

        [EnumMember(Value = "Undefined")]
        [XmlEnum(Name = "Undefined")]
        Undefined = 3
    }
}
