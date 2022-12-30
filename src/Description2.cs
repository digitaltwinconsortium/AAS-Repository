/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;
using System.Xml.Serialization;

//namespace AdminShellNS
namespace AdminShell_V30
{

    #region AdminShell_V3_0

    public partial class AdminShellV30
    {
        //
        // Description
        //

        public class Description
        {

            // members

            [XmlElement(ElementName = "langString")]
            public ListOfLangStr langString = new ListOfLangStr();

            // constructors

            public Description() { }

            public Description(Description src)
            {
                if (src != null && src.langString != null)
                    foreach (var ls in src.langString)
                        langString.Add(new LangStr(ls));
            }

            public Description(LangStringSet src)
            {
                if (src != null && src.langString != null)
                    foreach (var ls in src.langString)
                        langString.Add(new LangStr(ls));
            }

#if !DoNotUseAasxCompatibilityModels
            public Description(AasxCompatibilityModels.AdminShellV10.Description src)
            {
                if (src != null)
                    foreach (var ls in src.langString)
                        langString.Add(new LangStr(ls));
            }

            public Description(AasxCompatibilityModels.AdminShellV20.Description src)
            {
                if (src != null && src.langString != null)
                    foreach (var ls in src.langString)
                        langString.Add(new LangStr(ls));
            }
#endif

            // further
            [XmlIgnore]
            
            public bool IsValid { get { return langString != null && langString.Count >= 1; } }

            // single string representation
            public string GetDefaultStr(string defaultLang = null)
            {
                return this.langString?.GetDefaultStr(defaultLang);
            }

        }

    }

    #endregion
}

