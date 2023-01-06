
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum KindOfPermissionEnum
    {
        [EnumMember(Value = "Allow")]
        [XmlEnum(Name ="Allow")]
        Allow = 0,

        [EnumMember(Value = "Deny")]
        [XmlEnum(Name = "Deny")]
        Deny = 1,

        [EnumMember(Value = "NotApplicable")]
        [XmlEnum(Name = "NotApplicable")]
        NotApplicable = 2,

        [EnumMember(Value = "Undefined")]
        [XmlEnum(Name = "Undefined")]
        Undefined = 3
    }
}
