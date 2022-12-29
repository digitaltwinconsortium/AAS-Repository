/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using AdminShellNS;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

//namespace AdminShellNS
namespace AdminShell_V30
{
    public partial class AdminShellV30
    {
        //
        // Identifier
        //

        /// <summary>
        /// V30
        /// Thic class is the "old" Identification class of meta-model V2.0
        /// It did contain two attributes "idType" and "id"
        /// As string is sealed, this class cannot derive dirctly from string,
        /// so an implicit conversion is tested
        /// </summary>
        public class Identifier
        {

            // members

            [XmlText]
            [CountForHash]
            public string value = "";

            // implicit operators

            public static implicit operator string(Identifier d)
            {
                return d.value;
            }

            public static implicit operator Identifier(string d)
            {
                return new Identifier(d);
            }

            // some constants

            public static string IRDI = "IRDI";
            public static string IRI = "IRI";
            public static string IdShort = "IdShort";

            // constructors

            public Identifier() { }

            public Identifier(Identifier src)
            {
                this.value = src.value;
            }

#if !DoNotUseAasxCompatibilityModels
            public Identifier(AasxCompatibilityModels.AdminShellV10.Identification src)
            {
                this.value = src.id;
            }

            public Identifier(AasxCompatibilityModels.AdminShellV20.Identification src)
            {
                this.value = src.id;
            }
#endif

            public Identifier(string id)
            {
                this.value = id;
            }

            public Identifier(Key key)
            {
                this.value = key.value;
            }

            // Creator with validation

            public static Identifier CreateNew(string id)
            {
                return new Identifier(id);
            }

            // further

            public bool IsEqual(Identifier other)
            {
                return this.value.Trim().ToLower() == other.value.Trim().ToLower();
            }

            public static bool IsIRI(string value)
            {
                if (value == null)
                    return false;
                var m = Regex.Match(value, @"\s*(\w+)://");
                return m.Success;
            }

            public bool IsIRI()
            {
                return IsIRI(value);
            }

            public static bool IsIRDI(string value)
            {
                if (value == null)
                    return false;
                var m = Regex.Match(value, @"\s*(\d{3,4})(:|-|/)");
                return m.Success;
            }

            public bool IsIRDI()
            {
                return IsIRDI(value);
            }

            // Matching

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
                return this.Matches(key.value, matchMode);
            }

            // validation

            public static AasValidationAction Validate(
                AasValidationRecordList results, Identifier id, Referable container)
            {
                // access
                if (results == null || container == null)
                    return AasValidationAction.No;

                var res = AasValidationAction.No;

                // check
                if (id?.value == null)
                {
                    // violation case
                    results.Add(new AasValidationRecord(
                        AasValidationSeverity.SpecViolation, container,
                        "Value: is null",
                        () =>
                        {
                            res = AasValidationAction.ToBeDeleted;
                        }));
                }

                // may give result "to be deleted"
                return res;
            }

            // Other

            public override string ToString()
            {
                return value;
            }
        }

    }
}
