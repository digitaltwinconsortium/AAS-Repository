
namespace AdminShell
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Xml;

    public static class AasSchemaValidation
    {
        public enum SerializationFormat { XML, JSON }

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

            var newRecs = new AasValidationRecordList();

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

        /// <summary>
        /// creates an XML validator and applies it on the given content.
        ///
        /// If you repeatedly need to validate XML against a schema, re-use an instance of
        /// <see cref="XmlValidator"/> produced with <see cref="NewXmlValidator"/>.
        /// </summary>
        /// <param name="recs">Validation records</param>
        /// <param name="xmlContent">Content to be validated</param>
        public static void ValidateXML(AasValidationRecordList recs, Stream xmlContent)
        {
            var validator = NewXmlValidator();
            validator.Validate(recs, xmlContent);
        }

        public static int ValidateJSONAlternative(AasValidationRecordList recs, Stream jsonContent)
        {
            // see: https://github.com/RicoSuter/NJsonSchema/wiki/JsonSchemaValidator
            var newRecs = new AasValidationRecordList();

            // access
            if (recs == null || jsonContent == null)
                return -1;

            // Load the schema files
            // right now: exactly ONE schema file
            var files = GetSchemaResources(SerializationFormat.JSON);
            if (files == null || files.Length != 1)
                return -1;

            NJsonSchema.JsonSchema schema = null;

            try
            {
                Assembly myAssembly = Assembly.GetExecutingAssembly();
                foreach (var schemaFn in files)
                {
                    using (Stream schemaStream = myAssembly.GetManifestResourceStream(schemaFn))
                    {
                        using (var streamReader = new StreamReader(schemaStream))
                        {
                            var allTxt = streamReader.ReadToEnd();
                            schema = NJsonSchema.JsonSchema.FromJsonAsync(allTxt).GetAwaiter().GetResult();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new FileNotFoundException("ValidateJSON: Error loading schema: " +
                    ex.Message);
            }

            if (schema == null)
            {
                throw new FileNotFoundException("ValidateJSON: Schema not found properly.");
            }

            // create validator
            var validator = new NJsonSchema.Validation.JsonSchemaValidator();

            // load the JSON content
            string jsonTxt = null;
            try
            {
                using (var streamReader = new StreamReader(jsonContent))
                {
                    jsonTxt = streamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("ValidateJSON: Error loading JSON content: " +
                    ex.Message);
            }

            if (jsonTxt == null || jsonTxt == "")
                throw new InvalidOperationException("ValidateJSON: Error loading JSON content gave null.");

            // validate
            ICollection<NJsonSchema.Validation.ValidationError> errors;
            try
            {
                errors = validator.Validate(jsonTxt, schema);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("ValidateJSON: Error when validating: " +
                    ex.Message);
            }

            // re-format messages
            if (errors != null)
                foreach (var ve in errors)
                {
                    var msg = ("" + ve.ToString());
                    msg = Regex.Replace(msg, @"\s+", " ");
                    newRecs.Add(new AasValidationRecord(AasValidationSeverity.Serialization, null,
                        $"JSON: {ve.LineNumber,5},{ve.LinePosition:3}: {msg}"));
                }

            // result
            recs.AddRange(newRecs);
            return newRecs.Count;
        }
    }
}
