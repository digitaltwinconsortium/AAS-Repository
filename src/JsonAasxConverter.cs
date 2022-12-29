#define UseAasxCompatibilityModels

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        //
        // Handling of packages
        //

        /// <summary>
        /// This converter is used for reading JSON files; it claims to be responsible for "SubmodelElements" (the base class)
        /// and decides, which sub-class of the base class shall be populated. 
        /// The decision, shich special sub-class to create is done in a factory AdminShell.SubmodelElementWrapper.CreateAdequateType(),
        /// in order to have all sub-class specific decisions in one place (SubmodelElementWrapper)
        /// Remark: There is a NuGet package JsonSubTypes, which could have done the job, except the fact of having
        /// "modelType" being a class property with a contained property "name".
        /// </summary>
        public class JsonAasxConverter : JsonConverter
        {
            private string UpperClassProperty = "modelType";
            private string LowerClassProperty = "name";

            public JsonAasxConverter() : base()
            {
            }

            public JsonAasxConverter(string UpperClassProperty, string LowerClassProperty) : base()
            {
                this.UpperClassProperty = UpperClassProperty;
                this.LowerClassProperty = LowerClassProperty;
            }

            public override bool CanConvert(Type objectType)
            {
                if (typeof(AdminShellV10.SubmodelElement).IsAssignableFrom(objectType))
                    return true;
                return false;
            }

            public override bool CanWrite
            {
                get { return false; }
            }

            public override object ReadJson(JsonReader reader,
                                            Type objectType,
                                             object existingValue,
                                             JsonSerializer serializer)
            {
                // Load JObject from stream
                JObject jObject = JObject.Load(reader);

                // Create target object based on JObject
                object target = new AdminShellV10.SubmodelElement();

                if (jObject.ContainsKey(UpperClassProperty))
                {
                    var j2 = jObject[UpperClassProperty];
                    foreach (var c in j2.Children())
                    {
                        var cprop = c as Newtonsoft.Json.Linq.JProperty;
                        if (cprop == null)
                            continue;
                        if (cprop.Name == LowerClassProperty && cprop.Value != null && cprop.Value.Type.ToString() == "String")
                        {
                            var cpval = cprop.Value.ToObject<string>();
                            if (cpval == null)
                                continue;
                            var o = AdminShellV10.SubmodelElementWrapper.CreateAdequateType(cpval);
                            if (o != null)
                                target = o;
                        }
                    }
                }

                // Populate the object properties
                serializer.Populate(jObject.CreateReader(), target);

                return target;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }

    }

    #endregion
}

#endif