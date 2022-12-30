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
        //
        // Administration
        //

        public class Administration
        {

            // members

            [MetaModelName("Administration.version")]
            
            
            public string version = "";

            [MetaModelName("Administration.revision")]
            
            
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

            public Administration(AasxCompatibilityModels.AdminShellV20.Administration src)
            {
                this.version = src.version;
                this.revision = src.revision;
            }
#endif

            public Administration(string version, string revision)
            {
                this.version = version;
                this.revision = revision;
            }

            public override string ToString()
            {
                return $"R={this.version}, V={this.revision}";
            }
        }

    }

    #endregion
}

