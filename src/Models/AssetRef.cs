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
        [XmlType(TypeName = "assetRef")]
        public class AssetRef : Reference
        {
            // constructors

            public AssetRef() : base() { }

            public AssetRef(AssetRef src) : base(src) { }

            // translation

            public static AssetRef CreateNew(Reference r)
            {
                return (AssetRef)new Reference(r);
            }

            // further methods

            public override string GetElementName()
            {
                return "AssetRef";
            }
        }

    }

    #endregion
}

#endif