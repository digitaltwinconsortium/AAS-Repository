/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace AasxCompatibilityModels
{
    #region AdminShell_V2_0

    public partial class AdminShellV20
    {
        public class Referable : IAasElement
        {
            [XmlIgnore]
            [JsonIgnore]
            [SkipForHash] // important to skip, as recursion elsewise will go in cycles!
            [SkipForReflection] // important to skip, as recursion elsewise will go in cycles!
            public DateTime TimeStampCreate;

            [XmlIgnore]
            [JsonIgnore]
            [SkipForHash] // important to skip, as recursion elsewise will go in cycles!
            [SkipForReflection] // important to skip, as recursion elsewise will go in cycles!
            public DateTime TimeStamp;

            public void setTimeStamp(DateTime timeStamp)
            {
                Referable r = this;

                do
                {
                    r.TimeStamp = timeStamp;
                    if (r != r.parent)
                    {
                        r = r.parent;
                    }
                    else
                        r = null;
                }
                while (r != null);
            }

            public void SetAllTimeStamps(DateTime timeStamp, DateTime timeStampCreate)
            {
                TimeStamp = timeStamp;
                TimeStampCreate = timeStampCreate;

                // via interface enumaration
                if (this is IEnumerateChildren)
                {
                    var childs = (this as IEnumerateChildren).EnumerateChildren();
                    if (childs != null)
                        foreach (var c in childs)
                            c.submodelElement.SetAllTimeStamps(timeStamp, timeStampCreate);
                }
            }

            public void SetAllTimeStamps(DateTime timeStamp)
            {
                TimeStamp = timeStamp;
                TimeStampCreate = timeStamp;

                // via interface enumaration
                if (this is IEnumerateChildren)
                {
                    var childs = (this as IEnumerateChildren).EnumerateChildren();
                    if (childs != null)
                        foreach (var c in childs)
                            c.submodelElement.SetAllTimeStamps(timeStamp);
                }
            }

            public void SetAllParents(Referable parent, DateTime timeStamp)
            {
                if (parent == null)
                    return;

                this.parent = parent;
                this.TimeStamp = timeStamp;
                this.TimeStampCreate = timeStamp;

                // via interface enumaration
                if (this is IEnumerateChildren)
                {
                    var childs = (this as IEnumerateChildren).EnumerateChildren();
                    if (childs != null)
                        foreach (var c in childs)
                            c.submodelElement.SetAllParents(this, timeStamp);
                }
            }

            public void SetAllParentsAndTimestamps(Referable parent, DateTime timeStamp, DateTime timeStampCreate)
            {
                if (parent == null)
                    return;

                this.parent = parent;
                this.TimeStamp = timeStamp;
                this.TimeStampCreate = timeStampCreate;

                // via interface enumaration
                if (this is IEnumerateChildren)
                {
                    var childs = (this as IEnumerateChildren).EnumerateChildren();
                    if (childs != null)
                        foreach (var c in childs)
                            c.submodelElement.SetAllParentsAndTimestamps(this, timeStamp, timeStampCreate);
                }
            }

            public Submodel getParentSubmodel()
            {
                Referable parent = this;
                while (!(parent is Submodel) && parent != null)
                    parent = parent.parent;
                return parent as Submodel;
            }

            // members

            [MetaModelName("Referable.IdShort")]
            [TextSearchable]
            [CountForHash]
            public string idShort = "";

            [MetaModelName("Referable.category")]
            [TextSearchable]
            [CountForHash]
            public string category = null;

            [XmlElement(ElementName = "description")]
            [JsonIgnore]
            [CountForHash]
            public Description description = null;

            [XmlIgnore]
            [JsonProperty(PropertyName = "descriptions")]
            public ListOfLangStr JsonDescription
            {
                get
                {
                    return description?.langString;
                }
                set
                {
                    if (value == null)
                    {
                        description = null;
                        return;
                    }

                    if (description == null)
                        description = new Description();
                    description.langString = value;
                }
            }

            [XmlIgnore]
            [JsonIgnore]
            [SkipForHash] // important to skip, as recursion elsewise will go in cycles!
            [SkipForReflection] // important to skip, as recursion elsewise will go in cycles!
            public Referable parent = null;

            public static string CONSTANT = "CONSTANT";
            public static string Category_PARAMETER = "PARAMETER";
            public static string VARIABLE = "VARIABLE";

            public static string[] ReferableCategoryNames = new string[] { CONSTANT, Category_PARAMETER, VARIABLE };

            // constructors

            public Referable() { }

            public Referable(string idShort)
            {
                this.idShort = idShort;
            }

            public Referable(Referable src)
            {
                if (src == null)
                    return;
                this.idShort = src.idShort;
                this.category = src.category;
                if (src.description != null)
                    this.description = new Description(src.description);
            }

#if !DoNotUseAasxCompatibilityModels
            public Referable(AasxCompatibilityModels.AdminShellV10.Referable src)
            {
                if (src == null)
                    return;
                this.idShort = src.idShort;
                if (this.idShort == null)
                    // change in V2.0 -> mandatory
                    this.idShort = "";
                this.category = src.category;
                if (src.description != null)
                    this.description = new Description(src.description);
            }
#endif

            public void AddDescription(string lang, string str)
            {
                if (description == null)
                    description = new Description();
                description.langString.Add(new LangStr(lang, str));
            }

            public virtual AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("Referable", "Ref");
            }

            public virtual string GetElementName()
            {
                return this.GetSelfDescription()?.ElementName;
            }

            public string GetFriendlyName()
            {
                return AdminShellNS.AdminShellUtil.FilterFriendlyName(this.idShort);
            }

            public void CollectReferencesByParent(List<Key> refs)
            {
                // access
                if (refs == null)
                    return;

                // check, if this is identifiable
                if (this is Identifiable)
                {
                    var idf = this as Identifiable;
                    if (idf != null)
                    {
                        var k = Key.CreateNew(
                            idf.GetElementName(), true, idf.identification?.idType, idf.identification?.id);
                        refs.Insert(0, k);
                    }
                }
                else
                {
                    var k = Key.CreateNew(this.GetElementName(), true, "IdShort", this.idShort);
                    refs.Insert(0, k);
                    // recurse upwards!
                    if (parent != null)
                        (this.parent).CollectReferencesByParent(refs);
                }
            }

            public string CollectIdShortByParent()
            {
                // recurse first
                var head = "";
                if (!(this is Identifiable) && this.parent != null)
                    // can go up
                    head = this.parent.CollectIdShortByParent() + "/";
                // add own
                var myid = "<no id-Short!>";
                if (this.idShort != null && this.idShort.Trim() != "")
                    myid = this.idShort.Trim();
                // together
                return head + myid;
            }

            // string functions

            public string ToIdShortString()
            {
                if (this.idShort == null || this.idShort.Trim().Length < 1)
                    return ("<no idShort!>");
                return this.idShort.Trim();
            }

            public override string ToString()
            {
                return "" + this.idShort;
            }

            // hash functionality

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

            private static System.Security.Cryptography.SHA256 HashProvider =
                System.Security.Cryptography.SHA256.Create();

            public string ComputeHashcode()
            {
                var dataBytes = this.ComputeByteArray();
                var hashBytes = Referable.HashProvider.ComputeHash(dataBytes);

                StringBuilder sb = new StringBuilder();
                foreach (var hb in hashBytes)
                    sb.Append(hb.ToString("X2"));
                return sb.ToString();
            }

            // sorting

            public class ComparerIdShort : IComparer<Referable>
            {
                public int Compare(Referable a, Referable b)
                {
                    return String.Compare(a?.idShort, b?.idShort,
                        CultureInfo.InvariantCulture, CompareOptions.IgnoreCase);
                }
            }

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
        }



    }

    #endregion
}

