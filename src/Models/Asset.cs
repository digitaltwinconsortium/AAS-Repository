
namespace AdminShell
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Serialization;

    public class Asset : Identifiable
    {
        [XmlElement(ElementName = "hasDataSpecification")]
        public HasDataSpecification hasDataSpecification = null;

        [XmlElement(ElementName = "assetIdentificationModelRef")]
        public SubmodelRef assetIdentificationModelRef = null;

        [XmlElement(ElementName = "billOfMaterialRef")]
        public SubmodelRef billOfMaterialRef = null;

        [XmlElement(ElementName = "kind")]
        public AssetKind kind = new AssetKind();

        [XmlIgnore]
        [JsonProperty(PropertyName = "kind")]
        public string JsonKind
        {
            get
            {
                if (kind == null)
                    return null;
                return kind.kind;
            }
            set
            {
                if (kind == null)
                    kind = new AssetKind();
                kind.kind = value;
            }
        }

        public AssetRef GetAssetReference()
        {
            var r = new AssetRef();
            r.Keys.Add(
                Key.CreateNew(
                    this.GetElementName(), true, this.identification.idType, this.identification.id));
            return r;
        }

        public Reference GetReference()
        {
            return GetAssetReference();
        }

        public AasElementSelfDescription GetSelfDescription()
        {
            return new AasElementSelfDescription("Asset", "Asset");
        }

        public Tuple<string, string> ToCaptionInfo()
        {
            var caption = AdminShell.AdminShellUtil.EvalToNonNullString("\"{0}\" ", idShort, "<no idShort!>");
            if (administration != null)
                caption += "V" + administration.Version + "." + administration.Revision;

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

        public IEnumerable<Reference> FindAllReferences()
        {
            if (this.assetIdentificationModelRef != null)
                yield return this.assetIdentificationModelRef;

            if (this.billOfMaterialRef != null)
                yield return this.billOfMaterialRef;
        }
    }
}
