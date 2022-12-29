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
        public class ListOfConceptDescriptions : List<ConceptDescription>, IAasElement
        {
            // finding

            public ConceptDescription Find(ConceptDescriptionRef cdr)
            {
                if (cdr == null)
                    return null;
                return Find(cdr.Keys);
            }

            public ConceptDescription Find(Identifier id)
            {
                var cdr = ConceptDescriptionRef.CreateNew("Conceptdescription", id.value);
                return Find(cdr);
            }

            public ConceptDescription Find(List<Key> keys)
            {
                // trivial
                if (keys == null)
                    return null;
                // can only refs with 1 key
                if (keys.Count != 1)
                    return null;
                // and we're picky
                var key = keys[0];
                if (key.type.ToLower().Trim() != "conceptdescription")
                    return null;
                // brute force
                foreach (var cd in this)
                    if (cd.id.value.ToLower().Trim() == key.value.ToLower().Trim())
                        return cd;
                // uups
                return null;
            }

            // item management

            public ConceptDescription AddIfNew(ConceptDescription cd)
            {
                if (cd == null)
                    return null;

                var exist = this.Find(cd.id);
                if (exist != null)
                    return exist;

                this.Add(cd);
                return cd;
            }

            // self decscription

            public AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("ConceptDescriptions", "CDS");
            }

            public string GetElementName()
            {
                return this.GetSelfDescription()?.ElementName;
            }

            // sorting


        }

    }
}
