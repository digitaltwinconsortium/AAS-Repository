
namespace AdminShell
{
    using Newtonsoft.Json;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    public class Referable : IAasElement
    {
        [DataMember(Name = "category")]
        [MetaModelName("Referable.category")]
        public string Category { get; set; }

        [DataMember(Name = "description")]
        public List<LangString> Description { get; set; }

        [DataMember(Name = "displayName")]
        public string DisplayName { get; set; }

        [Required]
        [MetaModelName("Referable.idShort")]
        [DataMember(Name = "idShort")]
        public string IdShort { get; set; }

        [Required]
        [DataMember(Name = "modelType")]
        public ModelType ModelType { get; set; }

        [DataMember(Name = "checksum")]
        [MetaModelName("Referable.checksum")]
        public string Checksum { get; set; } = string.Empty;

        [XmlIgnore]
        public IAasElement parent = null;

        public static string CONSTANT = "CONSTANT";
        public static string Category_PARAMETER = "PARAMETER";
        public static string VARIABLE = "VARIABLE";

        public static string[] ReferableCategoryNames = new string[] { CONSTANT, Category_PARAMETER, VARIABLE };

        public List<Extension> extension = null;

        [XmlIgnore]
        public DateTime TimeStampCreate;

        [XmlIgnore]
        public DateTime TimeStamp;

        public void setTimeStamp(DateTime timeStamp)
        {
            Referable r = this;

            do
            {
                r.TimeStamp = timeStamp;
                if (r != r.parent)
                {
                    r = (Referable)r.parent;
                }
                else
                    r = null;
            }
            while (r != null);
        }

        public Referable() { }

        public Referable(string idShort)
        {
            IdShort = idShort;
        }

        public Referable(Referable src)
        {
            if (src == null)
                return;

            IdShort = src.IdShort;

            Category = src.Category;

            if (src.Description != null)
                Description = new Description(src.Description);
        }

        public string CollectIdShortByParent()
        {
            // recurse first
            var head = "";
            if (!(this is Identifiable) && parent is Referable prf)
                // can go up
                head = prf.CollectIdShortByParent() + "/";
            // add own
            var myid = "<no id-Short!>";
            if (IdShort != null && IdShort.Trim() != "")
                myid = IdShort.Trim();
            // together
            return head + myid;
        }

        public class ObjectFieldInfo
        {
            public object o;
            public FieldInfo fi;

            public ObjectFieldInfo() { }

            public ObjectFieldInfo(object o, FieldInfo fi)
            {
                this.o = o;
                this.fi = fi;
            }
        }

        public List<ObjectFieldInfo> RecursivelyFindFields(object o, Type countAttribute, Type skipAttribute)
        {
            // access
            var res = new List<ObjectFieldInfo>();
            if (o == null || countAttribute == null)
                return res;

            // find fields for this object
            var t = o.GetType();
            var l = t.GetFields(BindingFlags.Instance | BindingFlags.Public);
            foreach (var f in l)
            {
                // Skip this field??
                if (skipAttribute != null && f.GetCustomAttribute(skipAttribute) != null)
                    continue;

                // add directly?
                if (f.GetCustomAttribute(countAttribute) != null)
                    res.Add(new ObjectFieldInfo(o, f));

                // object
                if (f.FieldType.IsClass)
                {
                    var oo = f.GetValue(o);
                    var r = RecursivelyFindFields(oo, countAttribute, skipAttribute);
                    res.AddRange(r);
                }

                // try cast in IList to check further
                var elems = f.GetValue(o) as IList;
                if (elems != null)
                    foreach (var e in elems)
                    {
                        var r = RecursivelyFindFields(e, countAttribute, skipAttribute);
                        res.AddRange(r);
                    }

            }

            // OK
            return res;
        }

        public byte[] ComputeByteArray()
        {
            // use memory stream for effcient behaviour
            var mems = new MemoryStream();

            // TEST
            var xxx = RecursivelyFindFields(this, typeof(CountForHash), typeof(SkipForHash));

            foreach (var ofi in xxx)
            {
                var a = ofi.fi.GetCustomAttribute<CountForHash>();
                if (a != null)
                {
                    // found an accountable field, get bytes
                    var o = ofi.fi.GetValue(ofi.o);
                    byte[] bs = null;

                    // optimize for probabilities

                    if (o is string)
                        bs = System.Text.Encoding.UTF8.GetBytes((string)o);
                    else if (o is char[])
                        bs = System.Text.Encoding.UTF8.GetBytes((char[])o);
                    else if (o is double)
                        bs = BitConverter.GetBytes((double)o);
                    else if (o is float)
                        bs = BitConverter.GetBytes((float)o);
                    else if (o is char)
                        bs = BitConverter.GetBytes((char)o);
                    else if (o is byte)
                        bs = BitConverter.GetBytes((byte)o);
                    else if (o is int)
                        bs = BitConverter.GetBytes((int)o);
                    else if (o is long)
                        bs = BitConverter.GetBytes((long)o);
                    else if (o is short)
                        bs = BitConverter.GetBytes((short)o);
                    else if (o is uint)
                        bs = BitConverter.GetBytes((uint)o);
                    else if (o is ulong)
                        bs = BitConverter.GetBytes((ulong)o);
                    else if (o is ushort)
                        bs = BitConverter.GetBytes((ushort)o);

                    if (bs != null)
                        mems.Write(bs, 0, bs.Length);
                }
            }

            return mems.ToArray();
        }

        private static System.Security.Cryptography.SHA256 HashProvider = System.Security.Cryptography.SHA256.Create();

        public class ComparerIndexed : IComparer<Referable>
        {
            public int NullIndex = int.MaxValue;
            public Dictionary<Referable, int> Index = new Dictionary<Referable, int>();

            public int Compare(Referable a, Referable b)
            {
                var ca = Index.ContainsKey(a);
                var cb = Index.ContainsKey(b);

                if (!ca && !cb)
                    return 0;
                // make CDs without usage to appear at end of list
                if (!ca)
                    return +1;
                if (!cb)
                    return -1;

                var ia = Index[a];
                var ib = Index[b];

                if (ia == ib)
                    return 0;
                if (ia < ib)
                    return -1;
                return +1;
            }
        }

        public virtual void RecurseOnReferables(
            object state, Func<object, List<Referable>, Referable, bool> lambda,
            bool includeThis = false)
        {
            if (includeThis)
                lambda(state, null, this);
        }
    }
}
