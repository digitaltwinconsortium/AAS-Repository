
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class Entity : SubmodelElement
    {
        [Required]
        [DataMember(Name = "entityType")]
        [XmlElement(ElementName = "entityType")]
        public string EntityType { get; set; }

        [DataMember(Name = "globalAssetId")]
        [XmlElement(ElementName = "globalAssetId")]
        public Reference GlobalAssetId { get; set; }

        [DataMember(Name = "specificAssetIds")]
        [XmlArray(ElementName = "specificAssetIds")]
        public List<IdentifierKeyValuePair> SpecificAssetIds { get; set; }

        [DataMember(Name = "statements")]
        [XmlArray(ElementName = "statements")]
        public List<SubmodelElement> Statements { get; set; }

        [DataMember(Name = "asset")]
        [XmlElement(ElementName = "asset")]
        public ModelReference AssetRef = null;

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

            if (ent.AssetRef != null)
                AssetRef = new ModelReference(ent.AssetRef);
        }

        public EntityTypeEnum GetEntityType()
        {
            EntityTypeEnum res = EntityTypeEnum.Undefined;

            if (EntityType != null && EntityType.Trim().ToLower() == EntityTypeEnum.CoManagedEntity.ToString().ToLower())
                res = EntityTypeEnum.CoManagedEntity;

            if (EntityType != null && EntityType.Trim().ToLower() == EntityTypeEnum.SelfManagedEntity.ToString().ToLower())
                res = EntityTypeEnum.SelfManagedEntity;

            return res;
        }
    }
}
