#define UseAasxCompatibilityModels

using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

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
        public class SemanticId
        {

            // members

            [XmlIgnore]
            
            private KeyList keys = new KeyList();

            // getters / setters

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
            
            public Key this[int index] { get { return keys[index]; } }

            public override string ToString()
            {
                return Key.KeyListToString(keys.Keys);
            }

            // constructors / creators

            public SemanticId()
            {
            }

            public SemanticId(SemanticId src)
            {
                if (src != null)
                    foreach (var k in src.Keys)
                        keys.Add(k);
            }

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

            // matching

            public bool Matches(string type, bool local, string idType, string value)
            {
                if (this.Count == 1
                    && this.keys[0].type.ToLower().Trim() == type.ToLower().Trim()
                    && this.keys[0].local == local
                    && this.keys[0].idType.ToLower().Trim() == idType.ToLower().Trim()
                    && this.keys[0].value.ToLower().Trim() == value.ToLower().Trim())
                    return true;
                return false;
            }
        }

    }

    #endregion
}

#endif