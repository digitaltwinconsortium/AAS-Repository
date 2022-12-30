/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using System.Collections.Generic;
using System.Xml.Serialization;

namespace AasxCompatibilityModels
{
    #region AdminShell_V2_0

    public partial class AdminShellV20
    {
        public class KeyList : List<Key>
        {
            // getters / setters

            [XmlIgnore]
            public bool IsEmpty { get { return this.Count < 1; } }

            public void NumberIndices()
            {
                for (int i = 0; i < this.Count; i++)
                    this[i].index = i;
            }

            public string ToString(int format = 0, string delimiter = ",")
            {
                var res = "";
                foreach (var k in this)
                    res += k.ToString(format) + delimiter;
                return res.TrimEnd(',');
            }
        }



    }

    #endregion
}

