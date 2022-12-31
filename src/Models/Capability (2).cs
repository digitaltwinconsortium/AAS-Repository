/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/



//namespace AdminShell
namespace AdminShell_V30
{
    public partial class AdminShellV30
    {
        public class Capability : SubmodelElement
        {
            public Capability() { }

            public Capability(SubmodelElement src)
                : base(src)
            { }

#if !DoNotUseAasxCompatibilityModels
            public Capability(AasxCompatibilityModels.AdminShellV20.Capability src)
                : base(src)
            { }
#endif

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("Capability", "Cap",
                    SubmodelElementWrapper.AdequateElementEnum.Capability);
            }
        }

    }
}
