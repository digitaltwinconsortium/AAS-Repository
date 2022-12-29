/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using AdminShellNS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace AasxCompatibilityModels
{
    #region AdminShell_V2_0

    public partial class AdminShellV20
    {
        [XmlRoot(ElementName = "aasenv", Namespace = "http://www.admin-shell.io/aas/2/0")]
        public class AdministrationShellEnv : IFindAllReferences
        {
            [XmlAttribute(Namespace = System.Xml.Schema.XmlSchema.InstanceNamespace)]
            [JsonIgnore]
            public string schemaLocation =
                "http://www.admin-shell.io/aas/2/0 AAS.xsd http://www.admin-shell.io/IEC61360/2/0 IEC61360.xsd";

            [XmlIgnore] // will be ignored, anyway
            private List<AdministrationShell> administrationShells = new List<AdministrationShell>();
            [XmlIgnore] // will be ignored, anyway
            private List<Asset> assets = new List<Asset>();
            [XmlIgnore] // will be ignored, anyway
            private List<Submodel> submodels = new List<Submodel>();
            [XmlIgnore] // will be ignored, anyway
            private ListOfConceptDescriptions conceptDescriptions = new ListOfConceptDescriptions();

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
            public ListOfConceptDescriptions ConceptDescriptions
            {
                get { return conceptDescriptions; }
                set { conceptDescriptions = value; }
            }

            // constructors

            public AdministrationShellEnv() { }

#if !DoNotUseAasxCompatibilityModels
            public AdministrationShellEnv(AasxCompatibilityModels.AdminShellV10.AdministrationShellEnv src)
            {
                if (src.AdministrationShells != null)
                    foreach (var aas in src.AdministrationShells)
                        this.administrationShells.Add(new AdministrationShell(aas));

                if (src.Assets != null)
                    foreach (var asset in src.Assets)
                        this.assets.Add(new Asset(asset));

                if (src.Submodels != null)
                    foreach (var sm in src.Submodels)
                        this.submodels.Add(new Submodel(sm));

                if (src.ConceptDescriptions != null)
                    foreach (var cd in src.ConceptDescriptions)
                        this.conceptDescriptions.Add(new ConceptDescription(cd));
            }
#endif

            // to String

            public override string ToString()
            {
                var res = "AAS-ENV";
                if (AdministrationShells != null)
                    res += $" {AdministrationShells.Count} AAS";
                if (Assets != null)
                    res += $" {Assets.Count} Assets";
                if (Submodels != null)
                    res += $" {Submodels.Count} Submodels";
                if (ConceptDescriptions != null)
                    res += $" {ConceptDescriptions.Count} CDs";
                return res;
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
                            if (smref.Matches(smid))
                                return aas;
                return null;
            }

            public IEnumerable<AdministrationShell> FindAllAAS(
                Predicate<AdministrationShell> p = null)
            {
                if (this.administrationShells == null)
                    yield break;
                foreach (var x in this.administrationShells)
                    if (p == null || p(x))
                        yield return x;
            }

            public IEnumerable<Submodel> FindAllSubmodelGroupedByAAS(
                Func<AdministrationShell, Submodel, bool> p = null)
            {
                if (this.administrationShells == null || this.submodels == null)
                    yield break;
                foreach (var aas in this.administrationShells)
                {
                    if (aas?.submodelRefs == null)
                        continue;
                    foreach (var smref in aas.submodelRefs)
                    {
                        var sm = this.FindSubmodel(smref);
                        if (sm != null && (p == null || p(aas, sm)))
                            yield return sm;
                    }
                }
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

            public Submodel FindFirstSubmodelBySemanticId(Key semId)
            {
                // access
                if (semId == null)
                    return null;

                // brute force
                foreach (var sm in this.Submodels)
                    if (true == sm.semanticId?.MatchesExactlyOneKey(semId))
                        return sm;

                return null;
            }

            public IEnumerable<Submodel> FindAllSubmodelBySemanticId(
                Key semId, Key.MatchMode matchMode = Key.MatchMode.Strict)
            {
                // access
                if (semId == null)
                    yield break;

                // brute force
                foreach (var sm in this.Submodels)
                    if (true == sm.semanticId?.MatchesExactlyOneKey(semId, matchMode))
                        yield return sm;
            }

            public IEnumerable<Referable> FindAllReferable(Predicate<Referable> p)
            {
                if (p == null)
                    yield break;

                foreach (var r in this.FindAllReferable())
                    if (r != null && p(r))
                        yield return r;
            }

            public IEnumerable<Referable> FindAllReferable()
            {
                if (this.AdministrationShells != null)
                    foreach (var aas in this.AdministrationShells)
                        if (aas != null)
                        {
                            // AAS itself
                            yield return aas;

                            // Views
                            if (aas.views?.views != null)
                                foreach (var view in aas.views.views)
                                    yield return view;
                        }

                if (this.Assets != null)
                    foreach (var asset in this.Assets)
                        if (asset != null)
                            yield return asset;

                if (this.Submodels != null)
                    foreach (var sm in this.Submodels)
                        if (sm != null)
                        {
                            yield return sm;

                            // TODO (MIHO, 2020-08-26): not very elegant, yet. Avoid temporary collection
                            var allsme = new List<SubmodelElement>();
                            sm.RecurseOnSubmodelElements(null, (state, parents, sme) =>
                            {
                                allsme.Add(sme);
                            });
                            foreach (var sme in allsme)
                                yield return sme;
                        }

                if (this.ConceptDescriptions != null)
                    foreach (var cd in this.ConceptDescriptions)
                        if (cd != null)
                            yield return cd;
            }

            //
            // Reference handling
            //

            public Referable FindReferableByReference(Reference rf, int keyIndex = 0, bool exactMatch = false)
            {
                // first index needs to exist ..
                if (rf == null || keyIndex >= rf.Count)
                    return null;

                // which type?
                var firstType = rf[keyIndex].type.Trim().ToLower();
                var firstIdentification = new Identification(rf[keyIndex].idType, rf[keyIndex].value);
                AdministrationShell aasToFollow = null;

                if (firstType == Key.AAS.Trim().ToLower())
                {
                    // search aas
                    var aas = this.FindAAS(firstIdentification);

                    // not found or already at end with our search?
                    if (aas == null || keyIndex >= rf.Count - 1)
                        return aas;

                    // follow up
                    aasToFollow = aas;
                }

                if (firstType == Key.Asset.Trim().ToLower())
                {
                    // search asset
                    var asset = this.FindAsset(firstIdentification);

                    // not found or already at end with our search?
                    if (asset == null || keyIndex >= rf.Count - 1)
                        return exactMatch ? null : asset;

                    // try find aas for it
                    var aas = this.FindAllAAS((a) =>
                    {
                        return a?.assetRef?.Matches(asset.identification) == true;
                    }).FirstOrDefault();
                    if (aas == null)
                        return exactMatch ? null : asset;

                    // follow up
                    aasToFollow = aas;
                }

                // try
                if (aasToFollow != null)
                {
                    // search different entities
                    if (rf[keyIndex + 1].type.Trim().ToLower() == Key.Submodel.ToLower()
                        || rf[keyIndex + 1].type.Trim().ToLower() == Key.SubmodelRef.ToLower())
                    {
                        // ok, search SubmodelRef
                        var smref = aasToFollow.FindSubmodelRef(rf[keyIndex + 1].ToId());
                        if (smref == null)
                            return exactMatch ? null : aasToFollow;

                        // validate matching submodel
                        var sm = this.FindSubmodel(smref);
                        if (sm == null)
                            return exactMatch ? null : aasToFollow;

                        // at our end?
                        if (keyIndex >= rf.Count - 2)
                            return sm;

                        // go inside
                        return SubmodelElementWrapper.FindReferableByReference(sm.submodelElements, rf, keyIndex + 2);
                    }
                }

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

            //
            // Handling of CDs
            //

            public ConceptDescription FindConceptDescription(ConceptDescriptionRef cdr)
            {
                if (cdr == null)
                    return null;
                return FindConceptDescription(cdr.Keys);
            }

            public ConceptDescription FindConceptDescription(SemanticId semId)
            {
                if (semId == null)
                    return null;
                return FindConceptDescription(semId.Keys);
            }

            public ConceptDescription FindConceptDescription(Reference rf)
            {
                if (rf == null)
                    return null;
                return FindConceptDescription(rf.Keys);
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

            public IEnumerable<T> FindAllSubmodelElements<T>(
                Predicate<T> match = null, AdministrationShell onlyForAAS = null) where T : SubmodelElement
            {
                // more or less two different schemes
                if (onlyForAAS != null)
                {
                    if (onlyForAAS.submodelRefs == null)
                        yield break;
                    foreach (var smr in onlyForAAS.submodelRefs)
                    {
                        var sm = this.FindSubmodel(smr);
                        if (sm?.submodelElements != null)
                            foreach (var x in sm.submodelElements.FindDeep<T>(match))
                                yield return x;
                    }
                }
                else
                {
                    if (this.Submodels != null)
                        foreach (var sm in this.Submodels)
                            if (sm?.submodelElements != null)
                                foreach (var x in sm.submodelElements.FindDeep<T>(match))
                                    yield return x;
                }
            }

            public ConceptDescription FindConceptDescription(Key key)
            {
                if (key == null)
                    return null;
                var l = new List<Key> { key };
                return (FindConceptDescription(l));
            }

            public IEnumerable<Reference> FindAllReferences()
            {
                if (this.AdministrationShells != null)
                    foreach (var aas in this.AdministrationShells)
                        if (aas != null)
                            foreach (var r in aas.FindAllReferences())
                                yield return r;

                if (this.Assets != null)
                    foreach (var asset in this.Assets)
                        if (asset != null)
                            foreach (var r in asset.FindAllReferences())
                                yield return r;

                if (this.Submodels != null)
                    foreach (var sm in this.Submodels)
                        if (sm != null)
                            foreach (var r in sm.FindAllReferences())
                                yield return r;

                if (this.ConceptDescriptions != null)
                    foreach (var cd in this.ConceptDescriptions)
                        if (cd != null)
                            foreach (var r in cd.FindAllReferences())
                                yield return r;
            }

            // creators

            private void CopyConceptDescriptionsFrom(
                AdministrationShellEnv srcEnv, SubmodelElement src, bool shallowCopy = false)
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
                        CopyConceptDescriptionsFrom(srcEnv, m.submodelElement, shallowCopy: false);

            }

            public SubmodelElementWrapper CopySubmodelElementAndCD(
                AdministrationShellEnv srcEnv, SubmodelElement srcElem, bool copyCD = false, bool shallowCopy = false)
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

            public SubmodelRef CopySubmodelRefAndCD(
                AdministrationShellEnv srcEnv, SubmodelRef srcSubRef, bool copySubmodel = false, bool copyCD = false,
                bool shallowCopy = false)
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
                if (dstSub != null)
                {
                    // there is already an submodel, just add members
                    if (!shallowCopy && srcSub.submodelElements != null)
                    {
                        if (dstSub.submodelElements == null)
                            dstSub.submodelElements = new SubmodelElementWrapperCollection();
                        foreach (var smw in srcSub.submodelElements)
                            dstSub.submodelElements.Add(
                                new SubmodelElementWrapper(
                                    smw.submodelElement, shallowCopy: false));
                    }
                }

                // copy the CDs..
                if (copyCD && srcSub.submodelElements != null)
                    foreach (var smw in srcSub.submodelElements)
                        CopyConceptDescriptionsFrom(srcEnv, smw.submodelElement, shallowCopy);

                // give back
                return dstSubRef;
            }

            /// <summary>
            /// Tries renaming an Identifiable, specifically: the identification of an Identifiable and
            /// all references to it.
            /// Currently supported: ConceptDescriptions
            /// Returns, if a successful renaming was performed
            /// </summary>
            public bool RenameIdentifiable<T>(Identification oldId, Identification newId) where T : Identifiable
            {
                // access
                if (oldId == null || newId == null || oldId.IsEqual(newId))
                    return false;

                if (typeof(T) == typeof(ConceptDescription))
                {
                    // check, if exist or not exist
                    var cdOld = FindConceptDescription(oldId);
                    if (cdOld == null || FindConceptDescription(newId) != null)
                        return false;

                    // rename old cd
                    cdOld.identification = newId;

                    // search all SMEs referring to this CD
                    foreach (var sme in this.FindAllSubmodelElements<SubmodelElement>(match: (s) =>
                    {
                        return (s != null && s.semanticId != null && s.semanticId.Matches(oldId));
                    }))
                    {
                        sme.semanticId[0].idType = newId.idType;
                        sme.semanticId[0].value = newId.id;
                    }

                    // seems fine
                    return true;
                }

                if (typeof(T) == typeof(Submodel))
                {
                    // check, if exist or not exist
                    var smOld = FindSubmodel(oldId);
                    if (smOld == null || FindSubmodel(newId) != null)
                        return false;

                    // recurse all possible Referenes in the aas env
                    foreach (var r in this.FindAllReferences())
                        if (r != null)
                            for (int i = 0; i < r.Count; i++)
                                if (r[i].Matches(Key.Submodel, false, oldId.idType, oldId.id, Key.MatchMode.Relaxed))
                                {
                                    // directly replace
                                    r[i].idType = newId.idType;
                                    r[i].value = newId.id;
                                }

                    // rename old Submodel
                    smOld.identification = newId;

                    // seems fine
                    return true;
                }

                if (typeof(T) == typeof(Asset))
                {
                    // check, if exist or not exist
                    var assetOld = FindAsset(oldId);
                    if (assetOld == null || FindAsset(newId) != null)
                        return false;

                    // recurse all possible Referenes in the aas env
                    foreach (var r in this.FindAllReferences())
                        if (r != null)
                            for (int i = 0; i < r.Count; i++)
                                if (r[i].Matches(Key.Asset, false, oldId.idType, oldId.id, Key.MatchMode.Relaxed))
                                {
                                    // directly replace
                                    r[i].idType = newId.idType;
                                    r[i].value = newId.id;
                                }

                    // rename old Submodel
                    assetOld.identification = newId;

                    // seems fine
                    return true;
                }

                // no result is false, as well
                return false;
            }

            // serializations

            public void SerializeXmlToStream(StreamWriter s)
            {
                var serializer = new XmlSerializer(typeof(AdministrationShellEnv));
                var nss = new XmlSerializerNamespaces();
                nss.Add("xsi", System.Xml.Schema.XmlSchema.InstanceNamespace);
                nss.Add("aas", "http://www.admin-shell.io/aas/2/0");
                nss.Add("IEC61360", "http://www.admin-shell.io/IEC61360/2/0");
                serializer.Serialize(s, this, nss);
            }

            public JsonWriter SerialiazeJsonToStream(StreamWriter sw, bool leaveJsonWriterOpen = false)
            {
                sw.AutoFlush = true;

                JsonSerializer serializer = new JsonSerializer()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                    Formatting = Newtonsoft.Json.Formatting.Indented
                };

                JsonWriter writer = new JsonTextWriter(sw);
                serializer.Serialize(writer, this);
                if (leaveJsonWriterOpen)
                    return writer;
                writer.Close();
                return null;
            }

            public AdministrationShellEnv DeserializeFromXmlStream(TextReader reader)
            {
                XmlSerializer serializer = new XmlSerializer(
                    typeof(AdministrationShellEnv), "http://www.admin-shell.io/aas/2/0");
                var res = serializer.Deserialize(reader) as AdministrationShellEnv;
                return res;
            }

            public AdministrationShellEnv DeserializeFromJsonStream(TextReader reader)
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Converters.Add(new AdminShellConverters.JsonAasxConverter("modelType", "name"));
                var res = (AdministrationShellEnv)serializer.Deserialize(reader, typeof(AdministrationShellEnv));
                return res;
            }

            // special functions

            private static void CreateFromExistingEnvRecurseForCDs(
                AdministrationShellEnv src, List<SubmodelElementWrapper> wrappers,
                ref List<ConceptDescription> filterForCD)
            {
                if (wrappers == null || filterForCD == null)
                    return;

                foreach (var w in wrappers)
                {
                    // access
                    if (w == null)
                        continue;

                    // include in filter ..
                    if (w.submodelElement.semanticId != null)
                    {
                        var cd = src.FindConceptDescription(w.submodelElement.semanticId.Keys);
                        if (cd != null)
                            filterForCD.Add(cd);
                    }

                    // recurse?
                    if (w.submodelElement is SubmodelElementCollection smec)
                        CreateFromExistingEnvRecurseForCDs(src, smec.value, ref filterForCD);

                    if (w.submodelElement is Operation op)
                        for (int i = 0; i < 2; i++)
                        {
                            var w2s = Operation.GetWrappers(op[i]);
                            CreateFromExistingEnvRecurseForCDs(src, w2s, ref filterForCD);
                        }

                    if (w.submodelElement is Entity smee)
                        CreateFromExistingEnvRecurseForCDs(src, smee.statements, ref filterForCD);

                    if (w.submodelElement is AnnotatedRelationshipElement smea)
                        CreateFromExistingEnvRecurseForCDs(src, smea.annotations, ref filterForCD);
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
                                if (cdd.conceptDescriptionsRefs != null &&
                                    cdd.conceptDescriptionsRefs.conceptDescriptions != null)
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

                        // recursion in order to find used CDs
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

            // Sorting

            public Referable.ComparerIndexed CreateIndexedComparerCdsForSmUsage()
            {
                var cmp = new Referable.ComparerIndexed();
                int nr = 0;
                foreach (var sm in FindAllSubmodelGroupedByAAS())
                    foreach (var sme in sm.FindDeep<SubmodelElement>())
                    {
                        if (sme.semanticId == null)
                            continue;
                        var cd = this.FindConceptDescription(sme.semanticId);
                        if (cd == null)
                            continue;
                        if (cmp.Index.ContainsKey(cd))
                            continue;
                        cmp.Index[cd] = nr++;
                    }
                return cmp;
            }
        }



    }

    #endregion
}

