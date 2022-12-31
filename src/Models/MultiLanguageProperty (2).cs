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
            public GlobalReference valueId = null;

            // constructors

            public MultiLanguageProperty() { }

            public MultiLanguageProperty(SubmodelElement src)
                : base(src)
            {
                if (!(src is MultiLanguageProperty mlp))
                    return;

                this.value = new LangStringSet(mlp.value);
                if (mlp.valueId != null)
                    valueId = new GlobalReference(mlp.valueId);
            }

#if !DoNotUseAasxCompatibilityModels
            // not available in V1.0
            public MultiLanguageProperty(AasxCompatibilityModels.AdminShellV20.MultiLanguageProperty src)
                : base(src)
            {
                this.value = new LangStringSet(src.value);
                if (src.valueId != null)
                    valueId = new GlobalReference(src.valueId);
            }
#endif

            public static MultiLanguageProperty CreateNew(
                string idShort = null, string category = null, Identifier semanticIdKey = null)
            {
                var x = new MultiLanguageProperty();
                x.CreateNewLogic(idShort, category, semanticIdKey);
                return (x);
            }

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("MultiLanguageProperty", "MLP",
                    SubmodelElementWrapper.AdequateElementEnum.MultiLanguageProperty);
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
                if (ls == null)
                    return this;
                if (this.value?.langString == null)
                    this.value = new LangStringSet();
                this.value.langString[ls.lang] = ls.str;
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

            public override void ValueFromText(string text, string defaultLang = null)
            {
                Set(defaultLang, text);
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
}
