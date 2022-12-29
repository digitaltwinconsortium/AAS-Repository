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
        /// <summary>
        /// Multiple lang string for the IEC61360 namespace
        /// </summary>
        public class LangStringSetIEC61360 : ListOfLangStr
        {

            // getters / setters

            [XmlIgnore]
            [JsonIgnore]
            public bool IsEmpty { get { return this.Count < 1; } }

            // constructors

            public LangStringSetIEC61360() { }

            public LangStringSetIEC61360(ListOfLangStr lol) : base(lol) { }

            public LangStringSetIEC61360(LangStringSetIEC61360 src)
            {
                foreach (var ls in src)
                    this.Add(new LangStr(ls));
            }

#if !DoNotUseAasxCompatibilityModels
            public LangStringSetIEC61360(AasxCompatibilityModels.AdminShellV10.LangStringIEC61360 src)
            {
                if (src.langString != null)
                    foreach (var ls in src.langString)
                        this.Add(new LangStr(ls));
            }
#endif
            public LangStringSetIEC61360(string lang, string str)
            {
                if (str == null || str.Trim() == "")
                    return;
                this.Add(new LangStr(lang, str));
            }

            // converter

            public static LangStringSetIEC61360 CreateFrom(List<LangStr> src)
            {
                var res = new LangStringSetIEC61360();
                if (src != null)
                    foreach (var ls in src)
                        res.Add(new LangStr(ls));
                return res;
            }

        }



    }

    #endregion
}

