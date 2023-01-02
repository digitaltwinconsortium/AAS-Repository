
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class GlobalReference : Reference
    {
        [Required]
        [XmlArray("values")]
        [XmlArrayItem("Value")]
        [DataMember(Name = "Value")]
        public List<Identifier> Value { get; set; } = new();

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

        public static GlobalReference CreateNew(Identifier id)
        {
            if (id == null)
                return null;
            var r = new GlobalReference();
            r.value.Add(id);
            return r;
        }

        public static GlobalReference CreateNew(List<Identifier> loi)
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
                r.value.Add(key?.Value);
            return r;
        }

        public static GlobalReference CreateIrdiReference(string irdi)
        {
            return new GlobalReference("" + irdi);
        }

        public bool IsValid { get { return Value != null && Value.Count >= 1; } }
        [XmlIgnore]

        public int Count { get { if (Value == null) return 0; return Value.Count; } }
        [XmlIgnore]

        public Identifier this[int index] { get { return Value[index]; } }

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

        public string ToString(int format = 0, string delimiter = ",")
        {
            return string.Join(delimiter, value);
        }

        public static GlobalReference Parse(string input)
        {
            return CreateNew(ListOfIdentifier.Parse(input));
        }

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

        public AasElementSelfDescription GetSelfDescription()
        {
            return new AasElementSelfDescription("GlobalReference", "GRf");
        }

        public string GetElementName()
        {
            return this.GetSelfDescription()?.ElementName;
        }
    }
}
