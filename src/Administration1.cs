/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

namespace AasxCompatibilityModels
{
    #region AdminShell_V2_0

    public partial class AdminShellV20
    {
        public class Administration
        {

            // members

            [MetaModelName("Administration.version")]
            [TextSearchable]
            [CountForHash]
            public string version = "";

            [MetaModelName("Administration.revision")]
            [TextSearchable]
            [CountForHash]
            public string revision = "";

            // constructors

            public Administration() { }

            public Administration(Administration src)
            {
                this.version = src.version;
                this.revision = src.revision;
            }

#if !DoNotUseAasxCompatibilityModels
            public Administration(AasxCompatibilityModels.AdminShellV10.Administration src)
            {
                this.version = src.version;
                this.revision = src.revision;
            }
#endif

            public override string ToString()
            {
                return $"R={this.version}, V={this.revision}";
            }
        }



    }

    #endregion
}

