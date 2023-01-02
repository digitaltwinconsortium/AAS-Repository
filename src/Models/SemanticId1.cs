/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using System.Collections.Generic;

namespace AdminShell
{
    #region AdminShell_V2_0

    public partial class AdminShellV20
    {
        public class SemanticId : Reference
        {

            // constructors / creators

            public SemanticId()
                : base()
            {
            }

            public SemanticId(SemanticId src)
                : base(src)
            {
            }

            public SemanticId(Reference src) : base(src) { }

#if !DoNotUseAdminShell
            public SemanticId(AdminShell.AdminShellV10.SemanticId src)
                : base(src)
            {
            }
#endif
            public SemanticId(Key key) : base(key) { }

            public static SemanticId CreateFromKey(Key key)
            {
                if (key == null)
                    return null;
                var res = new SemanticId();
                res.Keys.Add(key);
                return res;
            }

            public static SemanticId CreateFromKeys(List<Key> keys)
            {
                if (keys == null)
                    return null;
                var res = new SemanticId();
                res.Keys.AddRange(keys);
                return res;
            }

        }



    }

    #endregion
}

