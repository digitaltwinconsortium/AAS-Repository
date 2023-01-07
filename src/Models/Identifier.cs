﻿
namespace AdminShell
{
    using System.Runtime.Serialization;
    using System.Xml;
    using System.Xml.Serialization;

    /// Thic class is the "old" Identification class of meta-model V2.0
    /// It did contain two attributes "IdType" and "Id"
    /// As string is sealed, this class cannot derive dirctly from string,
    /// so an implicit conversion is tested

    [DataContract]
    public class Identifier
    {
        [DataMember(Name ="Value")]
        public string Value = string.Empty;

        [XmlAttribute]
        [DataMember(Name = "idType")]
        public string IdType = string.Empty;

        [DataMember(Name = "id")]
        public string Id = string.Empty;

        public static implicit operator string(Identifier d)
        {
            return d.Value;
        }

        public static implicit operator Identifier(string d)
        {
            return new Identifier(d);
        }

        public Identifier() { }

        public Identifier(Identifier src)
        {
            Value = src.Value;
        }

        public Identifier(string id)
        {
            Value = id;
        }

        public bool IsEqual(Identifier other)
        {
            return Value.Trim().ToLower() == other.Value.Trim().ToLower();
        }
    }
}

