#define UseAasxCompatibilityModels

using Newtonsoft.Json;

/* Copyright (c) 2018-2019 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>, author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/


#if UseAasxCompatibilityModels

namespace AasxCompatibilityModels
{

    #region Utils

    #endregion


    #region AdminShell_V1_0

    public partial class AdminShellV10
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

    #endregion
}

#endif