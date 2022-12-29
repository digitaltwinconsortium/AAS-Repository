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
        [XmlType(TypeName = "derivedFrom")]
        public class AssetAdministrationShellRef : Reference
        {
            // constructors

            public AssetAdministrationShellRef() : base() { }

            public AssetAdministrationShellRef(Key k) : base(k) { }

            public AssetAdministrationShellRef(Reference src) : base(src) { }

#if !DoNotUseAasxCompatibilityModels
            public AssetAdministrationShellRef(AasxCompatibilityModels.AdminShellV10.Reference src) : base(src) { }
#endif

            // further methods

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("AssetAdministrationShellRef", "AasRef");
            }
        }



    }

    #endregion
}

