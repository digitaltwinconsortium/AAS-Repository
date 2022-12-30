/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using System.Collections.Generic;

namespace AasxCompatibilityModels
{
    #region AdminShell_V2_0

    public partial class AdminShellV20
    {
        public class ListOfLangStr : List<LangStr>
        {
            public ListOfLangStr() { }

            public ListOfLangStr(ListOfLangStr src)
            {
                if (src != null)
                    foreach (var ls in src)
                        this.Add(ls);
            }

            public string GetDefaultStr(string defaultLang = null)
            {
                // start
                if (defaultLang == null)
                    defaultLang = LangStr.LANG_DEFAULT;
                defaultLang = defaultLang.Trim().ToLower();
                string res = null;

                // search
                foreach (var ls in this)
                    if (ls.lang.Trim().ToLower() == defaultLang)
                        res = ls.str;
                if (res == null && this.Count > 0)
                    res = this[0].str;

                // found?
                return res;
            }

            public bool AllLangSameString()
            {
                if (this.Count < 2)
                    return true;

                for (int i = 1; i < this.Count; i++)
                    if (this[0]?.str != null && this[0]?.str?.Trim() != this[i]?.str?.Trim())
                        return false;

                return true;
            }
        }



    }

    #endregion
}

