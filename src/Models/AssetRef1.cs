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
        [XmlType(TypeName = "assetRef")]
        public class AssetRef : Reference
        {
            // constructors

            public AssetRef() : base() { }

            public AssetRef(AssetRef src) : base(src) { }

#if !DoNotUseAasxCompatibilityModels
            public AssetRef(AasxCompatibilityModels.AdminShellV10.AssetRef src) : base(src) { }
#endif

            public AssetRef(Reference r)
                : base(r)
            {
            }

            // further methods

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("AssetRef", "AssetRef");
            }
        }



    }

    #endregion
}

