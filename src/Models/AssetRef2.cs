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
        //
        // derived References
        //

        [XmlType(TypeName = "assetRef")]
        public class AssetRef : GlobalReference
        {
            // constructors

            public AssetRef() : base() { }
            public AssetRef(AssetRef src) : base(src) { }
            public AssetRef(GlobalReference r) : base(r) { }
            public AssetRef(Identifier id) : base(id) { }

#if !DoNotUseAasxCompatibilityModels
            public AssetRef(AasxCompatibilityModels.AdminShellV10.AssetRef src) : base(src) { }
            public AssetRef(AasxCompatibilityModels.AdminShellV20.AssetRef src) : base(src) { }
#endif


            // further methods

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("AssetRef", "AssetRef");
            }
        }

    }
}
