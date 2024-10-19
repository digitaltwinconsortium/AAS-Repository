
namespace AdminShell
{
    using Opc.Ua;
    using System;
    using System.Collections.Generic;

    public class AasUaUtils
    {
        public static List<string> ToOpcUaReferenceList(Reference refid)
        {
            if (refid == null || refid.Keys == null || refid.Keys.Count == 0)
            {
                return null;
            }

            var res = new List<string>();
            foreach (var k in refid.Keys)
            {
                res.Add(String.Format("{0}", k.Value));
            }

            return res;
        }
      
        public static LocalizedText GetBestUaDescriptionFromAasDescription(List<LangString> desc)
        {
            var res = new LocalizedText("", "");
            if (desc != null && desc != null)
            {
                var found = false;
                foreach (var ls in desc)
                {
                    if (!found || ls.Language.Trim().ToLower().StartsWith("en"))
                    {
                        found = true;
                        res = new LocalizedText(ls.Language, ls.Text);
                    }
                }
            }

            return res;
        }
    }
}
