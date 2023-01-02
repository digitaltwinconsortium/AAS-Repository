#define UseAdminShell

using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

/* Copyright (c) 2018-2019 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>, author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/


#if UseAdminShell

namespace AdminShell
{

    #region Utils

    #endregion


    #region AdminShell_V1_0

    public partial class AdminShellV10
    {
        public class UnitId
        {

            // members

            [XmlIgnore]
            
            public KeyList keys = new KeyList();

            // getter / setters

            [XmlArray("keys")]
            [XmlArrayItem("key")]
            
            public List<Key> Keys { get { return keys?.Keys; } }
            [XmlIgnore]
            [JsonProperty(PropertyName = "keys")]
            public List<Key> JsonKeys
            {
                get
                {
                    keys?.NumberIndices();
                    return keys.Keys;
                }
            }

            [XmlIgnore]
            
            public bool IsEmpty { get { return keys == null || keys.IsEmpty; } }
            [XmlIgnore]
            
            public int Count { get { if (keys == null) return 0; return keys.Count; } }
            [XmlIgnore]
            
            public Key this[int index] { get { return keys.Keys[index]; } }

            // constructors / creators

            public UnitId() { }

            public UnitId(UnitId src)
            {
                if (src.keys != null)
                    foreach (var k in src.Keys)
                        this.keys.Add(new Key(k));
            }

            public static UnitId CreateNew(string type, bool local, string idType, string value)
            {
                var u = new UnitId();
                u.keys.Keys.Add(Key.CreateNew(type, local, idType, value));
                return u;
            }

            public static UnitId CreateNew(Reference src)
            {
                var res = new UnitId();
                if (src != null && src.Keys != null)
                    foreach (var k in src.Keys)
                        res.keys.Add(k);
                return res;
            }
        }

    }

    #endregion
}

#endif