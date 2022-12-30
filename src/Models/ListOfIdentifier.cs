/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using AdminShellNS;
using System.Collections.Generic;
using System.Linq;

//namespace AdminShellNS
namespace AdminShell_V30
{
    public partial class AdminShellV30
    {
        public class ListOfIdentifier : List<Identifier>
        {
            // constructors / creators

            public ListOfIdentifier() : base() { }
            public ListOfIdentifier(Identifier id) : base() { if (id != null) Add(id); }
            public ListOfIdentifier(ListOfIdentifier loi) : base() { if (loi != null) AddRange(loi); }

            public static ListOfIdentifier CreateNew(KeyList kl)
            {
                if (kl == null)
                    return null;
                var res = new ListOfIdentifier();
                foreach (var k in kl)
                    if (k?.value != null)
                        res.Add(new Identifier(k?.value));
                return res;
            }

            // Member operation

            public void AddRange(List<Key> kl)
            {
                if (kl == null)
                    return;
                foreach (var k in kl)
                    Add(k?.value);
            }

            public string ToString(string delimiter = ",")
            {
                return string.Join(delimiter, this.Select((x) => x.ToString()));
            }

            public static ListOfIdentifier Parse(string input)
            {
                // access
                if (input == null)
                    return null;

                // split
                var parts = input.Split(',', ';');
                var loi = new ListOfIdentifier();

                foreach (var p in parts)
                    loi.Add(p);

                return loi;
            }

            // validation

            public static void Validate(AasValidationRecordList results, ListOfIdentifier kl,
                Referable container)
            {
                // access
                if (results == null || kl == null || container == null)
                    return;

                // iterate thru
                var idx = 0;
                while (idx < kl.Count)
                {
                    var act = Identifier.Validate(results, kl[idx], container);
                    if (act == AasValidationAction.ToBeDeleted)
                    {
                        kl.RemoveAt(idx);
                        continue;
                    }
                    idx++;
                }
            }

        }

    }
}
