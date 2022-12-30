/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using System.Xml.Serialization;

//namespace AdminShellNS
namespace AdminShell_V30
{
    public partial class AdminShellV30
    {
        [XmlType(TypeName = "dataSpecificationRef")]
        public class DataSpecificationRef : GlobalReference
        {
            // constructors

            public DataSpecificationRef() : base() { }

            public DataSpecificationRef(DataSpecificationRef src) : base(src) { }
            public DataSpecificationRef(GlobalReference src) : base(src) { }
            public DataSpecificationRef(Identifier src) : base(src) { }

#if !DoNotUseAasxCompatibilityModels
            public DataSpecificationRef(AasxCompatibilityModels.AdminShellV10.DataSpecificationRef src) : base(src) { }

            public DataSpecificationRef(AasxCompatibilityModels.AdminShellV10.Reference src) : base(src) { }

            public DataSpecificationRef(AasxCompatibilityModels.AdminShellV20.DataSpecificationRef src) : base(src) { }
#endif

            // further methods

            public static DataSpecificationRef CreateNew(GlobalReference src)
            {
                return new DataSpecificationRef(src);
            }

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("DataSpecificationRef", "DSRef");
            }

        }

    }
}
