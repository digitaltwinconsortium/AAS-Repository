/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;


//namespace AdminShell
namespace AdminShell_V30
{
    public partial class AdminShellV30
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
            // TODO (MIHO, 2022-01-03): check if default to null??
            public ModelReference observed = new ModelReference();

            // constructors

            public BasicEvent() { }

            public BasicEvent(SubmodelElement src)
                : base(src)
            {
                if (!(src is BasicEvent be))
                    return;

                if (be.observed != null)
                    this.observed = new ModelReference(be.observed);
            }

#if !DoNotUseAasxCompatibilityModels
            // not available in V1.0

            public BasicEvent(AasxCompatibilityModels.AdminShellV20.BasicEvent src)
                : base(src)
            {
                if (src.observed != null)
                    this.observed = new ModelReference(src.observed);
            }
#endif

            public static BasicEvent CreateNew(
                string idShort = null, string category = null, Identifier semanticIdKey = null)
            {
                var x = new BasicEvent();
                x.CreateNewLogic(idShort, category, semanticIdKey);
                return (x);
            }

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("BasicEvent", "Evt",
                    SubmodelElementWrapper.AdequateElementEnum.BasicEvent);
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
}
