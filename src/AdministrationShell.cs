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
        public class AdministrationShell : Identifiable
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

            [JsonProperty(PropertyName = "asset")]
            public AssetRef assetRef = new AssetRef();

            [JsonProperty(PropertyName = "submodels")]
            public List<SubmodelRef> submodelRefs = new List<SubmodelRef>();

            
            public Views views = null;
            [XmlIgnore]
            [JsonProperty(PropertyName = "views")]
            public View[] JsonViews
            {
                get { return views?.views.ToArray(); }
                set { views = Views.CreateOrSetInnerViews(views, value); }
            }

            [JsonProperty(PropertyName = "conceptDictionaries")]
            public List<ConceptDictionary> conceptDictionaries = null;

            // constructurs

            public AdministrationShell() { }

            public AdministrationShell(AdministrationShell src)
                : base(src)
            {
                if (src.hasDataSpecification != null)
                    this.hasDataSpecification = new HasDataSpecification(src.hasDataSpecification);

                if (src.derivedFrom != null)
                    this.derivedFrom = new AssetAdministrationShellRef(src.derivedFrom);

                if (src.assetRef != null)
                    this.assetRef = new AssetRef(src.assetRef);

                if (src.submodelRefs != null)
                    foreach (var smr in src.submodelRefs)
                        this.submodelRefs.Add(new SubmodelRef(smr));

                if (src.views != null)
                    this.views = new Views(src.views);

                if (src.conceptDictionaries != null)
                {
                    this.conceptDictionaries = new List<ConceptDictionary>();
                    foreach (var cdd in src.conceptDictionaries)
                        this.conceptDictionaries.Add(new ConceptDictionary(cdd));
                }
            }

            public static AdministrationShell CreateNew(string idType, string id, string version = null, string revision = null)
            {
                var s = new AdministrationShell();
                s.identification.idType = idType;
                s.identification.id = id;
                if (version != null)
                    s.SetAdminstration(version, revision);
                return (s);
            }

            // add

            public void AddView(View v)
            {
                if (views == null)
                    views = new Views();
                views.views.Add(v);
            }

            public void AddConceptDictionary(ConceptDictionary d)
            {
                if (conceptDictionaries == null)
                    conceptDictionaries = new List<ConceptDictionary>();
                conceptDictionaries.Add(d);
            }

            public void AddDataSpecification(Key k)
            {
                if (hasDataSpecification == null)
                    hasDataSpecification = new HasDataSpecification();
                var r = new Reference();
                r.Keys.Add(k);
                hasDataSpecification.reference.Add(r);
            }

            public override string GetElementName()
            {
                return "AssetAdministrationShell";
            }

            public Tuple<string, string> ToCaptionInfo()
            {
                var caption = AdminShellUtil.EvalToNonNullString("\"{0}\" ", idShort, "\"AAS\"");
                if (administration != null)
                    caption += "V" + administration.version + "." + administration.revision;

                var info = "";
                if (identification != null)
                    info = $"[{identification.idType}, {identification.id}]";
                return Tuple.Create(caption, info);
            }

            public override string ToString()
            {
                var ci = ToCaptionInfo();
                return string.Format("{0}{1}", ci.Item1, (ci.Item2 != "") ? " / " + ci.Item2 : "");
            }

            public SubmodelRef FindSubmodelRef(Identification refid)
            {
                if (this.submodelRefs == null)
                    return null;
                foreach (var r in this.submodelRefs)
                    if (r.MatchesTo(refid))
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
                    if (r.MatchesTo(newref))
                        found = true;

                return found;
            }

            public void AddSubmodelRef(SubmodelRef newref)
            {
                if (this.submodelRefs == null)
                    this.submodelRefs = new List<SubmodelRef>();
                this.submodelRefs.Add(newref);
            }
        }

    }

    #endregion
}

#endif