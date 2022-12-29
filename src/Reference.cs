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
        [XmlType(TypeName = "reference")]
        public class Reference
        {

            // members

            [XmlIgnore] // anyway, as it is privat
            [JsonIgnore]
            private KeyList keys = new KeyList();

            // getters / setters

            [XmlArray("keys")]
            [XmlArrayItem("key")]
            [JsonIgnore]
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
            [JsonIgnore]
            public bool IsEmpty { get { return keys == null || keys.Count < 1; } }
            [XmlIgnore]
            [JsonIgnore]
            public int Count { get { if (keys == null) return 0; return keys.Count; } }
            [XmlIgnore]
            [JsonIgnore]
            public Key this[int index] { get { return keys[index]; } }

            // constructors / creators

            public Reference()
            {
            }

            public Reference(Key k)
            {
                if (k != null)
                    keys.Keys.Add(k);
            }

            public Reference(Reference src)
            {
                if (src != null)
                    foreach (var k in src.Keys)
                        keys.Add(new Key(k));
            }

            public Reference(SemanticId src)
            {
                if (src != null)
                    foreach (var k in src.Keys)
                        keys.Add(new Key(k));
            }

            public static Reference CreateNew(Key k)
            {
                if (k == null)
                    return null;
                var r = new Reference();
                r.keys.Keys.Add(k);
                return r;
            }

            public static Reference CreateNew(List<Key> k)
            {
                if (k == null)
                    return null;
                var r = new Reference();
                r.keys.Keys.AddRange(k);
                return r;
            }

            public static Reference CreateNew(string type, bool local, string idType, string value)
            {
                if (type == null || idType == null || value == null)
                    return null;
                var r = new Reference();
                r.keys.Keys.Add(Key.CreateNew(type, local, idType, value));
                return r;
            }

            public static Reference CreateIrdiReference(string irdi)
            {
                if (irdi == null)
                    return null;
                var r = new Reference();
                r.keys.Keys.Add(new Key(Key.GlobalReference, false, "IRDI", irdi));
                return r;
            }

            // further

            public bool IsExactlyOneKey(string type, bool local, string idType, string id)
            {
                if (keys == null || keys.Keys == null || keys.Count != 1)
                    return false;
                var k = keys.Keys[0];
                return k.type == type && k.local == local && k.idType == idType && k.value == id;
            }

            public bool MatchesTo(Identification other)
            {
                return (this.keys != null && this.keys.Count == 1
                    && this.keys[0].idType.Trim().ToLower() == other.idType.Trim().ToLower()
                    && this.keys[0].value.Trim().ToLower() == other.id.Trim().ToLower());
            }

            public bool MatchesTo(Reference other)
            {
                if (this.keys == null || other == null || other.keys == null || other.Count != this.Count)
                    return false;

                var same = true;
                for (int i = 0; i < this.Count; i++)
                    same = same
                        && this.keys[i].type.Trim().ToLower() == other.keys[i].type.Trim().ToLower()
                        && this.keys[i].local == other.keys[i].local
                        && this.keys[i].idType.Trim().ToLower() == other.keys[i].idType.Trim().ToLower()
                        && this.keys[i].value.Trim().ToLower() == other.keys[i].value.Trim().ToLower();

                return same;
            }

            public override string ToString()
            {
                var res = "";
                if (keys != null && keys.Keys != null)
                    foreach (var k in keys.Keys)
                        res += k.ToString() + ",";
                return res.TrimEnd(',');
            }

            public string ListOfValues(string delim)
            {
                string res = "";
                if (this.Keys != null)
                    foreach (var x in this.Keys)
                    {
                        if (x == null)
                            continue;
                        if (res != "") res += delim;
                        res += x.value;
                    }
                return res;
            }

            public virtual string GetElementName()
            {
                return "Reference";
            }
        }

    }

    #endregion
}

#endif