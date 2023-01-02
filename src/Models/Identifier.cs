
namespace AdminShell
{
    using System.Text.RegularExpressions;
    using System.Xml;
    using System.Xml.Serialization;

    /// Thic class is the "old" Identification class of meta-model V2.0
    /// It did contain two attributes "idType" and "id"
    /// As string is sealed, this class cannot derive dirctly from string,
    /// so an implicit conversion is tested

    public class Identifier
    {
        [XmlText]
        public string value = string.Empty;

        [XmlAttribute]
        public string idType = string.Empty;

        [XmlText]
        public string id = string.Empty;

        public static implicit operator string(Identifier d)
        {
            return d.value;
        }

        public static implicit operator Identifier(string d)
        {
            return new Identifier(d);
        }

        public Identifier() { }

        public Identifier(Identifier src)
        {
            value = src.value;
        }

        public Identifier(string id)
        {
            value = id;
        }

        public Identifier(string idType, string id)
        {
            this.idType = idType;
            this.id = id;
        }

        public Identifier(Key key)
        {
            idType = key.Type.ToString();
            id = key.Value;
            value = key.Value;
        }

        public bool IsEqual(Identifier other)
        {
            return value.Trim().ToLower() == other.value.Trim().ToLower();
        }

        public static bool IsIRI(string value)
        {
            if (value == null)
                return false;
            var m = Regex.Match(value, @"\s*(\w+)://");
            return m.Success;
        }

        public static bool IsIRDI(string value)
        {
            if (value == null)
                return false;
            var m = Regex.Match(value, @"\s*(\d{3,4})(:|-|/)");
            return m.Success;
        }

        public bool Matches(string id, Key.MatchMode matchMode = Key.MatchMode.Identification)
        {
            if (id == null || value == null)
                return false;
            return value.Trim() == id.Trim();
        }

        public bool Matches(Identifier id, Key.MatchMode matchMode = Key.MatchMode.Identification)
        {
            if (id == null || value == null)
                return false;
            return value.Trim() == id.value.Trim();
        }

        public bool Matches(Key key, Key.MatchMode matchMode = Key.MatchMode.Identification)
        {
            if (key == null)
                return false;
            return Matches(key.Value, matchMode);
        }
    }
}

