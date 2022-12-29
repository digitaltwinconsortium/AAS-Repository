#define UseAasxCompatibilityModels

using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

/* Copyright (c) 2018-2019 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>, author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/


#if UseAasxCompatibilityModels

namespace AasxCompatibilityModels
{

    #region Utils

    #endregion


    #region AdminShell_V1_0

    public partial class AdminShellV10
    {
        public class Key
        {
            [XmlAttribute]
            public string type = "";
            [XmlAttribute]
            public bool local = false;

            [XmlAttribute]
            [JsonIgnore]
            public string idType = "";
            [XmlIgnore]
            [JsonProperty(PropertyName = "idType")]
            public string JsonIdType
            {
                get { return (idType == "idShort") ? "IdShort" : idType; }
                set { if (value == "IdShort") idType = "idShort"; else idType = value; }
            }

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

            public Key(string type, bool local, string idType, string value)
            {
                this.type = type;
                this.local = local;
                this.idType = idType;
                this.value = value;
            }

            public static Key CreateNew(string type, bool local, string idType, string value)
            {
                var k = new Key();
                k.type = type;
                k.local = local;
                k.idType = idType;
                k.value = value;
                return (k);
            }

            public static Key GetFromRef(Reference r)
            {
                if (r == null || r.Count != 1)
                    return null;
                return r[0];
            }

            public override string ToString()
            {
                var local = (this.local) ? "Local" : "not Local";
                return $"[{this.type}, {local}, {this.idType}, {this.value}]";
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
            "Event",
            "Operation",
            "OperationVariable",
            "Property",
            "ReferenceElement",
            "RelationshipElement",
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
            "Event",
            "Operation",
            "OperationVariable",
            "Property",
            "ReferenceElement",
            "RelationshipElement",
            "SubmodelElement",
            "SubmodelElementCollection",
            "View"
        };

            public static string[] SubmodelElements = new string[] {
            "DataElement",
            "File",
            "Event",
            "Operation",
            "Property",
            "ReferenceElement",
            "RelationshipElement",
            "SubmodelElementCollection"};

            public static string[] IdentifiableElements = new string[] {
            "Asset",
            "AssetAdministrationShell",
            "ConceptDescription",
            "Submodel" };

            // use this in list to designate all of the above elements
            public static string AllElements = "All";

            // use this in list to designate the GlobalReference
            public static string GlobalReference = "GlobalReference";
            public static string ConceptDescription = "ConceptDescription";
            public static string SubmodelRef = "SubmodelRef";
            public static string Submodel = "Submodel";
            public static string Asset = "Asset";
            public static string AAS = "AssetAdministrationShell";

            public static string[] IdentifierTypeNames = new string[] { "IdShort", "Custom", "IRDI", "URI" };

            public enum IdentifierType { IdShort = 0, Custom, IRDI, URI };

            public static string GetIdentifierTypeName(IdentifierType t)
            {
                return IdentifierTypeNames[(int)t];
            }

            // some helpers

            public static bool IsInKeyElements(string ke)
            {
                var res = false;
                foreach (var s in KeyElements)
                    if (s.Trim().ToLower() == ke.Trim().ToLower())
                        res = true;
                return res;
            }

        }

    }

    #endregion
}

#endif