
namespace AdminShell
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Text.RegularExpressions;
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
        [DataMember(Name = "Value")]
        [MetaModelName("Key.Value")]
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

        public Identifier ToId()
        {
            return new Identifier(this);
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

        public static Key Parse(string cell, string typeIfNotSet = null,
            bool allowFmtAll = false, bool allowFmt0 = false,
            bool allowFmt1 = false, bool allowFmt2 = false)
        {
            // access and defaults?
            if (cell == null || cell.Trim().Length < 1)
                return null;
            if (typeIfNotSet == null)
                typeIfNotSet = Key.GlobalReference;

            // TODO (MIHO, 2022-01-07): REWORK OLD & NEW FORMATS!!

            // OLD format == 1
            if (allowFmtAll || allowFmt1)
            {
                var m = Regex.Match(cell, @"\((\w+)\)\((\S+)\)\[(\w+)\]( ?)(.*)$");
                if (m.Success)
                {
                    return new AdminShell.Key(
                            m.Groups[1].ToString(), m.Groups[5].ToString());
                }
            }

            // OLD format == 2
            if (allowFmtAll || allowFmt2)
            {
                var m = Regex.Match(cell, @"\[(\w+)\]( ?)(.*)$");
                if (m.Success)
                {
                    return new AdminShell.Key(typeIfNotSet, m.Groups[3].ToString());
                }
            }

            // OLD format == 0
            if (allowFmtAll || allowFmt0)
            {
                var m = Regex.Match(cell, @"\[(\w+),( ?)([^,]+),( ?)\[(\w+)\],( ?)(.*)\]");
                if (m.Success)
                {
                    return new AdminShell.Key(m.Groups[1].ToString(), m.Groups[7].ToString());
                }
            }

            // no
            return null;
        }

        public static string KeyListToString(List<Key> keys)
        {
            if (keys == null || keys.Count < 1)
                return "";
            // normally, exactly one key
            if (keys.Count == 1)
                return keys[0].ToString();
            // multiple!
            var s = "[ ";
            foreach (var k in keys)
            {
                if (s.Length > 0)
                    s += ", ";
                s += k.ToString();
            }
            return s + " ]";
        }

        public static string[] KeyElements = new string[] {
            "AnnotatedRelationshipElement",
            "AssetAdministrationShell",
            "BasicEvent",
            "Blob",
            "Capability",
            "ConceptDescription",
            "DataElement",
            "Entity",
            "Event",
            "File",
            "FragmentReference",
            "GlobalElementReference",
            "ModelElementReference",
            "MultiLanguageProperty",
            "Operation",
            "OperationVariable", // not specified, but used by AASX Package Explorer
            "Property",
            "Range",
            "ReferenceElement",
            "RelationshipElement",
            "Submodel",
            "SubmodelElement",
            "SubmodelElementCollection", // not specified, but used by AASX Package Explorer
            "SubmodelElementList",
            "SubmodelElementStruct",
            "SubmodelReference" // not specified, but used by AASX Package Explorer
        };

        public static string[] ReferableElements = new string[] {
            "AnnotatedRelationshipElement",
            "AssetAdministrationShell",
            "BasicEvent",
            "Blob",
            "Capability",
            "ConceptDescription",
            "DataElement",
            "Entity",
            "Event",
            "File",
            "FragmentReference",
            "GlobalElementReference",
            "ModelElementReference",
            "MultiLanguageProperty",
            "Operation",
            "OperationVariable", // not specified, but used by AASX Package Explorer
            "Property",
            "Range",
            "ReferenceElement",
            "RelationshipElement",
            "Submodel",
            "SubmodelElement",
            "SubmodelElementCollection", // not specified, but used by AASX Package Explorer
            "SubmodelElementList",
            "SubmodelElementStruct"
        };

        public static string[] SubmodelElementElements = new string[] {
            "AnnotatedRelationshipElement",
            "BasicEvent",
            "Blob",
            "Capability",
            "DataElement",
            "Entity",
            "Event",
            "File",
            "GlobalReferenceElement",
            "ModelReferenceElement",
            "MultiLanguageProperty",
            "Operation",
            "Property",
            "Range",
            "ReferenceElement", // typo in spec?, but used by AASX Package Explorer
            "RelationshipElement",
            "Submodel", // not specified, but used by AASX Package Explorer
            "SubmodelElement",
            "SubmodelElementCollection", // not specified, but used by AASX Package Explorer
            "SubmodelElementList",
            "SubmodelElementStruct"
        };

        // use this in list to designate all of the above elements
        public static string AllElements = "All";

        // use this in list to designate the GlobalReference
        public static string GlobalReference = "GlobalElementReference";
        public static string ModelReference = "ModelElementReference";
        public static string FragmentReference = "FragmentReference";
        public static string ConceptDescription = "ConceptDescription";
        public static string SubmodelRef = "SubmodelReference";
        public static string Submodel = "Submodel";
        public static string SubmodelElement = "SubmodelElement";
        public static string AssetInformation = "AssetInformation";
        public static string AAS = "AssetAdministrationShell";
        public static string Entity = "Entity";

        public static bool IsInNamedElementsList(string[] elementsList, string ke)
        {
            if (elementsList == null || ke == null)
                return false;

            foreach (var s in elementsList)
                if (s.Trim().ToLower() == ke.Trim().ToLower())
                    return true;

            return false;
        }

        public bool IsInSubmodelElements()
        {
            return IsInNamedElementsList(SubmodelElementElements, Type.ToString());
        }

        public bool IsType(string value)
        {
            if (value == null)
                return false;

            return value.Trim().ToLower().Equals(Type.ToString().Trim().ToLower());
        }

        public bool Matches(
            string type, string id, MatchMode matchMode = MatchMode.Relaxed)
        {
            if (matchMode == MatchMode.Relaxed)
                return Type.ToString() == type && Value == id;

            if (matchMode == MatchMode.Identification)
                return Value == id;

            return false;
        }

        public bool Matches(Identifier id)
        {
            if (id == null)
                return false;
            return Matches(Key.GlobalReference, id.value, MatchMode.Identification);
        }

        public bool Matches(Key key, MatchMode matchMode = MatchMode.Relaxed)
        {
            if (key == null)
                return false;
            return Matches(key.Type.ToString(), key.Value, matchMode);
        }
    }
}
