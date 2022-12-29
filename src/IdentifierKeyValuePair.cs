/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using System;
using System.Xml;
using System.Xml.Serialization;

//namespace AdminShellNS
namespace AdminShell_V30
{
    public partial class AdminShellV30
    {
        //
        // IdentifierKeyValuePair(s)
        //

        public class IdentifierKeyValuePair : IAasElement
        {
            // member
            // from hasSemantics:
            [XmlElement(ElementName = "semanticId")]
            public SemanticId semanticId = null;

            [MetaModelName("IdentifierKeyValuePair.key")]
            [TextSearchable]
            [CountForHash]
            public string key = "";

            [MetaModelName("IdentifierKeyValuePair.value")]
            [TextSearchable]
            [CountForHash]
            public string value = null;

            [CountForHash]
            public GlobalReference externalSubjectId = null;

            // constructors

            public IdentifierKeyValuePair() { }

            public IdentifierKeyValuePair(IdentifierKeyValuePair src)
            {
                if (src.semanticId != null)
                    this.semanticId = new SemanticId(src.semanticId);
                this.key = src.key;
                this.value = src.value;
                if (src.externalSubjectId != null)
                    this.externalSubjectId = new GlobalReference(src.externalSubjectId);
            }

#if !DoNotUseAasxCompatibilityModels
            // not existing in V2.0
#endif

            // further

            public string ToString(int format = 0)
            {
                if (format == 1)
                {
                    return String.Format(
                        "({0}){1}", this.key, this.value);
                }
                if (format == 2)
                {
                    return String.Format("{0}", this.value);
                }

                // (old) default
                return $"[{this.key}, {this.value}]";
            }

            // self description

            public AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("IdentifierKeyValuePair", "IKV");
            }

            public string GetElementName()
            {
                return this.GetSelfDescription()?.ElementName;
            }

        }

    }
}
