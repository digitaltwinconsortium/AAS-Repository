﻿
namespace AdminShell
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Xml;
    using System.Xml.Serialization;

    [DataContract]
    public class Key
    {
        public enum MatchMode { Strict, Relaxed, Identification };

        [Required]
        [XmlAttribute]
        [DataMember(Name = "type")]
        [MetaModelName("Key.type")]
        public KeyElements Type { get; set; }

        [Required]
        [XmlText]
        [DataMember(Name = "value")]
        [MetaModelName("Key.value")]
        public string Value { get; set; }

        public Key()
        {
        }

        public Key(Key src)
        {
            Type = src.Type;
            Value = src.Value;
        }

        public Key(string type, string value)
        {
            Type = (KeyElements)int.Parse(type);
            Value = value;
        }

        public string ToString(int format = 0)
        {
            if (format == 1)
            {
                return String.Format(
                    "({0}){1}", Type, Value);
            }
            if (format == 2)
            {
                return String.Format("{0}", Value);
            }

            // (old) default
            return $"[{Type}, {Value}]";
        }

        public bool Matches(string type, string id, MatchMode matchMode = MatchMode.Relaxed)
        {
            if (matchMode == MatchMode.Relaxed)
                return Type.ToString() == type && Value == id;

            if (matchMode == MatchMode.Identification)
                return Value == id;

            return false;
        }

        public bool Matches(Key key, MatchMode matchMode = MatchMode.Relaxed)
        {
            if (key == null)
                return false;
            return Matches(key.Type.ToString(), key.Value, matchMode);
        }
    }
}
