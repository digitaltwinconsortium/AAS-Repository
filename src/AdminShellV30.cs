/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;
using System.Xml.Serialization;

//namespace AdminShellNS
namespace AdminShell_V30
{
    /// <summary>
    /// This empty class derives always from the current version of the Administration Shell class hierarchy.
    /// </summary>
    public class AdminShell : AdminShellV30 { }

    #region AdminShell_V3_0

    public partial class AdminShellV30
    {
        //
        // Version
        //

        /// <summary>
        /// Major version of the meta-model
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public static string MetaModelVersionCoarse = "AAS3.0";

        /// <summary>
        /// Minor version (extension) of the meta-model.
        /// Should be added to <c>MetaModelVersionCoarse</c>
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public static string MetaModelVersionFine = "RC02";

        //
        // Attributes
        //

        /// <summary>
        /// This attribute indicates, that it should e.g. serialized in JSON.
        /// </summary>
        [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
        public class CountForHash : System.Attribute
        {
        }

        /// <summary>
        /// This attribute indicates, that evaluation shall not count following field or not dive into references.
        /// </summary>
        [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
        public class SkipForHash : System.Attribute
        {
        }

        /// <summary>
        /// This attribute indicates, that the field / property shall be skipped for reflection
        /// in order to avoid cycles
        /// </summary>
        [System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property, AllowMultiple = true)]
        public class SkipForReflection : System.Attribute
        {
        }

        /// <summary>
        /// This attribute indicates, that the field / property shall be skipped for searching, because it is not
        /// directly displayed in Package Explorer
        /// </summary>
        [System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property, AllowMultiple = true)]
        public class SkipForSearch : System.Attribute
        {
        }

        /// <summary>
        /// This attribute indicates, that the field / property is searchable
        /// </summary>
        [System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property, AllowMultiple = true)]
        public class TextSearchable : System.Attribute
        {
        }

    }

    #endregion
}

