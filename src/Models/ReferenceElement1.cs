/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AdminShell
{
    #region AdminShell_V2_0

    public partial class AdminShellV20
    {
        public class ReferenceElement : DataElement
        {
            // for JSON only
            [XmlIgnore]
            [JsonProperty(PropertyName = "modelType")]
            public new JsonModelTypeWrapper JsonModelType
            {
                get { return new JsonModelTypeWrapper(GetElementName()); }
            }

            // members

            public Reference value = new Reference();

            // constructors

            public ReferenceElement() { }

            public ReferenceElement(SubmodelElement src)
                : base(src)
            {
                if (!(src is ReferenceElement re))
                    return;

                if (re.value != null)
                    this.value = new Reference(re.value);
            }

#if !DoNotUseAdminShell
            public ReferenceElement(AdminShell.AdminShellV10.ReferenceElement src)
                : base(src)
            {
                if (src == null)
                    return;

                if (src.value != null)
                    this.value = new Reference(src.value);
            }
#endif

            public static ReferenceElement CreateNew(
                string idShort = null, string category = null, Key semanticIdKey = null)
            {
                var x = new ReferenceElement();
                x.CreateNewLogic(idShort, category, semanticIdKey);
                return (x);
            }

            public void Set(Reference value = null)
            {
                this.value = value;
            }

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("ReferenceElement", "Ref");
            }

            public override object ToValueOnlySerialization()
            {
                var output = new Dictionary<string, List<Dictionary<string, string>>>();

                var list = new List<Dictionary<string, string>>();
                foreach (var key in value.Keys)
                {
                    var valueDict = new Dictionary<string, string>
                    {
                        { "Type", key.Type },
                        { "Value", key.Value }
                    };
                    list.Add(valueDict);
                }

                output.Add(idShort, list);
                return output;
            }
        }



    }

    #endregion
}

