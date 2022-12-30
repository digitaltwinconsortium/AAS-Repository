#define UseAasxCompatibilityModels

using System.Collections.Generic;
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
        // the whole class shall not be serialized by having it private
        public class KeyList
        {
            // members

            [XmlIgnore] // anyway, as it is privat
            private List<Key> key = new List<Key>();

            // getters / setters

            [XmlIgnore]
            public List<Key> Keys { get { return key; } }
            [XmlIgnore]
            public bool IsEmpty { get { return key == null || key.Count < 1; } }
            [XmlIgnore]
            public int Count { get { if (key == null) return 0; return key.Count; } }
            [XmlIgnore]
            public Key this[int index] { get { return key[index]; } }

            // constructors / creators

            public void Add(Key k)
            {
                key.Add(k);
            }

            public static KeyList CreateNew(Key k)
            {
                var kl = new KeyList();
                kl.Add(k);
                return kl;
            }

            public static KeyList CreateNew(string type, bool local, string idType, string value)
            {
                var kl = new KeyList();
                kl.Add(Key.CreateNew(type, local, idType, value));
                return kl;
            }

            // other

            public void NumberIndices()
            {
                if (this.Keys == null)
                    return;
                for (int i = 0; i < this.Keys.Count; i++)
                    this.Keys[i].index = i;
            }
        }

    }

    #endregion
}

#endif