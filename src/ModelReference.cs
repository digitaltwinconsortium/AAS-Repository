/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;

//namespace AdminShellNS
namespace AdminShell_V30
{
    public partial class AdminShellV30
    {
        /// <summary>
        /// These references are menat to point inside an model described by the meta model of the AAS
        /// </summary>
        public class ModelReference : Reference
        {
            // members

            public GlobalReference referredSemanticId = null;

            [XmlIgnore] // anyway, as it is private/ protected
            [JsonIgnore]
            protected KeyList keys = new KeyList();

            // Keys getters / setters

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

            // other members

            [XmlIgnore]
            [JsonIgnore]
            public bool IsEmpty { get { return keys == null || keys.Count < 1; } }
            [XmlIgnore]
            [JsonIgnore]
            public bool IsValid { get { return keys != null && keys.Count >= 1; } }
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

            public ModelReference()
            {
            }

            public ModelReference(Key k)
            {
                if (k != null)
                    keys.Add(k);
            }

            public ModelReference(ModelReference src)
            {
                if (src == null)
                    return;

                if (src.referredSemanticId != null)
                    referredSemanticId = new GlobalReference(src.referredSemanticId);

                foreach (var k in src.Keys)
                    keys.Add(new Key(k));
            }

            public ModelReference(GlobalReference src)
            {
                if (src == null)
                    return;

                foreach (var id in src.Value)
                    keys.Add(new Key("", id));
            }

            public ModelReference(SemanticId src, string type = null)
            {
                if (type == null)
                    type = Key.GlobalReference;
                if (src != null)
                    foreach (var id in src.Value)
                        keys.Add(new Key(type, id));
            }

#if !DoNotUseAasxCompatibilityModels
            public ModelReference(AasxCompatibilityModels.AdminShellV10.Reference src)
            {
                if (src != null)
                    foreach (var k in src.Keys)
                        keys.Add(new Key(k));
            }

            public ModelReference(AasxCompatibilityModels.AdminShellV10.SemanticId src)
            {
                if (src != null)
                    foreach (var k in src.Keys)
                        keys.Add(new Key(k));
            }

            public ModelReference(AasxCompatibilityModels.AdminShellV20.Reference src)
            {
                if (src != null)
                    foreach (var k in src.Keys)
                        keys.Add(new Key(k));
            }
#endif

            public static ModelReference CreateNew(Key k)
            {
                if (k == null)
                    return null;
                var r = new ModelReference();
                r.keys.Add(k);
                return r;
            }

            public static ModelReference CreateNew(List<Key> k)
            {
                if (k == null)
                    return null;
                var r = new ModelReference();
                r.keys.AddRange(k);
                return r;
            }

            public static ModelReference CreateNew(string keyType, Identifier id)
            {
                if (id == null)
                    return null;
                var r = new ModelReference();
                r.keys.Add(new Key(keyType, id));
                return r;
            }

            public static ModelReference CreateNew(string keyType, ListOfIdentifier loi)
            {
                if (loi == null)
                    return null;
                var r = new ModelReference();
                foreach (var id in loi)
                    r.keys.Add(new Key(keyType, id));
                return r;
            }

            public static ModelReference CreateNew(string type, string value)
            {
                if (type == null || value == null)
                    return null;
                var r = new ModelReference();
                r.keys.Add(Key.CreateNew(type, value));
                return r;
            }

            public static ModelReference Parse(string input)
            {
                return CreateNew(KeyList.Parse(input));
            }

            public static ModelReference CreateIrdiReference(string irdi)
            {
                if (irdi == null)
                    return null;
                var r = new ModelReference();
                r.keys.Add(new Key(Key.GlobalReference, irdi));
                return r;
            }

            // additions

            public static ModelReference operator +(ModelReference a, Key b)
            {
                var res = new ModelReference(a);
                res.Keys?.Add(b);
                return res;
            }

            public static ModelReference operator +(ModelReference a, ModelReference b)
            {
                var res = new ModelReference(a);
                res.Keys?.AddRange(b?.Keys);
                return res;
            }

            // Matching

            public bool MatchesExactlyOneKey(
                string type, string id, Key.MatchMode matchMode = Key.MatchMode.Relaxed)
            {
                if (keys == null || keys.Count != 1)
                    return false;
                var k = keys[0];
                return k.Matches(type, id, matchMode);
            }

            public bool MatchesExactlyOneKey(Key key, Key.MatchMode matchMode = Key.MatchMode.Relaxed)
            {
                if (key == null)
                    return false;
                return this.MatchesExactlyOneKey(key.type, key.value, matchMode);
            }

            public bool Matches(
                string type, string id, Key.MatchMode matchMode = Key.MatchMode.Relaxed)
            {
                if (this.Count == 1)
                {
                    var k = keys[0];
                    return k.Matches(type, id, matchMode);
                }
                return false;
            }

            public bool Matches(Key key, Key.MatchMode matchMode = Key.MatchMode.Relaxed)
            {
                if (this.Count == 1)
                {
                    var k = keys[0];
                    return k.Matches(key, matchMode);
                }
                return false;
            }

            public bool Matches(Identifier other)
            {
                if (other == null)
                    return false;
                if (this.Count == 1)
                {
                    var k = keys[0];
                    return k.Matches(Key.GlobalReference, other.value, Key.MatchMode.Identification);
                }
                return false;
            }

            public bool Matches(ModelReference other, Key.MatchMode matchMode = Key.MatchMode.Relaxed)
            {
                if (this.keys == null || other == null || other.keys == null || other.Count != this.Count)
                    return false;

                var same = true;
                for (int i = 0; i < this.Count; i++)
                    same = same && this.keys[i].Matches(other.keys[i], matchMode);

                return same;
            }

            public bool Matches(SemanticId other, Key.MatchMode matchMode = Key.MatchMode.Relaxed)
            {
                return Matches(new ModelReference(other), matchMode);
            }

            public bool Matches(ConceptDescription cd, Key.MatchMode matchMode = Key.MatchMode.Relaxed)
            {
                return Matches(cd?.GetModelReference(), matchMode);
            }

            public string ToString(int format = 0, string delimiter = ",")
            {
                return keys?.ToString(format, delimiter);
            }

            // further

            public Key GetAsExactlyOneKey()
            {
                if (keys == null || keys.Count != 1)
                    return null;
                var k = keys[0];
                return new Key(k.type, k.value);
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

            // self description

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("ModelReference", "MRf");
            }

            public override string GetElementName()
            {
                return this.GetSelfDescription()?.ElementName;
            }
        }

    }
}
