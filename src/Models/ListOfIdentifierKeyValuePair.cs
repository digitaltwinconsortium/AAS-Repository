/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using System.Collections.Generic;
using System.Linq;

//namespace AdminShellNS
namespace AdminShell_V30
{
    public partial class AdminShellV30
    {
        public class ListOfIdentifierKeyValuePair : List<IdentifierKeyValuePair>
        {

            // constructors
            public ListOfIdentifierKeyValuePair() : base() { }
            public ListOfIdentifierKeyValuePair(ListOfIdentifierKeyValuePair src) : base()
            {
                if (src != null)
                    foreach (var kvp in src)
                        Add(new IdentifierKeyValuePair(kvp));
            }

            // further

            public string ToString(int format = 0, string delimiter = ",")
            {
                return string.Join(delimiter, this.Select((x) => x.ToString(format)));
            }
        }

    }
}
