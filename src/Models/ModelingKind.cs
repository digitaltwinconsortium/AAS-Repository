
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Runtime.Serialization;

    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum ModelingKind
    {
        [EnumMember(Value = "Template")]
        TemplateEnum = 0,

        [EnumMember(Value = "Instance")]
        InstanceEnum = 1
    }
}
