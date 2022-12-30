/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/


//namespace AdminShellNS
namespace AdminShell_V30
{

    #region AdminShell_V3_0

    public partial class AdminShellV30
    {
        /// <summary>
        /// Use by <c>FindAllReference</c> to provide a enumeration of referenced with location
        /// info, where they are contained
        /// </summary>
        public class LocatedReference
        {
            public Identifiable Identifiable;

            // ReSharper disable once MemberHidesStaticFromOuterClass
            public ModelReference Reference;

            public LocatedReference() { }
            public LocatedReference(Identifiable identifiable, ModelReference reference)
            {
                Identifiable = identifiable;
                Reference = reference;
            }
        }

    }

    #endregion
}

