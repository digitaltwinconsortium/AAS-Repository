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
        [XmlType(TypeName = "derivedFrom")]
        public class AssetAdministrationShellRef : ModelReference
        {
            // constructors

            public AssetAdministrationShellRef() : base() { }

            public AssetAdministrationShellRef(Key k) : base(k) { }

            public AssetAdministrationShellRef(ModelReference src) : base(src) { }

#if !DoNotUseAasxCompatibilityModels
            public AssetAdministrationShellRef(AasxCompatibilityModels.AdminShellV10.Reference src) : base(src) { }

            public AssetAdministrationShellRef(AasxCompatibilityModels.AdminShellV20.Reference src) : base(src) { }
#endif

            // further methods

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("AssetAdministrationShellRef", "AasRef");
            }
        }

    }
}
