/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;
using System.Xml.Serialization;

//namespace AdminShell
namespace AdminShell_V30
{

    #region AdminShell_V3_0

    public partial class AdminShellV30
    {
        //
        // Kinds
        //

        public class AssetKind
        {
            // constants
            public static string Type = "Type";
            public static string Instance = "Instance";

            [MetaModelName("AssetKind.kind")]
            
            [XmlText]
            
            public string kind = "Instance";

            // getters / setters

            [XmlIgnore]
            
            public bool IsInstance { get { return kind == null || kind.Trim().ToLower() == "instance"; } }

            [XmlIgnore]
            
            public bool IsType { get { return kind != null && kind.Trim().ToLower() == "type"; } }

            // constructors / creators

            public AssetKind() { }

            public AssetKind(AssetKind src)
            {
                kind = src.kind;
            }

#if !DoNotUseAasxCompatibilityModels
            public AssetKind(AasxCompatibilityModels.AdminShellV10.Kind src)
            {
                kind = src.kind;
            }

            public AssetKind(AasxCompatibilityModels.AdminShellV20.AssetKind src)
            {
                kind = src.kind;
            }
#endif

            public AssetKind(string kind)
            {
                this.kind = kind;
            }

            public static AssetKind CreateAsType()
            {
                var res = new AssetKind() { kind = AssetKind.Type };
                return res;
            }

            public static AssetKind CreateAsInstance()
            {
                var res = new AssetKind() { kind = AssetKind.Instance };
                return res;
            }
        }

    }

    #endregion
}

