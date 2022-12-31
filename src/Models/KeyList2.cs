/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using AdminShell;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

//namespace AdminShell
namespace AdminShell_V30
{
    public partial class AdminShellV30
    {
        public class KeyList : List<Key>
        {
            // getters / setters

            [XmlIgnore]
            public bool IsEmpty { get { return this.Count < 1; } }

            // constructors / creators

            public KeyList() { }

            public KeyList(KeyList src)
            {
                if (src != null)
                    foreach (var k in src)
                        this.Add(new Key(k));
            }

            public static KeyList CreateNew(Key k)
            {
                var kl = new KeyList { k };
                return kl;
            }

            public static KeyList CreateNew(string type, string value)
            {
                var kl = new KeyList() {
                    Key.CreateNew(type, value)
                };
                return kl;
            }

            public static KeyList CreateNew(string type, string[] valueItems)
            {
                // access
                if (valueItems == null)
                    return null;

                // prepare
                var kl = new AdminShell.KeyList();
                foreach (var x in valueItems)
                    kl.Add(new AdminShell.Key(type, "" + x));
                return kl;
            }

            // matches

            public bool Matches(KeyList other, Key.MatchMode matchMode = Key.MatchMode.Relaxed)
            {
                if (other == null || other.Count != this.Count)
                    return false;

                var same = true;
                for (int i = 0; i < this.Count; i++)
                    same = same && this[i].Matches(other[i], matchMode);

                return same;
            }

            // other

            public void NumberIndices()
            {
                for (int i = 0; i < this.Count; i++)
                    this[i].index = i;
            }

            public string ToString(int format = 0, string delimiter = ",")
            {
                return string.Join(delimiter, this.Select((x) => x.ToString(format)));
            }

            public static KeyList Parse(string input)
            {
                // access
                if (input == null)
                    return null;

                // split
                var parts = input.Split(',', ';');
                var kl = new KeyList();

                foreach (var p in parts)
                {
                    var k = Key.Parse(p);
                    if (k != null)
                        kl.Add(k);
                }

                return kl;
            }

            public string MostSignificantInfo()
            {
                if (this.Count < 1)
                    return "-";
                var i = this.Count - 1;
                var res = this[i].value;
                if (this[i].IsType(Key.FragmentReference) && i > 0)
                    res += this[i - 1].value;
                return res;
            }

            // validation

            public static void Validate(AasValidationRecordList results, KeyList kl,
                Referable container)
            {
                // access
                if (results == null || kl == null || container == null)
                    return;

                // iterate thru
                var idx = 0;
                while (idx < kl.Count)
                {
                    var act = Key.Validate(results, kl[idx], container);
                    if (act == AasValidationAction.ToBeDeleted)
                    {
                        kl.RemoveAt(idx);
                        continue;
                    }
                    idx++;
                }
            }

            public bool StartsWith(KeyList head, bool emptyIsTrue = false,
                Key.MatchMode matchMode = Key.MatchMode.Relaxed)
            {
                // access
                if (head == null)
                    return false;
                if (head.Count == 0)
                    return emptyIsTrue;

                // simply test element-wise
                for (int i = 0; i < head.Count; i++)
                {
                    // does head have more elements than this list?
                    if (i >= this.Count)
                        return false;

                    if (!head[i].Matches(this[i], matchMode))
                        return false;
                }

                // ok!
                return true;
            }

            // arithmetics

            public static KeyList operator +(KeyList a, Key b)
            {
                var res = new KeyList(a);
                if (b != null)
                    res.Add(b);
                return res;
            }

            public static KeyList operator +(KeyList a, KeyList b)
            {
                var res = new KeyList(a);
                if (b != null)
                    res.AddRange(b);
                return res;
            }

            public KeyList SubList(int startPos, int count = int.MaxValue)
            {
                var res = new KeyList();
                if (startPos >= this.Count)
                    return res;
                int nr = 0;
                for (int i = startPos; i < this.Count && nr < count; i++)
                {
                    nr++;
                    res.Add(this[i]);
                }
                return res;
            }

            public KeyList ReplaceLastKey(KeyList newKeys)
            {
                var res = new KeyList(this);
                if (res.Count < 1 || newKeys == null || newKeys.Count < 1)
                    return res;

                res.Remove(res.Last());
                res = res + newKeys;

                return res;
            }

            // other

            /// <summary>
            /// Take only idShort, ignore all other key-types and create a '/'-separated list
            /// </summary>
            /// <returns>Empty string or list of idShorts</returns>
            public string BuildIdShortPath(int startPos = 0, int count = int.MaxValue)
            {
                if (startPos >= this.Count)
                    return "";
                int nr = 0;
                var res = "";
                for (int i = startPos; i < this.Count && nr < count; i++)
                {
                    nr++;
                    // V3RC02: quite expensive check: if SME -> then treat as idShort
                    if (this[i].IsInSubmodelElements())
                    {
                        if (res != "")
                            res += "/";
                        res += this[i].value;
                    }
                }
                return res;
            }
        }

    }
}
