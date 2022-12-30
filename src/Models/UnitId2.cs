/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/


//namespace AdminShellNS
namespace AdminShell_V30
{
    public partial class AdminShellV30
    {
        public class UnitId : GlobalReference
        {
            // constructors / creators

            public UnitId() : base() { }
            public UnitId(Identifier id) : base(id) { }
            public UnitId(UnitId src) : base(src) { }

            public UnitId(GlobalReference src) : base(src) { }
            public UnitId(Reference src) : base(src) { }

#if !DoNotUseAasxCompatibilityModels
            public UnitId(AasxCompatibilityModels.AdminShellV10.UnitId src) : base(src?.Keys) { }
            public UnitId(AasxCompatibilityModels.AdminShellV20.UnitId src) : base(src?.Keys) { }
#endif
        }

    }
}
