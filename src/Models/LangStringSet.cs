/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace AasxCompatibilityModels
{
    #region AdminShell_V2_0

    public partial class AdminShellV20
    {
        /// <summary>
        /// Multiple lang string for the AAS namespace
        /// </summary>
        public class LangStringSet
        {

            // members

            [XmlElement(ElementName = "langString", Namespace = "http://www.admin-shell.io/aas/2/0")]
            public ListOfLangStr langString = new ListOfLangStr();

            // getters / setters

            [XmlIgnore]
            
            public bool IsEmpty { get { return langString == null || langString.Count < 1; } }
            [XmlIgnore]
            
            public int Count { get { if (langString == null) return 0; return langString.Count; } }
            [XmlIgnore]
            
            public LangStr this[int index] { get { return langString[index]; } }

            // constructors

            public LangStringSet() { }

            public LangStringSet(LangStringSet src)
            {
                if (src.langString != null)
                    foreach (var ls in src.langString)
                        this.langString.Add(new LangStr(ls));
            }

            public LangStringSet(ListOfLangStr src)
            {
                if (src != null)
                    foreach (var ls in src)
                        this.langString.Add(new LangStr(ls));
            }

#if !DoNotUseAasxCompatibilityModels
            // not available in V1.0
#endif
            public LangStringSet(string lang, string str)
            {
                if (str == null || str.Trim() == "")
                    return;
                this.langString.Add(new LangStr(lang, str));
            }

            // converter

            public static LangStringSet CreateFrom(List<LangStr> src)
            {
                var res = new LangStringSet();
                if (src != null)
                    foreach (var ls in src)
                        res.langString.Add(new LangStr(ls));
                return res;
            }

            // add

            public LangStr Add(LangStr ls)
            {
                this.langString.Add(ls);
                return ls;
            }

            public LangStr Add(string lang, string str)
            {
                var ls = new LangStr(lang, str);
                this.langString.Add(ls);
                return ls;
            }

            // single string representation
            public string GetDefaultStr(string defaultLang = null)
            {
                return this.langString?.GetDefaultStr(defaultLang);
            }
        }



    }

    #endregion
}

