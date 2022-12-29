#define UseAasxCompatibilityModels

using System.Xml.Serialization;

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
        [XmlType(TypeName = "dataSpecificationRef")]
        public class DataSpecificationRef : Reference
        {
            // constructors

            public DataSpecificationRef() : base() { }

            public DataSpecificationRef(DataSpecificationRef src) : base(src) { }

            // further methods

            public static DataSpecificationRef CreateNew(Reference src)
            {
                if (src == null || src.Keys == null)
                    return null;
                var res = new DataSpecificationRef();
                foreach (var k in src.Keys)
                    res.Keys.Add(new Key(k));
                return res;
            }

            public override string GetElementName()
            {
                return "DataSpecificationRef";
            }
        }

    }

    #endregion
}

#endif