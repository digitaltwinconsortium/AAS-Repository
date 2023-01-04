
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class AssetAdministrationShellDescriptor : Descriptor
    {
        [DataMember(Name="administration")]
        public AdministrativeInformation Administration { get; set; }

        [DataMember(Name="description")]
        public List<LangString> Description { get; set; }

        [DataMember(Name="GlobalAssetId")]
        public Reference GlobalAssetId { get; set; }

        [DataMember(Name="idShort")]
        public string IdShort { get; set; }

        [Required]
        [DataMember(Name="identification")]
        public string Identification { get; set; }

        [DataMember(Name="specificAssetIds")]
        public List<IdentifierKeyValuePair> SpecificAssetIds { get; set; }

        [DataMember(Name="submodelDescriptors")]
        public List<SubmodelDescriptor> SubmodelDescriptors { get; set; }
    }
}
