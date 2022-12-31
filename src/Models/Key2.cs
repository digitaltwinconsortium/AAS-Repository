/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using AdminShell;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

//namespace AdminShell
namespace AdminShell_V30
{
    public partial class AdminShellV30
    {
        //
        // Keys
        //

        public class Key
        {
            // Constants

            public enum MatchMode { Relaxed, Identification }; // in V3.0RC02: Strict not anymore

            // Members

            [MetaModelName("Key.type")]
            
            [XmlAttribute]
            
            public string type = "";

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
                this.value = src.value;
            }

#if !DoNotUseAasxCompatibilityModels
            public Key(AasxCompatibilityModels.AdminShellV10.Key src)
            {
                this.type = src.type;
                this.value = src.value;
            }

            public Key(AasxCompatibilityModels.AdminShellV20.Key src)
            {
                if (src == null)
                    return;
                var stll = src.type?.Trim().ToLower();
                if (stll == AasxCompatibilityModels.AdminShellV20.Key.GlobalReference.ToLower())
                    this.type = Key.GlobalReference;
                else
                    this.type = src.type;
                this.value = src.value;
            }
#endif

            public Key(string type, string value)
            {
                this.type = type;
                this.value = value;
            }

            public static Key CreateNew(string type, string value)
            {
                var k = new Key()
                {
                    type = type,
                    value = value
                };
                return (k);
            }

            public static Key GetFromRef(ModelReference r)
            {
                if (r == null || r.Count != 1)
                    return null;
                return r[0];
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
                        "({0}){1}", this.type, this.value);
                }
                if (format == 2)
                {
                    return String.Format("{0}", this.value);
                }

                // (old) default
                return $"[{this.type}, {this.value}]";
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
                "SubmodelRef" // not specified, but used by AASX Package Explorer
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
            // Resharper disable MemberHidesStaticFromOuterClass
            public static string GlobalReference = "GlobalElementReference";
            public static string ModelReference = "ModelElementReference";
            public static string FragmentReference = "FragmentReference";
            public static string ConceptDescription = "ConceptDescription";
            public static string SubmodelRef = "SubmodelRef";
            public static string Submodel = "Submodel";
            public static string SubmodelElement = "SubmodelElement";
            public static string AssetInformation = "AssetInformation";
            public static string AAS = "AssetAdministrationShell";
            public static string Entity = "Entity";
            // Resharper enable MemberHidesStaticFromOuterClass

            // some helpers

            public static bool IsInNamedElementsList(string[] elementsList, string ke)
            {
                if (elementsList == null || ke == null)
                    return false;

                foreach (var s in elementsList)
                    if (s.Trim().ToLower() == ke.Trim().ToLower())
                        return true;

                return false;
            }

            public bool IsInKeyElements()
            {
                return IsInNamedElementsList(KeyElements, this.type);
            }

            public bool IsInReferableElements()
            {
                return IsInNamedElementsList(ReferableElements, this.type);
            }

            public bool IsInSubmodelElements()
            {
                return IsInNamedElementsList(SubmodelElementElements, this.type);
            }

            public bool IsIRI()
            {
                return Identifier.IsIRI(value);
            }

            public bool IsIRDI()
            {
                return Identifier.IsIRDI(value);
            }

            public bool IsType(string value)
            {
                if (value == null || type == null || type.Trim() == "")
                    return false;
                return value.Trim().ToLower().Equals(type.Trim().ToLower());
            }

            public bool IsAbsolute()
            {
                return IsType(Key.GlobalReference)
                    || IsType(Key.AAS)
                    || IsType(Key.Submodel);
            }

            public bool Matches(
                string type, string id, MatchMode matchMode = MatchMode.Relaxed)
            {
                if (matchMode == MatchMode.Relaxed)
                    return this.type == type && this.value == id;

                if (matchMode == MatchMode.Identification)
                    return this.value == id;

                return false;
            }

            public bool Matches(Identifier id)
            {
                if (id == null)
                    return false;
                return this.Matches(Key.GlobalReference, id.value, MatchMode.Identification);
            }

            public bool Matches(Key key, MatchMode matchMode = MatchMode.Relaxed)
            {
                if (key == null)
                    return false;
                return this.Matches(key.type, key.value, matchMode);
            }

            // validation

            public static AasValidationAction Validate(AasValidationRecordList results, Key k, Referable container)
            {
                // access
                if (results == null || container == null)
                    return AasValidationAction.No;

                var res = AasValidationAction.No;

                // check
                if (k == null)
                {
                    // violation case
                    results.Add(new AasValidationRecord(
                        AasValidationSeverity.SpecViolation, container,
                        "Key: is null",
                        () =>
                        {
                            res = AasValidationAction.ToBeDeleted;
                        }));
                }
                else
                {
                    // check type
                    var tf = AdminShellUtil.CheckIfInConstantStringArray(KeyElements, k.type);
                    if (tf == AdminShellUtil.ConstantFoundEnum.No)
                        // violation case
                        results.Add(new AasValidationRecord(
                            AasValidationSeverity.SchemaViolation, container,
                            "Key: type is not in allowed enumeration values",
                            () =>
                            {
                                k.type = GlobalReference;
                            }));
                    if (tf == AdminShellUtil.ConstantFoundEnum.AnyCase)
                        // violation case
                        results.Add(new AasValidationRecord(
                            AasValidationSeverity.SchemaViolation, container,
                            "Key: type in wrong casing",
                            () =>
                            {
                                k.type = AdminShellUtil.CorrectCasingForConstantStringArray(
                                    KeyElements, k.type);
                            }));
                }

                // may give result "to be deleted"
                return res;
            }
        }

    }
}
