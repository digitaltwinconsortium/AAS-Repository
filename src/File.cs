/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;


//namespace AdminShellNS
namespace AdminShell_V30
{
    public partial class AdminShellV30
    {
        public class File : DataElement
        {
            // for JSON only
            [XmlIgnore]
            [JsonProperty(PropertyName = "modelType")]
            public new JsonModelTypeWrapper JsonModelType
            {
                get { return new JsonModelTypeWrapper(GetElementName()); }
            }

            // members

            [MetaModelName("File.mimeType")]
            [TextSearchable]
            [CountForHash]
            public string mimeType = "";

            [MetaModelName("File.value")]
            [TextSearchable]
            [CountForHash]
            public string value = "";

            // constructors

            public File() { }

            public File(SubmodelElement src)
                : base(src)
            {
                if (!(src is File fil))
                    return;

                this.mimeType = fil.mimeType;
                this.value = fil.value;
            }

#if !DoNotUseAasxCompatibilityModels
            public File(AasxCompatibilityModels.AdminShellV10.File src)
                : base(src)
            {
                if (src == null)
                    return;

                this.mimeType = src.mimeType;
                this.value = src.value;
            }

            public File(AasxCompatibilityModels.AdminShellV20.File src)
                : base(src)
            {
                if (src == null)
                    return;

                this.mimeType = src.mimeType;
                this.value = src.value;
            }
#endif

            public static File CreateNew(string idShort = null, string category = null, Identifier semanticIdKey = null)
            {
                var x = new File();
                x.CreateNewLogic(idShort, category, semanticIdKey);
                return (x);
            }

            public void Set(string mimeType = "", string value = "")
            {
                this.mimeType = mimeType;
                this.value = value;
            }

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("File", "File",
                    SubmodelElementWrapper.AdequateElementEnum.File);
            }

            public static string[] GetPopularMimeTypes()
            {
                return
                    new[] {
                    System.Net.Mime.MediaTypeNames.Text.Plain,
                    System.Net.Mime.MediaTypeNames.Text.Xml,
                    System.Net.Mime.MediaTypeNames.Text.Html,
                    "application/json",
                    "application/rdf+xml",
                    System.Net.Mime.MediaTypeNames.Application.Pdf,
                    System.Net.Mime.MediaTypeNames.Image.Jpeg,
                    "image/png",
                    System.Net.Mime.MediaTypeNames.Image.Gif,
                    "application/iges",
                    "application/step"
                    };
            }

            public override string ValueAsText(string defaultLang = null)
            {
                return "" + value;
            }

            public override object ToValueOnlySerialization()
            {
                var output = new Dictionary<string, Dictionary<string, string>>();
                var valueDict = new Dictionary<string, string>
                {
                    { "mimeType", mimeType },
                    { "value", value }
                };

                output.Add(idShort, valueDict);
                return output;
            }
        }

    }
}
