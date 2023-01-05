
namespace AdminShell
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class Asset : Identifiable
    {
        [DataMember(Name = "hasDataSpecification")]
        [XmlElement(ElementName = "hasDataSpecification")]
        public HasDataSpecification HasDataSpecification = null;

        [DataMember(Name = "assetIdentificationModelRef")]
        [XmlElement(ElementName = "assetIdentificationModelRef")]
        public SubmodelReference AssetIdentificationModelRef = null;

        [DataMember(Name = "billOfMaterialRef")]
        [XmlElement(ElementName = "billOfMaterialRef")]
        public SubmodelReference BillOfMaterialRef = null;

        [DataMember(Name = "kind")]
        public AssetKind kind = new AssetKind();
    }
}
