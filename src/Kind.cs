#define UseAasxCompatibilityModels

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
        public class Kind
        {
            [XmlText]
            public string kind = "Instance";

            // getters / setters

            [XmlIgnore]
            
            public bool IsInstance { get { return kind == null || kind.Trim().ToLower() == "instance"; } }

            [XmlIgnore]
            
            public bool IsType { get { return kind != null && kind.Trim().ToLower() == "type"; } }

            // constructors / creators

            public Kind() { }

            public Kind(Kind src)
            {
                kind = src.kind;
            }

            public Kind(string kind)
            {
                this.kind = kind;
            }

            public static Kind CreateFrom(Kind k)
            {
                var res = new Kind();
                res.kind = k.kind;
                return res;
            }

            public static Kind CreateAsType()
            {
                var res = new Kind();
                res.kind = "Type";
                return res;
            }

            public static Kind CreateAsInstance()
            {
                var res = new Kind();
                res.kind = "Instance";
                return res;
            }
        }

    }

    #endregion
}

#endif