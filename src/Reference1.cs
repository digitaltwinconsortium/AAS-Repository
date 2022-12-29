/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AasxCompatibilityModels
{
    #region AdminShell_V2_0

    public partial class AdminShellV20
    {
        [XmlType(TypeName = "reference")]
        public class Reference : IAasElement
        {

            // members

            [XmlIgnore] // anyway, as it is private
            [JsonIgnore]
            private KeyList keys = new KeyList();

            // getters / setters

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
            public bool IsEmpty { get { return keys == null || keys.Count < 1; } }
            [XmlIgnore]
            [JsonIgnore]
            public int Count { get { if (keys == null) return 0; return keys.Count; } }
            [XmlIgnore]
            [JsonIgnore]
            public Key this[int index] { get { return keys[index]; } }

            [XmlIgnore]
            [JsonIgnore]
            public Key First { get { return this.Count < 1 ? null : this.keys[0]; } }

            [XmlIgnore]
            [JsonIgnore]
            public Key Last { get { return this.Count < 1 ? null : this.keys[this.keys.Count - 1]; } }

            // constructors / creators

            public Reference()
            {
            }

            public Reference(Key k)
            {
                if (k != null)
                    keys.Add(k);
            }

            public Reference(Reference src)
            {
                if (src != null)
                    foreach (var k in src.Keys)
                        keys.Add(new Key(k));
            }

#if !DoNotUseAasxCompatibilityModels
            public Reference(AasxCompatibilityModels.AdminShellV10.Reference src)
            {
                if (src != null)
                    foreach (var k in src.Keys)
                        keys.Add(new Key(k));
            }
#endif

            public Reference(SemanticId src)
            {
                if (src != null)
                    foreach (var k in src.Keys)
                        keys.Add(new Key(k));
            }

#if !DoNotUseAasxCompatibilityModels
            public Reference(AasxCompatibilityModels.AdminShellV10.SemanticId src)
            {
                if (src != null)
                    foreach (var k in src.Keys)
                        keys.Add(new Key(k));
            }
#endif
            public static Reference CreateNew(Key k)
            {
                if (k == null)
                    return null;
                var r = new Reference();
                r.keys.Add(k);
                return r;
            }

            public static Reference CreateNew(List<Key> k)
            {
                if (k == null)
                    return null;
                var r = new Reference();
                r.keys.AddRange(k);
                return r;
            }

            public static Reference CreateNew(string type, bool local, string idType, string value)
            {
                if (type == null || idType == null || value == null)
                    return null;
                var r = new Reference();
                r.keys.Add(Key.CreateNew(type, local, idType, value));
                return r;
            }

            public static Reference CreateIrdiReference(string irdi)
            {
                if (irdi == null)
                    return null;
                var r = new Reference();
                r.keys.Add(new Key(Key.GlobalReference, false, Identification.IRDI, irdi));
                return r;
            }

            // additions

            public static Reference operator +(Reference a, Key b)
            {
                var res = new Reference(a);
                res.Keys?.Add(b);
                return res;
            }

            public static Reference operator +(Reference a, Reference b)
            {
                var res = new Reference(a);
                res.Keys?.AddRange(b?.Keys);
                return res;
            }

            // further

            public Key GetAsExactlyOneKey()
            {
                if (keys == null || keys.Count != 1)
                    return null;
                var k = keys[0];
                return new Key(k.type, k.local, k.idType, k.value);
            }

            public bool MatchesExactlyOneKey(
                string type, bool local, string idType, string id, Key.MatchMode matchMode = Key.MatchMode.Strict)
            {
                if (keys == null || keys.Count != 1)
                    return false;
                var k = keys[0];
                return k.Matches(type, local, idType, id, matchMode);
            }

            public bool MatchesExactlyOneKey(Key key, Key.MatchMode matchMode = Key.MatchMode.Strict)
            {
                if (key == null)
                    return false;
                return this.MatchesExactlyOneKey(key.type, key.local, key.idType, key.value, matchMode);
            }

            public bool Matches(
                string type, bool local, string idType, string id, Key.MatchMode matchMode = Key.MatchMode.Strict)
            {
                if (this.Count == 1)
                {
                    var k = keys[0];
                    return k.Matches(type, local, idType, id, matchMode);
                }
                return false;
            }

            public bool Matches(Key key, Key.MatchMode matchMode = Key.MatchMode.Strict)
            {
                if (this.Count == 1)
                {
                    var k = keys[0];
                    return k.Matches(key, matchMode);
                }
                return false;
            }

            public bool Matches(Identification other)
            {
                if (other == null)
                    return false;
                if (this.Count == 1)
                {
                    var k = keys[0];
                    return k.Matches(Key.GlobalReference, false, other.idType, other.id, Key.MatchMode.Identification);
                }
                return false;
            }

            public bool Matches(Reference other, Key.MatchMode matchMode = Key.MatchMode.Strict)
            {
                if (this.keys == null || other == null || other.keys == null || other.Count != this.Count)
                    return false;

                var same = true;
                for (int i = 0; i < this.Count; i++)
                    same = same && this.keys[i].Matches(other.keys[i], matchMode);

                return same;
            }

            public bool Matches(SemanticId other, Key.MatchMode matchMode = Key.MatchMode.Strict)
            {
                return Matches(new Reference(other), matchMode);
            }

            public bool Matches(ConceptDescription cd, Key.MatchMode matchMode = Key.MatchMode.Strict)
            {
                return Matches(cd?.GetReference(), matchMode);
            }

            public string ToString(int format = 0, string delimiter = ",")
            {
                return keys?.ToString(format, delimiter);
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

            public virtual AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("Reference", "Rfc");
            }

            public virtual string GetElementName()
            {
                return this.GetSelfDescription()?.ElementName;
            }
        }



    }

    #endregion
}

