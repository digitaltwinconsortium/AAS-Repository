/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AasxCompatibilityModels
{
    #region AdminShell_V2_0

    public partial class AdminShellV20
    {
        public class BasicEvent : SubmodelElement
        {
            // for JSON only
            [XmlIgnore]
            [JsonProperty(PropertyName = "modelType")]
            public new JsonModelTypeWrapper JsonModelType
            {
                get { return new JsonModelTypeWrapper(GetElementName()); }
            }

            // from this very class
            public Reference observed = new Reference();

            // constructors

            public BasicEvent() { }

            public BasicEvent(SubmodelElement src)
                : base(src)
            {
                if (!(src is BasicEvent be))
                    return;

                if (be.observed != null)
                    this.observed = new Reference(be.observed);
            }

#if !DoNotUseAasxCompatibilityModels
            // not available in V1.0
#endif

            public static BasicEvent CreateNew(string idShort = null, string category = null, Key semanticIdKey = null)
            {
                var x = new BasicEvent();
                x.CreateNewLogic(idShort, category, semanticIdKey);
                return (x);
            }

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("BasicEvent", "Evt");
            }

            public override object ToValueOnlySerialization()
            {
                var output = new Dictionary<string, object>();
                var list = new List<Dictionary<string, string>>();
                foreach (var key in observed.Keys)
                {
                    var valueDict = new Dictionary<string, string>
                    {
                        { "type", key.type },
                        { "value", key.value }
                    };
                    list.Add(valueDict);
                }

                var observedDict = new Dictionary<string, List<Dictionary<string, string>>>();
                observedDict.Add("observed", list);
                output.Add(idShort, observedDict);

                return output;
            }
        }



    }

    #endregion
}

