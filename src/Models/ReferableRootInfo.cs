/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/



namespace AdminShell
{
    public partial class AdminShellV30
    {
        //
        // AAS ENV
        //

        /// <summary>
        /// Result of FindReferable in Environment
        /// </summary>
        public class ReferableRootInfo
        {
            public AdministrationShell AAS = null;
            public AssetInformation Asset = null;
            public Submodel Submodel = null;

            public int NrOfRootKeys = 0;

            public bool IsValid
            {
                get
                {
                    return NrOfRootKeys > 0 && (AAS != null || Submodel != null || Asset != null);
                }
            }
        }

    }
}
