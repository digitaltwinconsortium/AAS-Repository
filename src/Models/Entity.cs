
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class Entity : SubmodelElement
    {
        [Required]
        [DataMember(Name = "entityType")]
        public EntityTypeEnum EntityType { get; set; }

        [DataMember(Name = "GlobalAssetId")]
        public Reference GlobalAssetId { get; set; }

        [DataMember(Name = "specificAssetIds")]
        public List<IdentifierKeyValuePair> SpecificAssetIds { get; set; }

        [DataMember(Name = "statements")]
        public List<SubmodelElement> Statements { get; set; }

        [JsonProperty(PropertyName = "asset")]
        public ModelReference assetRef = null;

        public Entity() { }

        public Entity(SubmodelElement src)
            : base(src)
        {
            if (!(src is Entity ent))
                return;

            if (ent.Statements != null)
            {
                Statements = new List<SubmodelElement>();
                foreach (var smw in ent.Statements)
                    Statements.Add(smw);
            }
            EntityType = ent.EntityType;
            if (ent.assetRef != null)
                assetRef = new ModelReference(ent.assetRef);
        }
    }
}
