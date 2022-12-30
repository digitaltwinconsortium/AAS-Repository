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
        public class LangStringIEC61360
        {

            // members

            [XmlElement(ElementName = "langString", Namespace = "http://www.admin-shell.io/aas/1/0")]
            public List<LangStr> langString = new List<LangStr>();

            // getters / setters

            [XmlIgnore]
            
            public bool IsEmpty { get { return langString == null || langString.Count < 1; } }
            [XmlIgnore]
            
            public int Count { get { if (langString == null) return 0; return langString.Count; } }
            [XmlIgnore]
            
            public LangStr this[int index] { get { return langString[index]; } }

            // constructors

            public LangStringIEC61360() { }

            public LangStringIEC61360(LangStringIEC61360 src)
            {
                if (src.langString != null)
                    foreach (var ls in src.langString)
                        this.langString.Add(new LangStr(ls));
            }

            // converter

            public static LangStringIEC61360 CreateFrom(List<LangStr> src)
            {
                var res = new LangStringIEC61360();
                if (src != null)
                    foreach (var ls in src)
                        res.langString.Add(new LangStr(ls));
                return res;
            }

        }

    }

    #endregion
}

#endif