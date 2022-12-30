#define UseAasxCompatibilityModels

using System.Xml.Serialization;
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
        public class Blob : DataElement
        {
            // for JSON only
            [XmlIgnore]
            [JsonProperty(PropertyName = "modelType")]
            public new JsonModelTypeWrapper JsonModelType { get { return new JsonModelTypeWrapper(GetElementName()); } }

            // members

            public string mimeType = "";
            public string value = "";

            // constructors

            public Blob() { }

            public Blob(Blob src)
                : base(src)
            {
                this.mimeType = src.mimeType;
                this.value = src.value;
            }

            public static Blob CreateNew(string idShort = null, string category = null, Key semanticIdKey = null)
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

            public override string GetElementName()
            {
                return "Blob";
            }

        }

    }

    #endregion
}

#endif