/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;
using System.Xml.Serialization;

//namespace AdminShell
namespace AdminShell_V30
{

    #region AdminShell_V3_0

    public partial class AdminShellV30
    {
#if !DoNotUseAasxCompatibilityModels
#endif

        //
        // Lang Str
        //

        [XmlType(TypeName = "langString", Namespace = "http://www.admin-shell.io/3/0")]
        public class LangStr
        {
            // constants
            public static string LANG_DEFAULT = "en";

            // members

            [MetaModelName("LangStr.lang")]
            
            [XmlAttribute(Namespace = "http://www.admin-shell.io/3/0")]
            [JsonProperty(PropertyName = "language")]
            
            public string lang = "";

            [MetaModelName("LangStr.str")]
            
            [XmlText]
            [JsonProperty(PropertyName = "text")]
            
            public string str = "";

            // constructors

            public LangStr() { }

            public LangStr(LangStr src)
            {
                this.lang = src.lang;
                this.str = src.str;
            }

#if !DoNotUseAasxCompatibilityModels
            public LangStr(AasxCompatibilityModels.AdminShellV10.LangStr src)
            {
                this.lang = src.lang;
                this.str = src.str;
            }

            public LangStr(AasxCompatibilityModels.AdminShellV20.LangStr src)
            {
                this.lang = src.lang;
                this.str = src.str;
            }
#endif

            public LangStr(string lang, string str)
            {
                this.lang = lang;
                this.str = str;
            }

            public static ListOfLangStr CreateManyFromStringArray(string[] s)
            {
                var r = new ListOfLangStr();
                var i = 0;
                while ((i + 1) < s.Length)
                {
                    r.Add(new LangStr(s[i], s[i + 1]));
                    i += 2;
                }
                return r;
            }

            public override string ToString()
            {
                return $"{str}@{lang}";
            }
        }

    }

    #endregion
}

