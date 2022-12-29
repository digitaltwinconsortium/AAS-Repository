/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using System.Xml.Serialization;

namespace AasxCompatibilityModels
{
    #region AdminShell_V2_0

    public partial class AdminShellV20
    {
        [XmlType(TypeName = "dataSpecificationRef")]
        public class DataSpecificationRef : Reference
        {
            // constructors

            public DataSpecificationRef() : base() { }

            public DataSpecificationRef(DataSpecificationRef src) : base(src) { }

            public DataSpecificationRef(Reference src) : base(src) { }

#if !DoNotUseAasxCompatibilityModels
            public DataSpecificationRef(AasxCompatibilityModels.AdminShellV10.DataSpecificationRef src) : base(src) { }

            public DataSpecificationRef(AasxCompatibilityModels.AdminShellV10.Reference src) : base(src) { }
#endif

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

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("DataSpecificationRef", "DSRef");
            }

        }



    }

    #endregion
}

