/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

//namespace AdminShellNS
namespace AdminShell_V30
{
    public partial class AdminShellV30
    {
        /// <summary>
        /// This reference is used to point OUTSIDE the model, to definitions and knowledge
        /// existing in the outside world.
        /// </summary>
        public class GlobalReference : Reference
        {
            // members

            [XmlIgnore] // anyway, as it is private/ protected
            
            protected ListOfIdentifier value = new ListOfIdentifier();

            // Keys getters / setters

            [XmlArray("values")]
            [XmlArrayItem("value")]
            
            public ListOfIdentifier Value { get { return value; } }
            [XmlIgnore]
            [JsonProperty(PropertyName = "values")]
            public ListOfIdentifier JsonKeys => value;

            // other members

            [XmlIgnore]
            
            public bool IsEmpty { get { return value == null || value.Count < 1; } }
            [XmlIgnore]
            
            public bool IsValid { get { return value != null && value.Count >= 1; } }
            [XmlIgnore]
            
            public int Count { get { if (value == null) return 0; return value.Count; } }
            [XmlIgnore]
            
            public Identifier this[int index] { get { return value[index]; } }

            [XmlIgnore]
            
            public Identifier First { get { return this.Count < 1 ? null : this.value[0]; } }

            [XmlIgnore]
            
            public Identifier Last { get { return this.Count < 1 ? null : this.value[this.value.Count - 1]; } }

            // constructors / creators

            public GlobalReference() : base() { }
            public GlobalReference(GlobalReference src) : base()
            {
                if (src == null)
                    return;

                foreach (var id in src.Value)
                    value.Add(new Identifier(id));
            }

            public GlobalReference(Reference r) : base() { }

            public GlobalReference(Identifier id) : base()
            {
                value.Add(id);
            }

#if !DoNotUseAasxCompatibilityModels
            public GlobalReference(List<AasxCompatibilityModels.AdminShellV10.Key> src)
            {
                if (src == null)
                    return;

                foreach (var k in src)
                    value.Add("" + k?.value);
            }

            public GlobalReference(List<AasxCompatibilityModels.AdminShellV20.Key> src)
            {
                if (src == null)
                    return;

                foreach (var k in src)
                    value.Add("" + k?.value);
            }

            public GlobalReference(AasxCompatibilityModels.AdminShellV10.Reference src)
            {
                if (src == null)
                    return;

                foreach (var k in src.Keys)
                    value.Add("" + k?.value);
            }

            public GlobalReference(AasxCompatibilityModels.AdminShellV20.Reference src)
            {
                if (src == null)
                    return;

                foreach (var k in src.Keys)
                    value.Add("" + k?.value);
            }
#endif

            public static GlobalReference CreateNew(Identifier id)
            {
                if (id == null)
                    return null;
                var r = new GlobalReference();
                r.value.Add(id);
                return r;
            }

            public static GlobalReference CreateNew(ListOfIdentifier loi)
            {
                if (loi == null)
                    return null;
                var r = new GlobalReference();
                r.value.AddRange(loi);
                return r;
            }

            public static GlobalReference CreateNew(List<Identifier> loi)
            {
                if (loi == null)
                    return null;
                var r = new GlobalReference();
                foreach (var id in loi)
                    r.value.Add(id);
                return r;
            }

            public static GlobalReference CreateNew(ModelReference mref)
            {
                if (mref?.Keys == null)
                    return null;
                var r = new GlobalReference();
                foreach (var key in mref.Keys)
                    r.value.Add(key?.value);
                return r;
            }

            public static GlobalReference CreateIrdiReference(string irdi)
            {
                return new GlobalReference("" + irdi);
            }

            // Matching

            public bool MatchesExactlyOneId(Identifier id, Key.MatchMode matchMode = Key.MatchMode.Relaxed)
            {
                if (value == null || value.Count != 1)
                    return false;
                return value[0].Matches(id, matchMode);
            }

            public bool Matches(Identifier id, Key.MatchMode matchMode = Key.MatchMode.Relaxed)
            {
                if (this.Count == 1)
                    return this[0].Matches(id, matchMode);
                return false;
            }

            public bool Matches(GlobalReference other, Key.MatchMode matchMode = Key.MatchMode.Relaxed)
            {
                if (this.value == null || other == null || other.Value == null || other.Count != this.Count)
                    return false;

                var same = true;
                for (int i = 0; i < this.Count; i++)
                    same = same && this.Value[i].Matches(other[i], matchMode);

                return same;
            }

            public bool Matches(ModelReference other, Key.MatchMode matchMode = Key.MatchMode.Relaxed)
            {
                if (this.value == null || other == null || other.Keys == null || other.Count != this.Count)
                    return false;

                var same = true;
                for (int i = 0; i < this.Count; i++)
                    same = same && this.Value[i].Matches(other[i], matchMode);

                return same;
            }

            public bool Matches(SemanticId other, Key.MatchMode matchMode = Key.MatchMode.Relaxed)
            {
                return Matches(new GlobalReference(other), matchMode);
            }

            public bool Matches(ConceptDescription cd, Key.MatchMode matchMode = Key.MatchMode.Relaxed)
            {
                return Matches(cd?.GetSemanticId(), matchMode);
            }

            // other

            public string ToString(int format = 0, string delimiter = ",")
            {
                return string.Join(delimiter, value);
            }

            public static GlobalReference Parse(string input)
            {
                return CreateNew(ListOfIdentifier.Parse(input));
            }

            /// <summary>
            /// Converts the GlobalReference to a simple Identifier
            /// </summary>
            /// <param name="strict">Check, if exact number of information is available</param>
            /// <returns>Identifier</returns>
            public Identifier GetAsIdentifier(bool strict = false)
            {
                if (value == null || value.Count < 1)
                    return null;
                if (strict && value.Count != 1)
                    return null;
                return value.First().value;
            }

            public Key GetAsExactlyOneKey(string type = null)
            {
                if (value == null || value.Count != 1)
                    return null;
                if (type == null)
                    type = Key.GlobalReference;
                var k = value[0];
                return new Key(type, k.value);
            }

            // self description

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("GlobalReference", "GRf");
            }

            public override string GetElementName()
            {
                return this.GetSelfDescription()?.ElementName;
            }
        }

    }
}
