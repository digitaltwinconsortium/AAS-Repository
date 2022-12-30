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
        public class MultiLanguageProperty : DataElement
        {
            // for JSON only
            [XmlIgnore]
            [JsonProperty(PropertyName = "modelType")]
            public new JsonModelTypeWrapper JsonModelType
            {
                get { return new JsonModelTypeWrapper(GetElementName()); }
            }

            // members

            public LangStringSet value = new LangStringSet();
            public Reference valueId = null;

            // constructors

            public MultiLanguageProperty() { }

            public MultiLanguageProperty(SubmodelElement src)
                : base(src)
            {
                if (!(src is MultiLanguageProperty mlp))
                    return;

                this.value = new LangStringSet(mlp.value);
                if (mlp.valueId != null)
                    valueId = new Reference(mlp.valueId);
            }

#if !DoNotUseAasxCompatibilityModels
            // not available in V1.0
#endif

            public static MultiLanguageProperty CreateNew(
                string idShort = null, string category = null, Key semanticIdKey = null)
            {
                var x = new MultiLanguageProperty();
                x.CreateNewLogic(idShort, category, semanticIdKey);
                return (x);
            }

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("MultiLanguageProperty", "MLP");
            }

            public MultiLanguageProperty Set(LangStringSet ls)
            {
                this.value = ls;
                return this;
            }

            public MultiLanguageProperty Set(ListOfLangStr ls)
            {
                this.value = new LangStringSet(ls);
                return this;
            }

            public MultiLanguageProperty Set(LangStr ls)
            {
                if (this.value == null)
                    this.value = new LangStringSet();
                this.value.Add(ls);
                return this;
            }

            public MultiLanguageProperty Set(string lang, string str)
            {
                return this.Set(new LangStr(lang, str));
            }

            public override string ValueAsText(string defaultLang = null)
            {
                return "" + value?.GetDefaultStr(defaultLang);
            }

            public override object ToValueOnlySerialization()
            {
                var output = new Dictionary<string, Dictionary<string, string>>();
                var valueDict = new Dictionary<string, string>();
                foreach (LangStr langStr in value.langString)
                {
                    valueDict.Add(langStr.lang, langStr.str);
                }

                output.Add(idShort, valueDict);
                return output;
            }
        }



    }

    #endregion
}

