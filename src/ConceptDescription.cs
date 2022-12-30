/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace AasxCompatibilityModels
{
    #region AdminShell_V2_0

    public partial class AdminShellV20
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

#if __not_anymore

        [XmlElement(ElementName = "embeddedDataSpecification")]
        
        public EmbeddedDataSpecification embeddedDataSpecification = new EmbeddedDataSpecification();
#else
            // According to Spec V2.0.1, a ConceptDescription might feature alos multiple data specifications
            /* TODO (MIHO, 2020-08-30): align wording of the member ("embeddedDataSpecification") with the 
                * wording of the other entities ("hasDataSpecification") */
            [XmlElement(ElementName = "embeddedDataSpecification")]
            
            public HasDataSpecification embeddedDataSpecification = null;
#endif

            [XmlIgnore]
            [JsonProperty(PropertyName = "embeddedDataSpecifications")]
            public EmbeddedDataSpecification[] JsonEmbeddedDataSpecifications
            {
                get
                {
                    return this.embeddedDataSpecification?.ToArray();
                }
                set
                {
                    embeddedDataSpecification = new HasDataSpecification(value);
                }
            }

            // old

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
                    this.embeddedDataSpecification = new HasDataSpecification(src.embeddedDataSpecification);
                if (src.isCaseOf != null)
                    foreach (var ico in src.isCaseOf)
                    {
                        if (this.isCaseOf == null)
                            this.isCaseOf = new List<Reference>();
                        this.isCaseOf.Add(new Reference(ico));
                    }
            }

#if !DoNotUseAasxCompatibilityModels
            public ConceptDescription(AasxCompatibilityModels.AdminShellV10.ConceptDescription src)
                : base(src)
            {
                if (src.embeddedDataSpecification != null)
                {
                    this.embeddedDataSpecification = new HasDataSpecification();
                    this.embeddedDataSpecification.Add(new EmbeddedDataSpecification(src.embeddedDataSpecification));
                }
                if (src.IsCaseOf != null)
                    foreach (var ico in src.IsCaseOf)
                    {
                        if (this.isCaseOf == null)
                            this.isCaseOf = new List<Reference>();
                        this.isCaseOf.Add(new Reference(ico));
                    }
            }
#endif

            public static ConceptDescription CreateNew(
                string idShort, string idType, string id, string version = null, string revision = null)
            {
                var cd = new ConceptDescription();
                cd.idShort = idShort;
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

            public Key GetSingleKey()
            {
                return Key.CreateNew(this.GetElementName(), true, this.identification.idType, this.identification.id);
            }

            public ConceptDescriptionRef GetCdReference()
            {
                var r = new ConceptDescriptionRef();
                r.Keys.Add(
                    Key.CreateNew(
                        this.GetElementName(), true, this.identification.idType, this.identification.id));
                return r;
            }

            public Reference GetReference()
            {
                return GetCdReference();
            }

            public void SetIEC61360Spec(
                string[] preferredNames = null,
                string shortName = "",
                string unit = "",
                UnitId unitId = null,
                string valueFormat = null,
                string sourceOfDefinition = null,
                string symbol = null,
                string dataType = "",
                string[] definition = null
            )
            {
                var eds = new EmbeddedDataSpecification(new DataSpecificationRef(), new DataSpecificationContent());
                eds.dataSpecification.Keys.Add(
                    DataSpecificationIEC61360.GetKey());
                eds.dataSpecificationContent.dataSpecificationIEC61360 =
                    DataSpecificationIEC61360.CreateNew(
                        preferredNames, shortName, unit, unitId, valueFormat, sourceOfDefinition, symbol,
                        dataType, definition);

                this.embeddedDataSpecification = new HasDataSpecification();
                this.embeddedDataSpecification.Add(eds);

                this.AddIsCaseOf(
                    Reference.CreateNew(
                        new Key("ConceptDescription", false, this.identification.idType, this.identification.id)));
            }

            public DataSpecificationIEC61360 GetIEC61360()
            {
                return this.embeddedDataSpecification?.IEC61360Content;
            }

            // as experimental approach, forward the IEC getter/sett of hasDataSpec directly

            [XmlIgnore]
            
            public EmbeddedDataSpecification IEC61360DataSpec
            {
                get
                {
                    return this.embeddedDataSpecification?.IEC61360;
                }
                set
                {
                    // add embeddedDataSpecification first?
                    if (this.embeddedDataSpecification == null)
                        this.embeddedDataSpecification = new HasDataSpecification();
                    this.embeddedDataSpecification.IEC61360 = value;
                }
            }

            [XmlIgnore]
            
            public DataSpecificationIEC61360 IEC61360Content
            {
                get
                {
                    return this.embeddedDataSpecification?.IEC61360Content;
                }
                set
                {
                    // add embeddedDataSpecification first?
                    if (this.embeddedDataSpecification == null)
                        this.embeddedDataSpecification = new HasDataSpecification();

                    // check, if e IEC61360 can be found
                    var eds = this.embeddedDataSpecification.IEC61360;

                    // if already there, update
                    if (eds != null)
                    {
                        eds.dataSpecificationContent = new DataSpecificationContent();
                        eds.dataSpecificationContent.dataSpecificationIEC61360 = value;
                        return;
                    }

                    // no: add a full record
                    eds = EmbeddedDataSpecification.CreateIEC61360WithContent();
                    eds.dataSpecificationContent.dataSpecificationIEC61360 = value;
                    this.embeddedDataSpecification.Add(eds);
                }
            }

            public DataSpecificationIEC61360 CreateDataSpecWithContentIec61360()
            {
                var eds = EmbeddedDataSpecification.CreateIEC61360WithContent();
                if (this.embeddedDataSpecification == null)
                    this.embeddedDataSpecification = new HasDataSpecification();
                this.embeddedDataSpecification.Add(eds);
                return eds.dataSpecificationContent?.dataSpecificationIEC61360;
            }

            public string GetDefaultPreferredName(string defaultLang = null)
            {
                return "" +
                    GetIEC61360()?
                        .preferredName?.GetDefaultStr(defaultLang);
            }

            public string GetDefaultShortName(string defaultLang = null)
            {
                return "" +
                    GetIEC61360()?
                        .shortName?.GetDefaultStr(defaultLang);
            }

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("ConceptDescription", "CD");
            }

            public Tuple<string, string> ToCaptionInfo()
            {
                var caption = "";
                if (this.idShort != null && this.idShort.Trim() != "")
                    caption = $"\"{this.idShort.Trim()}\"";
                if (this.identification != null)
                    caption = (caption + " " + this.identification).Trim();

                var info = "" + GetDefaultShortName();

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

                  public IEnumerable<Reference> FindAllReferences()
            {
                yield break;
            }
        }



    }

    #endregion
}

