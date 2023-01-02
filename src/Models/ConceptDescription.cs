
namespace AdminShell
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml;
    using System.Xml.Serialization;

    public class ConceptDescription : Identifiable
        {
            [DataMember(Name = "embeddedDataSpecifications")]
            public List<EmbeddedDataSpecification> EmbeddedDataSpecifications { get; set; }

            [DataMember(Name = "isCaseOf")]
            public List<Reference> IsCaseOf { get; set; }

            [XmlIgnore]
            private List<ModelReference> isCaseOf = null;


            [XmlElement(ElementName = "isCaseOf")]
            [JsonProperty(PropertyName = "isCaseOf")]
            public List<ModelReference> IsCaseOf
            {
                get { return isCaseOf; }
                set { isCaseOf = value; }
            }

            public ConceptDescription()
                : base() { }

            public ConceptDescription(ConceptDescription src)
                : base(src)
            {
                if (src.EmbeddedDataSpecification != null)
                    this.EmbeddedDataSpecification = new HasDataSpecification(src.EmbeddedDataSpecification);
                if (src.isCaseOf != null)
                    foreach (var ico in src.isCaseOf)
                    {
                        if (this.isCaseOf == null)
                            this.isCaseOf = new List<ModelReference>();
                        this.isCaseOf.Add(new ModelReference(ico));
                    }
            }

            public static ConceptDescription CreateNew(
                string idShort, string idType, string id, string version = null, string revision = null)
            {
                var cd = new ConceptDescription();
                cd.idShort = idShort;
                cd.id.value = id;
                if (version != null)
                {
                    if (cd.administration == null)
                        cd.administration = new Administration();
                    cd.administration.Version = version;
                    cd.administration.Revision = revision;
                }
                return (cd);
            }

            public Key GetSingleKey()
            {
                return Key.CreateNew(this.GetElementName(), this.id.value);
            }

            public Identifier GetSingleId()
            {
                return new Identifier(this.id.value);
            }

            /// <summary>
            /// In order to be semantically precise, use this id to figure out
            /// the single id zo be put in a semantic id.
            /// </summary>
            public Identifier GetSemanticId()
            {
                return new Identifier(this.id.value);
            }

            public ConceptDescriptionRef GetCdReference()
            {
                var r = new ConceptDescriptionRef();
                r.Keys.Add(
                    Key.CreateNew(this.GetElementName(), this.id.value));
                return r;
            }

            public DataSpecificationIEC61360 GetIEC61360()
            {
                return this.EmbeddedDataSpecification?.IEC61360Content;
            }

            [XmlIgnore]
            public EmbeddedDataSpecification IEC61360DataSpec
            {
                get
                {
                    return this.EmbeddedDataSpecification?.IEC61360;
                }
                set
                {
                    // add EmbeddedDataSpecification first?
                    if (this.EmbeddedDataSpecification == null)
                        this.EmbeddedDataSpecification = new HasDataSpecification();
                    this.EmbeddedDataSpecification.IEC61360 = value;
                }
            }

            [XmlIgnore]

            public DataSpecificationIEC61360 IEC61360Content
            {
                get
                {
                    return this.EmbeddedDataSpecification?.IEC61360Content;
                }
                set
                {
                    // add EmbeddedDataSpecification first?
                    if (this.EmbeddedDataSpecification == null)
                        this.EmbeddedDataSpecification = new HasDataSpecification();

                    // check, if e IEC61360 can be found
                    var eds = this.EmbeddedDataSpecification.IEC61360;

                    // if already there, update
                    if (eds != null)
                    {
                        eds.dataSpecificationContent = new DataSpecificationContent();
                        eds.dataSpecificationContent.dataSpecificationIEC61360 = value;
                        return;
                    }

                    // no: add a full record
                    eds = AdminShell.EmbeddedDataSpecification.CreateIEC61360WithContent();
                    eds.dataSpecificationContent.dataSpecificationIEC61360 = value;
                    this.EmbeddedDataSpecification.Add(eds);
                }
            }

            public DataSpecificationIEC61360 CreateDataSpecWithContentIec61360()
            {
                var eds = AdminShell.EmbeddedDataSpecification.CreateIEC61360WithContent();
                if (this.EmbeddedDataSpecification == null)
                    this.EmbeddedDataSpecification = new HasDataSpecification();
                this.EmbeddedDataSpecification.Add(eds);
                return eds.dataSpecificationContent?.dataSpecificationIEC61360;
            }

            public string GetDefaultShortName(string defaultLang = null)
            {
                return "" +
                    GetIEC61360()?
                        .shortName?.GetDefaultStr(defaultLang);
            }

            public AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("ConceptDescription", "CD");
            }

            public Tuple<string, string> ToCaptionInfo()
            {
                var caption = "";
                if (this.idShort != null && this.idShort.Trim() != "")
                    caption = $"\"{this.idShort.Trim()}\"";
                if (this.id != null)
                    caption = (caption + " " + this.id).Trim();

                var info = "" + GetDefaultShortName();

                return Tuple.Create(caption, info);
            }

            public override string ToString()
            {
                var ci = ToCaptionInfo();
                return string.Format("{0}{1}", ci.Item1, (ci.Item2 != "") ? " / " + ci.Item2 : "");
            }

            public void AddIsCaseOf(ModelReference ico)
            {
                if (isCaseOf == null)
                    isCaseOf = new List<ModelReference>();
                isCaseOf.Add(ico);
            }

            public static IDisposable CreateNew()
            {
                throw new NotImplementedException();
            }

            // validation

            public override void Validate(AasValidationRecordList results)
            {
                // access
                if (results == null)
                    return;

                // check CD itself
                base.Validate(results);

                // check IEC61360 spec
                var eds61360 = this.IEC61360DataSpec;
                if (eds61360 != null)
                {
                    // check data spec
                    if (eds61360.dataSpecification == null ||
                        !(eds61360.dataSpecification.MatchesExactlyOneId(DataSpecificationIEC61360.GetIdentifier())))
                        results.Add(new AasValidationRecord(
                            AasValidationSeverity.SpecViolation, this,
                            "HasDataSpecification: data specification content set to IEC61360, but no " +
                            "data specification reference set!",
                            () =>
                            {
                                eds61360.dataSpecification = new DataSpecificationRef(
                                    new GlobalReference(
                                        DataSpecificationIEC61360.GetIdentifier()));
                            }));

                    // validate content
                    if (eds61360.dataSpecificationContent?.dataSpecificationIEC61360 == null)
                    {
                        results.Add(new AasValidationRecord(
                            AasValidationSeverity.SpecViolation, this,
                            "HasDataSpecification: data specification reference set to IEC61360, but no " +
                            "data specification content set!",
                            () =>
                            {
                                eds61360.dataSpecificationContent = new DataSpecificationContent();
                                eds61360.dataSpecificationContent.dataSpecificationIEC61360 =
                                new DataSpecificationIEC61360();
                            }));
                    }
                    else
                    {
                        // validate
                        eds61360.dataSpecificationContent.dataSpecificationIEC61360.Validate(results, this);
                    }
                }
            }

            // more find

            public IEnumerable<ModelReference> FindAllReferences()
            {
                yield break;
            }
        }

    }
}
