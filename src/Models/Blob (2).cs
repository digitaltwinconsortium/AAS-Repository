/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;


//namespace AdminShellNS
namespace AdminShell_V30
{
    public partial class AdminShellV30
    {
        public class Blob : DataElement
        {
            // for JSON only
            [XmlIgnore]
            [JsonProperty(PropertyName = "modelType")]
            public new JsonModelTypeWrapper JsonModelType
            {
                get { return new JsonModelTypeWrapper(GetElementName()); }
            }

            // members

            [MetaModelName("Blob.mimeType")]
            
            
            public string mimeType = "";

            [MetaModelName("Blob.value")]
            
            
            public string value = "";

            // constructors

            public Blob() { }

            public Blob(SubmodelElement src)
                : base(src)
            {
                if (!(src is Blob blb))
                    return;

                this.mimeType = blb.mimeType;
                this.value = blb.value;
            }

#if !DoNotUseAasxCompatibilityModels
            public Blob(AasxCompatibilityModels.AdminShellV10.Blob src)
                : base(src)
            {
                if (src == null)
                    return;

                this.mimeType = src.mimeType;
                this.value = src.value;
            }

            public Blob(AasxCompatibilityModels.AdminShellV20.Blob src)
                : base(src)
            {
                if (src == null)
                    return;

                this.mimeType = src.mimeType;
                this.value = src.value;
            }
#endif

            public static Blob CreateNew(string idShort = null, string category = null, Identifier semanticIdKey = null)
            {
                var x = new Blob();
                x.CreateNewLogic(idShort, category, semanticIdKey);
                return (x);
            }

            public void Set(string mimeType = "", string value = "")
            {
                this.mimeType = mimeType;
                this.value = value;
            }

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("Blob", "Blob",
                    SubmodelElementWrapper.AdequateElementEnum.Blob);
            }

            public override object ToValueOnlySerialization()
            {
                var output = new Dictionary<string, Dictionary<string, string>>();
                var valueDict = new Dictionary<string, string>
                {
                    { "mimeType", mimeType },
                };

                output.Add(idShort, valueDict);
                return output;
            }

            public object ToWithBlobOnlyValue()
            {
                var output = new Dictionary<string, Dictionary<string, string>>();
                var valueDict = new Dictionary<string, string>
                {
                    { "mimeType", mimeType },
                    { "value", Convert.ToBase64String(Encoding.UTF8.GetBytes(value)) }
                };

                output.Add(idShort, valueDict);
                return output;
            }

        }

    }
}
