/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;


//namespace AdminShell
namespace AdminShell_V30
{
    public partial class AdminShellV30
    {
        public class JsonValueTypeCast
        {

            public class JsonDataObjectType
            {
                [JsonProperty(PropertyName = "name")]
                public string name = "";
            }

            [JsonProperty(PropertyName = "dataObjectType")]
            public JsonDataObjectType dataObjectType = new JsonDataObjectType();

            public JsonValueTypeCast(string name)
            {
                this.dataObjectType.name = name;
            }
        }

    }
}
