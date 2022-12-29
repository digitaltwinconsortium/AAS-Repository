#define UseAasxCompatibilityModels

using System.Collections.Generic;
using System.IO;
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
        [XmlRoot(ElementName = "aasenv", Namespace = "http://www.admin-shell.io/aas/1/0")]
        public class AdministrationShellEnv
        {
            [XmlAttribute(Namespace = System.Xml.Schema.XmlSchema.InstanceNamespace)]
            public string schemaLocation = "http://www.admin-shell.io/aas/1/0 AAS.xsd http://www.admin-shell.io/IEC61360/1/0 IEC61360.xsd";

            // [XmlElement(ElementName="assetAdministrationShells")]
            [XmlIgnore] // will be ignored, anyway
            private List<AdministrationShell> administrationShells = new List<AdministrationShell>();
            [XmlIgnore] // will be ignored, anyway
            private List<Asset> assets = new List<Asset>();
            [XmlIgnore] // will be ignored, anyway
            private List<Submodel> submodels = new List<Submodel>();
            [XmlIgnore] // will be ignored, anyway
            private List<ConceptDescription> conceptDescriptions = new List<ConceptDescription>();

            // getter / setters

            [XmlArray("assetAdministrationShells")]
            [XmlArrayItem("assetAdministrationShell")]
            [JsonProperty(PropertyName = "assetAdministrationShells")]
            public List<AdministrationShell> AdministrationShells
            {
                get { return administrationShells; }
                set { administrationShells = value; }
            }

            [XmlArray("assets")]
            [XmlArrayItem("asset")]
            [JsonProperty(PropertyName = "assets")]
            public List<Asset> Assets
            {
                get { return assets; }
                set { assets = value; }
            }

            [XmlArray("submodels")]
            [XmlArrayItem("submodel")]
            [JsonProperty(PropertyName = "submodels")]
            public List<Submodel> Submodels
            {
                get { return submodels; }
                set { submodels = value; }
            }

            [XmlArray("conceptDescriptions")]
            [XmlArrayItem("conceptDescription")]
            [JsonProperty(PropertyName = "conceptDescriptions")]
            public List<ConceptDescription> ConceptDescriptions
            {
                get { return conceptDescriptions; }
                set { conceptDescriptions = value; }
            }

            // finders

            public AdministrationShell FindAAS(Identification id)
            {
                if (id == null)
                    return null;
                foreach (var aas in this.AdministrationShells)
                    if (aas.identification != null && aas.identification.IsEqual(id))
                        return aas;
                return null;
            }

            public AdministrationShell FindAAS(string idShort)
            {
                if (idShort == null)
                    return null;
                foreach (var aas in this.AdministrationShells)
                    if (aas.idShort != null && aas.idShort.Trim().ToLower() == idShort.Trim().ToLower())
                        return aas;
                return null;
            }

            public AdministrationShell FindAASwithSubmodel(Identification smid)
            {
                if (smid == null)
                    return null;
                foreach (var aas in this.AdministrationShells)
                    if (aas.submodelRefs != null)
                        foreach (var smref in aas.submodelRefs)
                            if (smref.MatchesTo(smid))
                                return aas;
                return null;
            }

            public Asset FindAsset(Identification id)
            {
                if (id == null)
                    return null;
                foreach (var asset in this.Assets)
                    if (asset.identification != null && asset.identification.IsEqual(id))
                        return asset;
                return null;
            }

            public Asset FindAsset(AssetRef aref)
            {
                // trivial
                if (aref == null)
                    return null;
                // can only refs with 1 key
                if (aref.Count != 1)
                    return null;
                // and we're picky
                var key = aref[0];
                if (!key.local || key.type.ToLower().Trim() != "asset")
                    return null;
                // brute force
                foreach (var a in assets)
                    if (a.identification.idType.ToLower().Trim() == key.idType.ToLower().Trim()
                        && a.identification.id.ToLower().Trim() == key.value.ToLower().Trim())
                        return a;
                // uups 
                return null;
            }

            public Submodel FindSubmodel(Identification id)
            {
                if (id == null)
                    return null;
                foreach (var sm in this.Submodels)
                    if (sm.identification != null && sm.identification.IsEqual(id))
                        return sm;
                return null;
            }

            public Submodel FindSubmodel(SubmodelRef smref)
            {
                // trivial
                if (smref == null)
                    return null;
                // can only refs with 1 key
                if (smref.Count != 1)
                    return null;
                // and we're picky
                var key = smref.Keys[0];
                if (!key.local || key.type.ToLower().Trim() != "submodel")
                    return null;
                // brute force
                foreach (var sm in this.Submodels)
                    if (sm.identification.idType.ToLower().Trim() == key.idType.ToLower().Trim()
                        && sm.identification.id.ToLower().Trim() == key.value.ToLower().Trim())
                        return sm;
                // uups 
                return null;
            }

            /* OLD VERSION. OPC UA -> inadequate for finding all References
            public Referable FindReferableByReference(Reference rf, int keyIndex = 0)
            {
                // first index needs to exist ..
                if (rf == null || keyIndex >= rf.Count)
                    return null;

                // .. and point to an Submodel
                if (rf[keyIndex].type.Trim().ToLower() != Key.Submodel.Trim().ToLower())
                    return null;

                // ok, search Submodel
                var sm = this.FindSubmodel(new Identification(rf[keyIndex].idType, rf[keyIndex].value));
                if (sm == null)
                    return null;

                // go inside
                return SubmodelElementWrapper.FindReferableByReference(sm.submodelElements, rf, keyIndex + 1);
            }
            */

            public Referable FindReferableByReference(Reference rf, int keyIndex = 0)
            {
                // first index needs to exist ..
                if (rf == null || keyIndex >= rf.Count)
                    return null;

                // which type?
                var firstType = rf[keyIndex].type.Trim().ToLower();
                var firstIdentification = new Identification(rf[keyIndex].idType, rf[keyIndex].value);

                if (firstType == Key.AAS.Trim().ToLower())
                    return this.FindAAS(firstIdentification);

                if (firstType == Key.Asset.Trim().ToLower())
                    return this.FindAsset(firstIdentification);

                if (firstType == Key.ConceptDescription.Trim().ToLower())
                    return this.FindConceptDescription(firstIdentification);

                if (firstType == Key.Submodel.Trim().ToLower())
                {
                    // ok, search Submodel
                    var sm = this.FindSubmodel(new Identification(rf[keyIndex].idType, rf[keyIndex].value));
                    if (sm == null)
                        return null;

                    // at our end?
                    if (keyIndex >= rf.Count - 1)
                        return sm;

                    // go inside
                    return SubmodelElementWrapper.FindReferableByReference(sm.submodelElements, rf, keyIndex + 1);
                }

                // nothing in this Environment
                return null;
            }


            public ConceptDescription FindConceptDescription(ConceptDescriptionRef cdr)
            {
                if (cdr == null)
                    return null;
                return FindConceptDescription(cdr.Keys);
            }

            public ConceptDescription FindConceptDescription(Identification id)
            {
                var cdr = ConceptDescriptionRef.CreateNew("Conceptdescription", true, id.idType, id.id);
                return FindConceptDescription(cdr);
            }

            public ConceptDescription FindConceptDescription(List<Key> keys)
            {
                // trivial
                if (keys == null)
                    return null;
                // can only refs with 1 key
                if (keys.Count != 1)
                    return null;
                // and we're picky
                var key = keys[0];
                if (!key.local || key.type.ToLower().Trim() != "conceptdescription")
                    return null;
                // brute force
                foreach (var cd in conceptDescriptions)
                    if (cd.identification.idType.ToLower().Trim() == key.idType.ToLower().Trim()
                        && cd.identification.id.ToLower().Trim() == key.value.ToLower().Trim())
                        return cd;
                // uups 
                return null;
            }

            public ConceptDescription FindConceptDescription(Key key)
            {
                if (key == null)
                    return null;
                var l = new List<Key>();
                l.Add(key);
                return (FindConceptDescription(l));
            }

            // creators

            private void CopyConceptDescriptionsFrom(AdministrationShellEnv srcEnv, SubmodelElement src, bool shallowCopy = false)
            {
                // access
                if (srcEnv == null || src == null || src.semanticId == null)
                    return;
                // check for this SubmodelElement in Source
                var cdSrc = srcEnv.FindConceptDescription(src.semanticId.Keys);
                if (cdSrc == null)
                    return;
                // check for this SubmodelElement in Destnation (this!)
                var cdDest = this.FindConceptDescription(src.semanticId.Keys);
                if (cdDest != null)
                    return;
                // copy new
                this.ConceptDescriptions.Add(new ConceptDescription(cdSrc));
                // recurse?
                if (!shallowCopy && src is SubmodelElementCollection)
                    foreach (var m in (src as SubmodelElementCollection).value)
                        CopyConceptDescriptionsFrom(srcEnv, m.submodelElement, shallowCopy);

            }

            public SubmodelElementWrapper CopySubmodelElementAndCD(AdministrationShellEnv srcEnv, SubmodelElement srcElem, bool copyCD = false, bool shallowCopy = false)
            {
                // access
                if (srcEnv == null || srcElem == null)
                    return null;

                // 1st result pretty easy (calling function will add this to the appropriate Submodel)
                var res = new SubmodelElementWrapper(srcElem);

                // copy the CDs..
                if (copyCD)
                    CopyConceptDescriptionsFrom(srcEnv, srcElem, shallowCopy);

                // give back
                return res;
            }

            public SubmodelRef CopySubmodelRefAndCD(AdministrationShellEnv srcEnv, SubmodelRef srcSubRef, bool copySubmodel = false, bool copyCD = false, bool shallowCopy = false)
            {
                // access
                if (srcEnv == null || srcSubRef == null)
                    return null;

                // need to have the source Submodel
                var srcSub = srcEnv.FindSubmodel(srcSubRef);
                if (srcSub == null)
                    return null;

                // 1st result pretty easy (calling function will add this to the appropriate AAS)
                var dstSubRef = new SubmodelRef(srcSubRef);

                // get the destination and shall src != dst
                var dstSub = this.FindSubmodel(dstSubRef);
                if (srcSub == dstSub)
                    return null;

                // maybe we need the Submodel in our environment, as well
                if (dstSub == null && copySubmodel)
                {
                    dstSub = new Submodel(srcSub, shallowCopy);
                    this.Submodels.Add(dstSub);
                }
                else
                {
                    // there is already an submodel, just add members
                    if (!shallowCopy && srcSub.submodelElements != null)
                    {
                        if (dstSub.submodelElements == null)
                            dstSub.submodelElements = new List<SubmodelElementWrapper>();
                        foreach (var smw in srcSub.submodelElements)
                            dstSub.submodelElements.Add(new SubmodelElementWrapper(smw.submodelElement, shallowCopy));
                    }
                }

                // copy the CDs..
                if (copyCD && srcSub.submodelElements != null)
                    foreach (var smw in srcSub.submodelElements)
                        CopyConceptDescriptionsFrom(srcEnv, smw.submodelElement, shallowCopy);

                // give back
                return dstSubRef;
            }

            // serializations

            public void SerializeXmlToStream(StreamWriter s)
            {
                var serializer = new XmlSerializer(typeof(AdminShellV10.AdministrationShellEnv));
                var nss = new XmlSerializerNamespaces();
                nss.Add("xsi", System.Xml.Schema.XmlSchema.InstanceNamespace);
                nss.Add("aas", "http://www.admin-shell.io/aas/1/0");
                nss.Add("IEC61360", "http://www.admin-shell.io/IEC61360/1/0");
                serializer.Serialize(s, this, nss);
            }

            public JsonWriter SerialiazeJsonToStream(StreamWriter sw, bool leaveJsonWriterOpen = false)
            {
                sw.AutoFlush = true;

                JsonSerializer serializer = new JsonSerializer();
                serializer.NullValueHandling = NullValueHandling.Ignore;
                serializer.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
                serializer.Formatting = Newtonsoft.Json.Formatting.Indented;

                JsonWriter writer = new JsonTextWriter(sw);
                serializer.Serialize(writer, this);
                if (leaveJsonWriterOpen)
                    return writer;
                writer.Close();
                return null;
            }

            public AdministrationShellEnv DeserializeFromXmlStream(TextReader reader)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AdminShellV10.AdministrationShellEnv), "http://www.admin-shell.io/aas/1/0");
                var res = serializer.Deserialize(reader) as AdminShellV10.AdministrationShellEnv;
                return res;
            }

            public AdministrationShellEnv DeserializeFromJsonStream(TextReader reader)
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new AdminShellV10.JsonAasxConverter("modelType", "name"));
                var res = (AdministrationShellEnv)serializer.Deserialize(reader, typeof(AdministrationShellEnv));
                return res;
            }

            // special functions

            private static void CreateFromExistingEnvRecurseForCDs(AdministrationShellEnv src, List<SubmodelElementWrapper> wrappers, ref List<ConceptDescription> filterForCD)
            {
                if (wrappers == null || filterForCD == null)
                    return;

                foreach (var w in wrappers)
                {
                    // include in filter ..
                    if (w.submodelElement.semanticId != null)
                    {
                        var cd = src.FindConceptDescription(w.submodelElement.semanticId.Keys);
                        if (cd != null)
                            filterForCD.Add(cd);
                    }

                    // recurse?
                    if (w.submodelElement is SubmodelElementCollection)
                        CreateFromExistingEnvRecurseForCDs(src, (w.submodelElement as SubmodelElementCollection).value, ref filterForCD);

                    if (w.submodelElement is Operation)
                        for (int i = 0; i < 2; i++)
                        {
                            var w2s = Operation.GetWrappers((w.submodelElement as Operation)[i]);
                            CreateFromExistingEnvRecurseForCDs(src, w2s, ref filterForCD);
                        }

                }
            }

            public static AdministrationShellEnv CreateFromExistingEnv(AdministrationShellEnv src,
                List<AdministrationShell> filterForAas = null,
                List<Asset> filterForAsset = null,
                List<Submodel> filterForSubmodel = null,
                List<ConceptDescription> filterForCD = null)
            {
                // prepare defaults
                if (filterForAas == null)
                    filterForAas = new List<AdministrationShell>();
                if (filterForAsset == null)
                    filterForAsset = new List<Asset>();
                if (filterForSubmodel == null)
                    filterForSubmodel = new List<Submodel>();
                if (filterForCD == null)
                    filterForCD = new List<ConceptDescription>();

                // make new
                var res = new AdministrationShellEnv();

                // take over AAS
                foreach (var aas in src.administrationShells)
                    if (filterForAas.Contains(aas))
                    {
                        // take over
                        res.administrationShells.Add(new AdministrationShell(aas));

                        // consequences
                        if (aas.assetRef != null)
                        {
                            var asset = src.FindAsset(aas.assetRef);
                            if (asset != null)
                                filterForAsset.Add(asset);
                        }

                        if (aas.submodelRefs != null)
                            foreach (var smr in aas.submodelRefs)
                            {
                                var sm = src.FindSubmodel(smr);
                                if (sm != null)
                                    filterForSubmodel.Add(sm);
                            }

                        if (aas.conceptDictionaries != null)
                            foreach (var cdd in aas.conceptDictionaries)
                                if (cdd.conceptDescriptionsRefs != null && cdd.conceptDescriptionsRefs.conceptDescriptions != null)
                                    foreach (var cdr in cdd.conceptDescriptionsRefs.conceptDescriptions)
                                    {
                                        var cd = src.FindConceptDescription(cdr);
                                        if (cd != null)
                                            filterForCD.Add(cd);
                                    }
                    }

                // take over Assets
                foreach (var asset in src.assets)
                    if (filterForAsset.Contains(asset))
                    {
                        // take over
                        res.assets.Add(new Asset(asset));
                    }

                // take over Submodels
                foreach (var sm in src.Submodels)
                    if (filterForSubmodel.Contains(sm))
                    {
                        // take over
                        res.submodels.Add(new Submodel(sm));

                        // recursion in order to find used CDs ... :-(
                        CreateFromExistingEnvRecurseForCDs(src, sm.submodelElements, ref filterForCD);
                    }

                // ConceptDescriptions
                foreach (var cd in src.ConceptDescriptions)
                    if (filterForCD.Contains(cd))
                    {
                        // take over
                        res.conceptDescriptions.Add(new ConceptDescription(cd));
                    }

                // ok
                return res;
            }
        }

    }

    #endregion
}

#endif