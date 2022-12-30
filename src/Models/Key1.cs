/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace AasxCompatibilityModels
{
    #region AdminShell_V2_0

    public partial class AdminShellV20
    {
        public class Key
        {
            // Constants

            public enum MatchMode { Strict, Relaxed, Identification };

            // Members

            [MetaModelName("Key.type")]
            
            [XmlAttribute]
            
            public string type = "";

            [XmlAttribute]
            
            public bool local = false;

            [MetaModelName("Key.idType")]
            
            [XmlAttribute]
            
            
            public string idType = "";

            [XmlIgnore]
            [JsonProperty(PropertyName = "idType")]
            public string JsonIdType
            {
                // adapt idShort <-> IdShort
                get => (idType == "idShort") ? "IdShort" : idType;
                set => idType = (value == "idShort") ? "IdShort" : value;
            }

            [MetaModelName("Key.value")]
            
            [XmlText]
            
            public string value = "";

            [XmlIgnore]
            [JsonProperty(PropertyName = "index")]
            
            public int index = 0;

            public Key()
            {
            }

            public Key(Key src)
            {
                this.type = src.type;
                this.local = src.local;
                this.idType = src.idType;
                this.value = src.value;
            }

#if !DoNotUseAasxCompatibilityModels
            public Key(AasxCompatibilityModels.AdminShellV10.Key src)
            {
                this.type = src.type;
                this.local = src.local;
                this.idType = src.idType;
                if (this.idType.Trim().ToLower() == "uri")
                    this.idType = Identification.IRI;
                if (this.idType.Trim().ToLower() == "idshort")
                    this.idType = Identification.IdShort;
                this.value = src.value;
            }
#endif

            public Key(string type, bool local, string idType, string value)
            {
                this.type = type;
                this.local = local;
                this.idType = idType;
                this.value = value;
            }

            public static Key CreateNew(string type, bool local, string idType, string value)
            {
                var k = new Key()
                {
                    type = type,
                    local = local,
                    idType = idType,
                    value = value
                };
                return (k);
            }

            public static Key GetFromRef(Reference r)
            {
                if (r == null || r.Count != 1)
                    return null;
                return r[0];
            }

            public Identification ToId()
            {
                return new Identification(this);
            }

            public string ToString(int format = 0)
            {
                if (format == 1)
                {
                    return String.Format(
                        "({0})({1})[{2}]{3}", this.type, this.local ? "local" : "no-local", this.idType, this.value);
                }
                if (format == 2)
                {
                    return String.Format("[{0}]{1}", this.idType, this.value);
                }

                // (old) default
                var tlc = (this.local) ? "Local" : "not Local";
                return $"[{this.type}, {tlc}, {this.idType}, {this.value}]";
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
            "GlobalReference",
            "FragmentReference",
            "AccessPermissionRule",
            "Asset",
            "AssetAdministrationShell",
            "ConceptDescription",
            "Submodel",
            "SubmodelRef", // not completely right, but used by Package Explorer
            "Blob",
            "ConceptDictionary",
            "DataElement",
            "File",
            "Operation",
            "OperationVariable",
            "BasicEvent",
            "Entity",
            "Property",
            "MultiLanguageProperty",
            "Range",
            "ReferenceElement",
            "RelationshipElement",
            "AnnotatedRelationshipElement",
            "Capability",
            "SubmodelElement",
            "SubmodelElementCollection",
            "View" };

            public static string[] ReferableElements = new string[] {
            "AccessPermissionRule",
            "Asset",
            "AssetAdministrationShell",
            "ConceptDescription",
            "Submodel",
            "Blob",
            "ConceptDictionary",
            "DataElement",
            "File",
            "Operation",
            "OperationVariable",
            "Entity",
            "BasicEvent",
            "Property",
            "MultiLanguageProperty",
            "Range",
            "ReferenceElement",
            "RelationshipElement",
            "AnnotatedRelationshipElement",
            "Capability",
            "SubmodelElement",
            "SubmodelElementCollection",
            "View" };

            public static string[] SubmodelElements = new string[] {
            "DataElement",
            "File",
            "Event",
            "Operation",
            "Property",
            "MultiLanguageProperty",
            "Range",
            "ReferenceElement",
            "RelationshipElement",
            "AnnotatedRelationshipElement",
            "Capability",
            "BasicEvent",
            "Entity",
            "SubmodelElementCollection"};

            public static string[] IdentifiableElements = new string[] {
            "Asset",
            "AssetAdministrationShell",
            "ConceptDescription",
            "Submodel" };

            // use this in list to designate all of the above elements
            public static string AllElements = "All";

            // use this in list to designate the GlobalReference
            // Resharper disable MemberHidesStaticFromOuterClass
            public static string GlobalReference = "GlobalReference";
            public static string FragmentReference = "FragmentReference";
            public static string ConceptDescription = "ConceptDescription";
            public static string SubmodelRef = "SubmodelRef";
            public static string Submodel = "Submodel";
            public static string SubmodelElement = "SubmodelElement";
            public static string Asset = "Asset";
            public static string AAS = "AssetAdministrationShell";
            public static string Entity = "Entity";
            // Resharper enable MemberHidesStaticFromOuterClass

            public static string[] IdentifierTypeNames = new string[] {
                Identification.IdShort, "FragmentId", "Custom", Identification.IRDI, Identification.IRI };

            public enum IdentifierType { IdShort = 0, FragmentId, Custom, IRDI, IRI };

            public static string GetIdentifierTypeName(IdentifierType t)
            {
                return IdentifierTypeNames[(int)t];
            }

            public static string IdShort = "IdShort";
            public static string FragmentId = "FragmentId";
            public static string Custom = "Custom";

            // some helpers

            public static bool IsInKeyElements(string ke)
            {
                var res = false;
                foreach (var s in KeyElements)
                    if (s.Trim().ToLower() == ke.Trim().ToLower())
                        res = true;
                return res;
            }

            public bool IsIdType(string[] value)
            {
                if (value == null || idType == null || idType.Trim() == "")
                    return false;
                return value.Contains(idType.Trim());
            }

            public bool IsIdType(string value)
            {
                if (value == null || idType == null || idType.Trim() == "")
                    return false;
                return value.Trim().Equals(idType.Trim());
            }

            public bool Matches(
                string type, bool local, string idType, string id, MatchMode matchMode = MatchMode.Strict)
            {
                if (matchMode == MatchMode.Strict)
                    return this.type == type && this.local == local && this.idType == idType && this.value == id;

                if (matchMode == MatchMode.Relaxed)
                    return (this.type == type || this.type == Key.GlobalReference || type == Key.GlobalReference)
                        && this.idType == idType && this.value == id;

                if (matchMode == MatchMode.Identification)
                    return this.idType == idType && this.value == id;

                return false;
            }

            public bool Matches(Identification id)
            {
                if (id == null)
                    return false;
                return this.Matches(Key.GlobalReference, false, id.idType, id.id, MatchMode.Identification);
            }

            public bool Matches(Key key, MatchMode matchMode = MatchMode.Strict)
            {
                if (key == null)
                    return false;
                return this.Matches(key.type, key.local, key.idType, key.value, matchMode);
            }
        }



    }

    #endregion
}

