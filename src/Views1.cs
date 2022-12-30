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
        public class Views
        {
            [XmlElement(ElementName = "view")]
            
            public List<View> views = new List<View>();

            // constructors

            public Views() { }

            public Views(Views src)
            {
                if (src != null && src.views != null)
                    foreach (var v in src.views)
                        this.views.Add(new View(v));
            }

            public static Views CreateOrSetInnerViews(Views outer, View[] inner)
            {
                var res = outer;
                if (res == null)
                    res = new Views();
                if (inner == null)
                {
                    res.views = null;
                    return res;
                }
                res.views = new List<View>(inner);
                return res;
            }
        }

    }

    #endregion
}

#endif