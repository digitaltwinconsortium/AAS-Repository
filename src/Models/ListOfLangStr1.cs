/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

//namespace AdminShellNS
namespace AdminShell_V30
{

    #region AdminShell_V3_0

    public partial class AdminShellV30
    {
        public class ListOfLangStr : List<LangStr>
        {
            public ListOfLangStr() { }

            public ListOfLangStr(LangStr ls)
            {
                if (ls != null)
                    this.Add(ls);
            }

            public ListOfLangStr(ListOfLangStr src)
            {
                if (src != null)
                    foreach (var ls in src)
                        this.Add(ls);
            }

            public string this[string lang]
            {
                get
                {
                    return GetDefaultStr(lang);
                }
                set
                {
                    foreach (var ls in this)
                        if (ls.lang.Trim().ToLower() == lang?.Trim().ToLower())
                        {
                            ls.str = value;
                            return;
                        }
                    this.Add(new LangStr(lang, value));
                }
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

            public override string ToString()
            {
                return string.Join(", ", this.Select((ls) => ls.ToString()));
            }

            public static ListOfLangStr Parse(string cell)
            {
                // access
                if (cell == null)
                    return null;

                // iterative approach
                var res = new ListOfLangStr();
                while (true)
                {
                    // trivial case and finite end
                    if (!cell.Contains("@"))
                    {
                        if (cell.Trim() != "")
                            res.Add(new LangStr(LangStr.LANG_DEFAULT, cell));
                        break;
                    }

                    // OK, pick the next couple
                    var m = Regex.Match(cell, @"(.*?)@(\w+)", RegexOptions.Singleline);
                    if (!m.Success)
                    {
                        // take emergency exit?
                        res.Add(new LangStr("??", cell));
                        break;
                    }

                    // use the match and shorten cell ..
                    res.Add(new LangStr(m.Groups[2].ToString(), m.Groups[1].ToString().Trim()));
                    cell = cell.Substring(m.Index + m.Length);
                }

                return res;
            }
        }

    }

    #endregion
}

