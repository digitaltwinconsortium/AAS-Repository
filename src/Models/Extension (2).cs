/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using System.Collections.Generic;
using System.Xml.Serialization;

//namespace AdminShell
namespace AdminShell_V30
{

    #region AdminShell_V3_0

    public partial class AdminShellV30
    {
        //
        // Extensions attached to each Referable
        //

        /// <summary>
        /// Single extension of an element. 
        /// </summary>
        public class Extension : IGetSemanticId
        {
            // members

            // from hasSemantics:
            [XmlElement(ElementName = "semanticId")]
            public SemanticId semanticId = null;
            public SemanticId GetSemanticId() => semanticId;

            // this class

            /// <summary>
            /// Name of the extension.  The  name of an extension within <c>ListOfExtension</c> 
            /// needs to be unique.
            /// </summary>
            public string name = "";

            /// <summary>
            /// Type of the value of the extension.
            /// </summary>
            public string valueType = "xsd:string";

            /// <summary>
            /// Value of the extension. In meta model this is ValueDataType, but in this SDK
            /// its just a plain string. Appropriate serialization needs to happen.
            /// Note: this string can VERY HUGE, i.e. an XML document on its own.
            /// </summary>
            public string value = "";

            /// <summary>
            /// Reference  to (multiple)  elements  the extension refers to.
            /// </summary>
            public List<ModelReference> refersTo = null;
        }

    }

    #endregion
}

