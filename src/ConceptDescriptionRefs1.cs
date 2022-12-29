/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace AasxCompatibilityModels
{
    #region AdminShell_V2_0

    public partial class AdminShellV20
    {
        [XmlType(TypeName = "conceptDescriptions")]
        public class ConceptDescriptionRefs
        {
            [XmlElement(ElementName = "conceptDescriptionRef")]
            public List<ConceptDescriptionRef> conceptDescriptions = new List<ConceptDescriptionRef>();

            // constructors

            public ConceptDescriptionRefs() { }

            public ConceptDescriptionRefs(ConceptDescriptionRefs src)
            {
                if (src.conceptDescriptions != null)
                    foreach (var cdr in src.conceptDescriptions)
                        this.conceptDescriptions.Add(new ConceptDescriptionRef(cdr));
            }

#if !DoNotUseAasxCompatibilityModels
            public ConceptDescriptionRefs(AasxCompatibilityModels.AdminShellV10.ConceptDescriptionRefs src)
            {
                if (src.conceptDescriptions != null)
                    foreach (var cdr in src.conceptDescriptions)
                        this.conceptDescriptions.Add(new ConceptDescriptionRef(cdr));
            }
#endif
        }



    }

    #endregion
}

