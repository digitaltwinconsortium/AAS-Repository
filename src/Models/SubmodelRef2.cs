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
        [XmlType(TypeName = "submodelRef")]
        public class SubmodelRef : ModelReference
        {
            // constructors

            public SubmodelRef() : base() { }

            public SubmodelRef(SubmodelRef src) : base(src) { }

            public SubmodelRef(ModelReference src) : base(src) { }

#if !DoNotUseAasxCompatibilityModels
            public SubmodelRef(AasxCompatibilityModels.AdminShellV10.SubmodelRef src) : base(src) { }

            public SubmodelRef(AasxCompatibilityModels.AdminShellV20.SubmodelRef src) : base(src) { }
#endif

            public new static SubmodelRef CreateNew(string type, string value)
            {
                var r = new SubmodelRef();
                r.Keys.Add(Key.CreateNew(type, value));
                return r;
            }

            public static SubmodelRef CreateNew(ModelReference src)
            {
                if (src == null || src.Keys == null)
                    return null;
                var r = new SubmodelRef();
                r.Keys.AddRange(src.Keys);
                return r;
            }

            // further methods

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("SubmodelRef", "SMRef");
            }
        }

    }
}
