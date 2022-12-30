/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using System.Xml.Serialization;

namespace AasxCompatibilityModels
{
    #region AdminShell_V2_0

    public partial class AdminShellV20
    {
        [XmlType(TypeName = "conceptDescriptionRef")]
        public class ConceptDescriptionRef : Reference
        {
            // constructors

            public ConceptDescriptionRef() : base() { }

            public ConceptDescriptionRef(ConceptDescriptionRef src) : base(src) { }

#if !DoNotUseAasxCompatibilityModels
            public ConceptDescriptionRef(
                AasxCompatibilityModels.AdminShellV10.ConceptDescriptionRef src) : base(src) { }
#endif

            // further methods

            public new static ConceptDescriptionRef CreateNew(string type, bool local, string idType, string value)
            {
                var r = new ConceptDescriptionRef();
                r.Keys.Add(Key.CreateNew(type, local, idType, value));
                return r;
            }

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("ConceptDescriptionRef", "CDRef");
            }

        }



    }

    #endregion
}

