#define UseAasxCompatibilityModels

using System.Collections.Generic;
using System.Xml;
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
        [XmlType(TypeName = "langString", Namespace = "http://www.admin-shell.io/1/0")]
        public class LangStr
        {

            // members

            [XmlAttribute(Namespace = "http://www.admin-shell.io/1/0")]
            [JsonProperty(PropertyName = "language")]
            public string lang = "";
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

            public static LangStr CreateNew(string lang, string str)
            {
                var l = new LangStr();
                l.lang = lang;
                l.str = str;
                return (l);
            }

            public static List<LangStr> CreateManyFromStringArray(string[] s)
            {
                var r = new List<LangStr>();
                var i = 0;
                while ((i + 1) < s.Length)
                {
                    r.Add(LangStr.CreateNew(s[i], s[i + 1]));
                    i += 2;
                }
                return r;
            }
        }

    }

    #endregion
}

#endif