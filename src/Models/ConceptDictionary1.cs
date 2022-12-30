#define UseAasxCompatibilityModels

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
        public class ConceptDictionary : Referable
        {
            [XmlElement(ElementName = "conceptDescriptions")]
            public ConceptDescriptionRefs conceptDescriptionsRefs = null;

            // constructors

            public ConceptDictionary() { }

            public ConceptDictionary(ConceptDictionary src)
            {
                if (src.conceptDescriptionsRefs != null)
                    this.conceptDescriptionsRefs = new ConceptDescriptionRefs(src.conceptDescriptionsRefs);
            }

            public static ConceptDictionary CreateNew(string idShort = null)
            {
                var d = new ConceptDictionary();
                if (idShort != null)
                    d.idShort = idShort;
                return (d);
            }

            // add

            public void AddReference(Reference r)
            {
                var cdr = (ConceptDescriptionRef)(ConceptDescriptionRef.CreateNew(r.Keys));
                if (conceptDescriptionsRefs == null)
                    conceptDescriptionsRefs = new ConceptDescriptionRefs();
                conceptDescriptionsRefs.conceptDescriptions.Add(cdr);
            }

            public override string GetElementName()
            {
                return "ConceptDictionary";
            }
        }

    }

    #endregion
}

#endif