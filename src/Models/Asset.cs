
namespace AdminShell
{
    using System.Xml;
    using System.Xml.Serialization;

    public class Asset : Identifiable
    {
        [XmlElement(ElementName = "hasDataSpecification")]
        public HasDataSpecification hasDataSpecification = null;

        [XmlElement(ElementName = "assetIdentificationModelRef")]
        public SubmodelReference assetIdentificationModelRef = null;

        [XmlElement(ElementName = "billOfMaterialRef")]
        public SubmodelReference billOfMaterialRef = null;

        [XmlElement(ElementName = "kind")]
        public AssetKind kind = new AssetKind();
    }
}
