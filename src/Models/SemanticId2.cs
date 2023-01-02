/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using System.Collections.Generic;


namespace AdminShell
{
    public partial class AdminShellV30
    {
        public class SemanticId : GlobalReference
        {
            // constructors / creators

            public SemanticId() : base() { }
            public SemanticId(Identifier id) : base(id) { }
            public SemanticId(SemanticId src) : base(src) { }
            public SemanticId(Reference src) : base(src) { }

#if !DoNotUseAdminShell
            public SemanticId(AdminShell.AdminShellV10.SemanticId src) : base(src?.Keys) { }
            public SemanticId(AdminShell.AdminShellV20.SemanticId src) : base(src) { }
#endif

            public static SemanticId CreateFromKey(Key key)
            {
                if (key == null)
                    return null;
                var res = new SemanticId();
                res.Value.Add(key?.Value);
                return res;
            }

            public static SemanticId CreateFromKeys(List<Key> keys)
            {
                if (keys == null)
                    return null;
                var res = new SemanticId();
                foreach (var k in keys)
                    res.Value.Add(k?.Value);
                return res;
            }

            public new static SemanticId Parse(string input)
            {
                return (SemanticId)CreateNew(ListOfIdentifier.Parse(input));
            }
        }

    }
}
