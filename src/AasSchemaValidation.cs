
namespace AdminShell
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml;

    public static class AasSchemaValidation
    {
        public enum SerializationFormat
        {
            XML,
            JSON
        }

        public static string[] GetSchemaResources(SerializationFormat fmt)
        {
            if (fmt == SerializationFormat.XML)
            {
                return new[]
                {
                    "AdminShell.Resources.schemaV201.AAS.xsd",
                    "AdminShell.Resources.schemaV201.AAS_ABAC.xsd",
                    "AdminShell.Resources.schemaV201.IEC61360.xsd"
                };
            }
            if (fmt == SerializationFormat.JSON)
            {
                return new[]
                {
                    "AdminShell.Resources.schemaV201.aas.json"
                };
            }
            return null;
        }

        /// <summary>
        /// produces a validator which validates XML AASX files.
        /// </summary>
        /// <returns>initialized validator</returns>
        public static XmlValidator NewXmlValidator()
        {
            // Load the schema files
            var files = GetSchemaResources(SerializationFormat.XML);
            if (files == null)
                throw new InvalidOperationException("No XML schema files could be found in the resources.");

            var xmlSchemaSet = new System.Xml.Schema.XmlSchemaSet();
            xmlSchemaSet.XmlResolver = new System.Xml.XmlUrlResolver();

            try
            {
                Assembly myAssembly = Assembly.GetExecutingAssembly();
                foreach (var schemaFn in files)
                {
                    using (Stream schemaStream = myAssembly.GetManifestResourceStream(schemaFn))
                    {
                        using (XmlReader schemaReader = XmlReader.Create(schemaStream))
                        {
                            xmlSchemaSet.Add(null, schemaReader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FileNotFoundException(
                    $"Error accessing embedded resource schema files: {ex.Message}");
            }

            var newRecs = new List<AasValidationRecord>();

            // set up messages
            xmlSchemaSet.ValidationEventHandler += (object sender, System.Xml.Schema.ValidationEventArgs e) =>
            {
                newRecs.Add(
                    new AasValidationRecord(
                    AasValidationSeverity.Serialization, null,
                    $"{e?.Exception?.LineNumber}, {e?.Exception?.LinePosition}: {e?.Message}"));
            };

            // compile
            try
            {
                xmlSchemaSet.Compile();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Error compiling schema files: {ex.Message}");
            }

            if (newRecs.Count > 0)
            {
                var parts = new List<string> { $"Failed to compile the schema files:" };
                parts.AddRange(newRecs.Select<AasValidationRecord, string>((r) => r.Message));
                throw new InvalidOperationException(string.Join(Environment.NewLine, parts));
            }

            return new XmlValidator(xmlSchemaSet);
        }
    }
}
