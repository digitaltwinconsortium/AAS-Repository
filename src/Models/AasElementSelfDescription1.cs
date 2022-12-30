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
        // Self description (not in the meta model!)
        //

        public class AasElementSelfDescription
        {
            public string ElementName = "";
            public string ElementAbbreviation = "";
            public SubmodelElementWrapper.AdequateElementEnum ElementEnum =
                SubmodelElementWrapper.AdequateElementEnum.Unknown;

            public AasElementSelfDescription() { }

            public AasElementSelfDescription(
                string ElementName, string ElementAbbreviation,
                SubmodelElementWrapper.AdequateElementEnum elementEnum
                    = SubmodelElementWrapper.AdequateElementEnum.Unknown)
            {
                this.ElementName = ElementName;
                this.ElementAbbreviation = ElementAbbreviation;
                this.ElementEnum = elementEnum;
            }
        }

    }

    #endregion
}

