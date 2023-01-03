/*
Copyright (c) 2018-2019 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Schema;

namespace AdminShell
{
    /// <summary>
    /// validates the XML content against the AASX XML schema.
    ///
    /// Please produce instances with <see cref="AasSchemaValidation.NewXmlValidator"/>.
    /// </summary>
    public class XmlValidator
    {
        private XmlSchemaSet xmlSchemaSet;

        internal XmlValidator(XmlSchemaSet xmlSchemaSet)
        {
            this.xmlSchemaSet = xmlSchemaSet;
        }

        /// <summary>
        /// validates the given XML content and stores the results in the <paramref name="recs"/>.
        /// </summary>
        /// <param name="recs">Validation records</param>
        /// <param name="xmlContent">Content to be validated</param>
        public void Validate(List<AasValidationRecord> recs, Stream xmlContent)
        {
            if (recs == null)
                throw new ArgumentException($"Unexpected null {nameof(recs)}");

            if (xmlContent == null)
                throw new ArgumentException($"Unexpected null {nameof(xmlContent)}");

            // load/ validate on same records
            var settings = new System.Xml.XmlReaderSettings();
            settings.ValidationType = System.Xml.ValidationType.Schema;
            settings.Schemas = xmlSchemaSet;

            settings.ValidationEventHandler +=
                (object sender, System.Xml.Schema.ValidationEventArgs e) =>
                {
                    recs.Add(
                        new AasValidationRecord(
                            AasValidationSeverity.Serialization, null,
                        $"XML: {e?.Exception?.LineNumber}, {e?.Exception?.LinePosition}: {e?.Message}"));
                };

            // use the xml stream
            using (var reader = System.Xml.XmlReader.Create(xmlContent, settings))
            {
                while (reader.Read())
                {
                    // Invoke callbacks
                };
            }
        }
    }
}
