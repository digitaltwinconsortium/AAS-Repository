#define UseAasxCompatibilityModels

using System;
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
        public class ConceptDescription : Identifiable, System.IDisposable
        {
            // for JSON only
            [XmlIgnore]
            [JsonProperty(PropertyName = "modelType")]
            public JsonModelTypeWrapper JsonModelType { get { return new JsonModelTypeWrapper(GetElementName()); } }

            // members

            // do this in order to be IDisposable, that is: suitable for (using)
            void System.IDisposable.Dispose() { }
            public void GetData() { }
            // from HasDataSpecification
            // TODO: in V1.0, shall be a list of embeddedDataSpecification
            [XmlElement(ElementName = "embeddedDataSpecification")]
            
            public EmbeddedDataSpecification embeddedDataSpecification = new EmbeddedDataSpecification();
            [XmlIgnore]
            [JsonProperty(PropertyName = "embeddedDataSpecifications")]
            public EmbeddedDataSpecification[] JsonEmbeddedDataSpecifications
            {
                get
                {
                    if (embeddedDataSpecification == null)
                        return null;
                    return new EmbeddedDataSpecification[] { embeddedDataSpecification };
                }
                set
                {
                    if (value == null)
                        embeddedDataSpecification = null;
                    else
                        embeddedDataSpecification = value[0];
                }
            }


            // old
            // [XmlElement(ElementName="conceptDefinitionRef")]
            // public Reference conceptDefinitionRef = null ;

            // this class
            [XmlIgnore]
            private List<Reference> isCaseOf = null;

            // getter / setter

            [XmlElement(ElementName = "isCaseOf")]
            [JsonProperty(PropertyName = "isCaseOf")]
            public List<Reference> IsCaseOf
            {
                get { return isCaseOf; }
                set { isCaseOf = value; }
            }

            // constructors / creators

            public ConceptDescription() : base() { }

            public ConceptDescription(ConceptDescription src)
                : base(src)
            {
                if (src.embeddedDataSpecification != null)
                    this.embeddedDataSpecification = new EmbeddedDataSpecification(src.embeddedDataSpecification);
                if (src.isCaseOf != null)
                    foreach (var ico in src.isCaseOf)
                    {
                        if (this.isCaseOf == null)
                            this.isCaseOf = new List<Reference>();
                        this.isCaseOf.Add(new Reference(ico));
                    }
            }

            public static ConceptDescription CreateNew(string idType, string id, string version = null, string revision = null)
            {
                var cd = new ConceptDescription();
                cd.identification.idType = idType;
                cd.identification.id = id;
                if (version != null)
                {
                    if (cd.administration == null)
                        cd.administration = new Administration();
                    cd.administration.version = version;
                    cd.administration.revision = revision;
                }
                return (cd);
            }

            public ConceptDescriptionRef GetReference()
            {
                var r = new ConceptDescriptionRef();
                r.Keys.Add(Key.CreateNew(this.GetElementName(), true, this.identification.idType, this.identification.id));
                return r;
            }

            public Key GetGlobalDataSpecRef()
            {
                if (embeddedDataSpecification.hasDataSpecification.Count != 1)
                    return null;
                return (embeddedDataSpecification.hasDataSpecification[0]);
            }

            public void SetIEC61360Spec(
                string[] preferredNames = null,
                string shortName = "",
                string unit = "",
                UnitId unitId = null,
                string valueFormat = null,
                string[] sourceOfDefinition = null,
                string symbol = null,
                string dataType = "",
                string[] definition = null
            )
            {
                this.embeddedDataSpecification = new EmbeddedDataSpecification();
                this.embeddedDataSpecification.hasDataSpecification.Keys.Add(Key.CreateNew("GlobalReference", false, "URI", "www.admin-shell.io/DataSpecificationTemplates/DataSpecificationIEC61360"));
                this.embeddedDataSpecification.dataSpecificationContent.dataSpecificationIEC61360 = AdminShellV10.DataSpecificationIEC61360.CreateNew(
                    preferredNames, shortName, unit, unitId, valueFormat, sourceOfDefinition, symbol, dataType, definition
                );
                this.AddIsCaseOf(Reference.CreateNew(new Key("ConceptDescription", false, this.identification.idType, this.identification.id)));
            }

            public DataSpecificationIEC61360 GetIEC61360()
            {
                if (embeddedDataSpecification != null && embeddedDataSpecification.dataSpecificationContent != null && embeddedDataSpecification.dataSpecificationContent.dataSpecificationIEC61360 != null)
                    return embeddedDataSpecification.dataSpecificationContent.dataSpecificationIEC61360;
                return null;
            }

            public string GetShortName()
            {
                if (embeddedDataSpecification != null && embeddedDataSpecification.dataSpecificationContent != null && embeddedDataSpecification.dataSpecificationContent.dataSpecificationIEC61360 != null)
                    return embeddedDataSpecification.dataSpecificationContent.dataSpecificationIEC61360.shortName;
                return "";
            }

            public override string GetElementName()
            {
                return "ConceptDescription";
            }

            public Tuple<string, string> ToCaptionInfo()
            {
                var caption = "";
                if (this.idShort != null && this.idShort.Trim() != "")
                    caption = $"\"{this.idShort.Trim()}\"";
                if (this.identification != null)
                    caption = (caption + " " + this.identification).Trim();

                var info = "";
                if (embeddedDataSpecification != null && embeddedDataSpecification.dataSpecificationContent != null && embeddedDataSpecification.dataSpecificationContent.dataSpecificationIEC61360 != null)
                    info += embeddedDataSpecification.dataSpecificationContent.dataSpecificationIEC61360.shortName;

                return Tuple.Create(caption, info);
            }

            public override string ToString()
            {
                var ci = ToCaptionInfo();
                return string.Format("{0}{1}", ci.Item1, (ci.Item2 != "") ? " / " + ci.Item2 : "");
            }

            public void AddIsCaseOf(Reference ico)
            {
                if (isCaseOf == null)
                    isCaseOf = new List<Reference>();
                isCaseOf.Add(ico);
            }

            public static IDisposable CreateNew()
            {
                throw new NotImplementedException();
            }
        }

    }

    #endregion
}

#endif