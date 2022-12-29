/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using System.Collections.Generic;

//namespace AdminShellNS
namespace AdminShell_V30
{
    public partial class AdminShellV30
    {
        public class ListOfSubmodels : List<Submodel>, IAasElement
        {
            // self decscription

            public AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("Submodels", "SMS");
            }

            public string GetElementName()
            {
                return this.GetSelfDescription()?.ElementName;
            }
        }

    }
}
