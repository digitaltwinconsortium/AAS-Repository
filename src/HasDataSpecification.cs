#define UseAasxCompatibilityModels

using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

/* Copyright (c) 2018-2019 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>, author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/


#if UseAasxCompatibilityModels

namespace AasxCompatibilityModels
{

    #region Utils

    #endregion


    #region AdminShell_V1_0

    public partial class AdminShellV10
    {
        [XmlType(TypeName = "hasDataSpecification")]
        public class HasDataSpecification
        {
            [XmlElement(ElementName = "reference")] // make "reference" go away by magic?!
            public List<Reference> reference = new List<Reference>();

            public HasDataSpecification() { }

            public HasDataSpecification(HasDataSpecification src)
            {
                foreach (var r in src.reference)
                    reference.Add(new Reference(r));
            }
        }

    }

    #endregion
}

#endif