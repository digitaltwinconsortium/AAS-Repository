/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using AdminShellNS;
using Newtonsoft.Json;

//namespace AdminShellNS
namespace AdminShell_V30
{
    public partial class AdminShellV30
    {
        //
        // AAS
        //

        public class AdministrationShell : Identifiable, IFindAllReferences
        {
            // for JSON only
            [XmlIgnore]
            [JsonProperty(PropertyName = "modelType")]
            public JsonModelTypeWrapper JsonModelType { get { return new JsonModelTypeWrapper(GetElementName()); } }

            // from hasDataSpecification:
            [XmlElement(ElementName = "hasDataSpecification")]
            public HasDataSpecification hasDataSpecification = null;

            // from this very class
            public AssetAdministrationShellRef derivedFrom = null;

            public AssetInformation assetInformation = null;

            [JsonProperty(PropertyName = "submodels")]

            public List<SubmodelRef> submodelRefs = new List<SubmodelRef>();

            // constructors

            public AdministrationShell() { }

            public AdministrationShell(string idShort) : base(idShort) { }

            public AdministrationShell(AdministrationShell src)
                : base(src)
            {
                if (src != null)
                {
                    if (src.hasDataSpecification != null)
                        this.hasDataSpecification = new HasDataSpecification(src.hasDataSpecification);

                    if (src.derivedFrom != null)
                        this.derivedFrom = new AssetAdministrationShellRef(src.derivedFrom);

                    if (src.submodelRefs != null)
                        foreach (var smr in src.submodelRefs)
                            this.submodelRefs.Add(new SubmodelRef(smr));
                }
            }

#if !DoNotUseAasxCompatibilityModels
            public AdministrationShell(
                AasxCompatibilityModels.AdminShellV10.AdministrationShell src,
                AasxCompatibilityModels.AdminShellV10.AdministrationShellEnv srcenv)
                : base(src)
            {
                if (src.hasDataSpecification != null)
                    this.hasDataSpecification = new HasDataSpecification(src.hasDataSpecification);

                if (src.derivedFrom != null)
                    this.derivedFrom = new AssetAdministrationShellRef(src.derivedFrom);

                if (src.submodelRefs != null)
                    foreach (var smr in src.submodelRefs)
                        this.submodelRefs.Add(new SubmodelRef(smr));

                // now locate the Asset in the old environment and set
                var srcasset = srcenv?.FindAsset(src.assetRef);
                if (srcasset != null)
                    assetInformation = new AssetInformation(srcasset);
            }

            public AdministrationShell(AasxCompatibilityModels.AdminShellV20.AdministrationShell src,
                AasxCompatibilityModels.AdminShellV20.AdministrationShellEnv srcenv)
                : base(src)
            {
                if (src != null)
                {
                    if (src.hasDataSpecification != null)
                        this.hasDataSpecification = new HasDataSpecification(src.hasDataSpecification);

                    if (src.derivedFrom != null)
                        this.derivedFrom = new AssetAdministrationShellRef(src.derivedFrom);

                    if (src.submodelRefs != null)
                        foreach (var smr in src.submodelRefs)
                            this.submodelRefs.Add(new SubmodelRef(smr));

                    // now locate the Asset in the old environment and set
                    var srcasset = srcenv?.FindAsset(src.assetRef);
                    if (srcasset != null)
                    {
                        assetInformation = new AssetInformation(srcasset);

                        this.AddExtension(new Extension()
                        {
                            name = "AAS2.0/MIGRATION",
                            valueType = "application/json",
                            value = JsonConvert.SerializeObject(srcasset, Newtonsoft.Json.Formatting.Indented)
                        });
                    }
                }
            }
#endif

            public static AdministrationShell CreateNew(
                string idShort, string idType, string id, string version = null, string revision = null)
            {
                var s = new AdministrationShell();
                s.idShort = idShort;
                if (version != null)
                    s.SetAdminstration(version, revision);
                s.id.value = id;
                return (s);
            }

            // add

            public void AddDataSpecification(Identifier id)
            {
                if (hasDataSpecification == null)
                    hasDataSpecification = new HasDataSpecification();
                hasDataSpecification.Add(new EmbeddedDataSpecification(new GlobalReference(id)));
            }

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("AssetAdministrationShell", "AAS");
            }

            public Tuple<string, string> ToCaptionInfo()
            {
                var caption = AdminShellUtil.EvalToNonNullString("\"{0}\" ", idShort, "\"AAS\"");
                if (administration != null)
                    caption += "V" + administration.version + "." + administration.revision;

                var info = "";
                if (id != null)
                    info = $"[{id.value}]";
                return Tuple.Create(caption, info);
            }

            public override string ToString()
            {
                var ci = ToCaptionInfo();
                return string.Format("{0}{1}", ci.Item1, (ci.Item2 != "") ? " / " + ci.Item2 : "");
            }

            public SubmodelRef FindSubmodelRef(Identifier refid)
            {
                if (this.submodelRefs == null)
                    return null;
                foreach (var r in this.submodelRefs)
                    if (r.Matches(refid))
                        return r;
                return null;
            }

            public bool HasSubmodelRef(SubmodelRef newref)
            {
                // check, if existing
                if (this.submodelRefs == null)
                    return false;
                var found = false;
                foreach (var r in this.submodelRefs)
                    if (r.Matches(newref))
                        found = true;

                return found;
            }

            public void AddSubmodelRef(SubmodelRef newref)
            {
                if (this.submodelRefs == null)
                    this.submodelRefs = new List<SubmodelRef>();
                this.submodelRefs.Add(newref);
            }

            public IEnumerable<LocatedReference> FindAllReferences()
            {
                // Submodel references
                if (this.submodelRefs != null)
                    foreach (var r in this.submodelRefs)
                        yield return new LocatedReference(this, r);
            }

            public ModelReference GetReference()
            {
                return ModelReference.CreateNew(new Key(GetElementName(), "" + id?.value));
            }
        }

    }
}
