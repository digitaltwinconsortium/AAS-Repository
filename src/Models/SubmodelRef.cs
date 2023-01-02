#define UseAdminShell

using System.Xml.Serialization;

/* Copyright (c) 2018-2019 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>, author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/


#if UseAdminShell

namespace AdminShell
{

    #region Utils

    #endregion


    #region AdminShell_V1_0

    public partial class AdminShellV10
    {
        [XmlType(TypeName = "submodelRef")]
        public class SubmodelRef : Reference
        {
            // constructors

            public SubmodelRef() : base() { }

            public SubmodelRef(SubmodelRef src) : base(src) { }

            public static new SubmodelRef CreateNew(string type, bool local, string idType, string value)
            {
                var r = new SubmodelRef();
                r.Keys.Add(Key.CreateNew(type, local, idType, value));
                return r;
            }

            public static SubmodelRef CreateNew(Reference src)
            {
                if (src == null || src.Keys == null)
                    return null;
                var r = new SubmodelRef();
                r.Keys.AddRange(src.Keys);
                return r;
            }

            // further methods

            public override string GetElementName()
            {
                return "SubmodelRef";
            }
        }

    }

    #endregion
}

#endif