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
        [XmlType(TypeName = "containedElementRef")]
        public class ContainedElementRef : Reference
        {
            // constructors

            public ContainedElementRef() { }

            public ContainedElementRef(ContainedElementRef src) : base(src) { }

#if !DoNotUseAasxCompatibilityModels
            public ContainedElementRef(AasxCompatibilityModels.AdminShellV10.ContainedElementRef src) : base(src) { }
#endif

            public static ContainedElementRef CreateNew(Reference src)
            {
                if (src == null || src.Keys == null)
                    return null;
                var r = new ContainedElementRef();
                r.Keys.AddRange(src.Keys);
                return r;
            }

            // further methods

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("ContainedElementRef", "CERef");
            }

        }



    }

    #endregion
}

