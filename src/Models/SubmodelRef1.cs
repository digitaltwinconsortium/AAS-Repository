/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using System.Xml.Serialization;

namespace AdminShell
{
    #region AdminShell_V2_0

    public partial class AdminShellV20
    {
        [XmlType(TypeName = "submodelRef")]
        public class SubmodelRef : Reference
        {
            // constructors

            public SubmodelRef() : base() { }

            public SubmodelRef(SubmodelRef src) : base(src) { }

            public SubmodelRef(Reference src) : base(src) { }

#if !DoNotUseAdminShell
            public SubmodelRef(AdminShell.AdminShellV10.SubmodelRef src) : base(src) { }
#endif

            public new static SubmodelRef CreateNew(string type, bool local, string idType, string value)
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

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("SubmodelRef", "SMRef");
            }
        }



    }

    #endregion
}

