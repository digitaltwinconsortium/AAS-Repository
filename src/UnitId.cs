/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;
using System.Xml.Serialization;

namespace AasxCompatibilityModels
{
    #region AdminShell_V2_0

    public partial class AdminShellV20
    {
        public class UnitId
        {

            // members

            [XmlIgnore]
            [JsonIgnore]
            public KeyList keys = new KeyList();

            // getter / setters

            [XmlArray("keys")]
            [XmlArrayItem("key")]
            [JsonIgnore]
            public KeyList Keys { get { return keys; } }
            [XmlIgnore]
            [JsonProperty(PropertyName = "keys")]
            public KeyList JsonKeys
            {
                get
                {
                    keys?.NumberIndices();
                    return keys;
                }
            }

            [XmlIgnore]
            [JsonIgnore]
            public bool IsEmpty { get { return keys == null || keys.IsEmpty; } }
            [XmlIgnore]
            [JsonIgnore]
            public int Count { get { if (keys == null) return 0; return keys.Count; } }
            [XmlIgnore]
            [JsonIgnore]
            public Key this[int index] { get { return keys[index]; } }

            // constructors / creators

            public UnitId() { }

            public UnitId(UnitId src)
            {
                if (src.keys != null)
                    foreach (var k in src.Keys)
                        this.keys.Add(new Key(k));
            }

#if !DoNotUseAasxCompatibilityModels
            public UnitId(AasxCompatibilityModels.AdminShellV10.UnitId src)
            {
                if (src.keys != null)
                    foreach (var k in src.Keys)
                        this.keys.Add(new Key(k));
            }
#endif

            public static UnitId CreateNew(string type, bool local, string idType, string value)
            {
                var u = new UnitId();
                u.keys.Add(Key.CreateNew(type, local, idType, value));
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

