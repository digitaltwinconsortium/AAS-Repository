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

    #region AdminShell_V3_0

    public partial class AdminShellV30
    {
        public class ListOfReferable : List<Referable>
        {
            // conversion to other list

            public KeyList ToKeyList()
            {
                var res = new KeyList();
                foreach (var rf in this)
                    res.Add(rf.ToKey());
                return res;
            }

            public ModelReference GetReference()
            {
                return ModelReference.CreateNew(ToKeyList());
            }
        }

    }

    #endregion
}

