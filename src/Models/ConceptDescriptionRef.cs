#define UseAasxCompatibilityModels

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
        [XmlType(TypeName = "conceptDescriptionRef")]
        public class ConceptDescriptionRef : Reference
        {
            // constructors

            public ConceptDescriptionRef() : base() { }

            public ConceptDescriptionRef(ConceptDescriptionRef src) : base(src) { }

            // further methods

            public new static ConceptDescriptionRef CreateNew(string type, bool local, string idType, string value)
            {
                var r = new ConceptDescriptionRef();
                r.Keys.Add(Key.CreateNew(type, local, idType, value));
                return r;
            }

            public override string GetElementName()
            {
                return "ConceptDescriptionRef";
            }
        }

    }

    #endregion
}

#endif