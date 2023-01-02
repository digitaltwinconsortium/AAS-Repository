
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;

    public class KeyList : List<Key>
    {
        [XmlIgnore]
        public bool IsEmpty { get { return this.Count < 1; } }

        public KeyList() { }

        public KeyList(KeyList src)
        {
            if (src != null)
                foreach (var k in src)
                    this.Add(new Key(k));
        }

        public bool Matches(KeyList other, Key.MatchMode matchMode = Key.MatchMode.Relaxed)
        {
            if (other == null || other.Count != this.Count)
                return false;

            var same = true;
            for (int i = 0; i < this.Count; i++)
                same = same && this[i].Matches(other[i], matchMode);

            return same;
        }

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
            var res = this[i].Value;
            if (this[i].IsType(Key.FragmentReference) && i > 0)
                res += this[i - 1].Value;
            return res;
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
                    res += this[i].Value;
                }
            }

            return res;
        }
    }
}

