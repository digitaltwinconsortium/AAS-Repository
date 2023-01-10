
namespace AdminShell
{
    using Newtonsoft.Json;

    public class JsonValueType
    {
        public class JsonDataObjectType
        {
            [JsonProperty(PropertyName = "name")]
            public string Name = string.Empty;
        }

        [JsonProperty(PropertyName = "dataObjectType")]
        public JsonDataObjectType DataObjectType = new JsonDataObjectType();

        public JsonValueType(string name)
        {
            DataObjectType.Name = name;
        }
    }
}

