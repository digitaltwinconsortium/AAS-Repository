
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
        public EntityType EntityType { get; set; }

        [DataMember(Name = "globalAssetId")]
        public Reference GlobalAssetId { get; set; }

        [DataMember(Name = "specificAssetIds")]
        public List<IdentifierKeyValuePair> SpecificAssetIds { get; set; }

        [DataMember(Name = "statements")]
        public List<SubmodelElement> Statements { get; set; }

        [JsonProperty(PropertyName = "asset")]
        public ModelReference assetRef = null;

        // enumerates its children

        public IEnumerable<SubmodelElementWrapper> EnumerateChildren()
        {
            if (this.statements != null)
                foreach (var smw in this.statements)
                    yield return smw;
        }

        public Entity() { }

        public Entity(SubmodelElement src)
            : base(src)
        {
            if (!(src is Entity ent))
                return;

            if (ent.statements != null)
            {
                this.statements = new SubmodelElementWrapperCollection();
                foreach (var smw in ent.statements)
                    this.statements.Add(new SubmodelElementWrapper(smw.submodelElement));
            }
            this.entityType = ent.entityType;
            if (ent.assetRef != null)
                this.assetRef = new ModelReference(ent.assetRef);
        }

        public Entity(EntityTypeEnum entityType, string idShort = null, ModelReference assetRef = null,
            string category = null, Identifier semanticIdKey = null)
        {
            CreateNewLogic(idShort, null, semanticIdKey);

            this.entityType = EntityTypeNames[(int)entityType];
            this.assetRef = assetRef;
        }

        public static Entity CreateNew(
            string idShort = null, string category = null, Identifier semanticIdKey = null)
        {
            var x = new Entity();
            x.CreateNewLogic(idShort, category, semanticIdKey);
            return (x);
        }

        public void Add(SubmodelElement sme)
        {
            if (statements == null)
                statements = new SubmodelElementWrapperCollection();
            var sew = new SubmodelElementWrapper();
            sme.parent = this; // track parent here!
            sew.submodelElement = sme;
            statements.Add(sew);
        }

        public void Insert(int index, SubmodelElement sme)
        {
            if (statements == null)
                statements = new SubmodelElementWrapperCollection();
            var sew = new SubmodelElementWrapper();
            sme.parent = this; // track parent here!
            sew.submodelElement = sme;
            if (index < 0 || index >= statements.Count)
                return;
            statements.Insert(index, sew);
        }

        public void Remove(SubmodelElement sme)
        {
            if (statements != null)
                statements.Remove(sme);
        }

        public SubmodelElementWrapper FindSubmodelElementWrapper(string idShort)
        {
            if (this.statements == null)
                return null;
            foreach (var smw in this.statements)
                if (smw.submodelElement != null)
                    if (smw.submodelElement.idShort.Trim().ToLower() == idShort.Trim().ToLower())
                        return smw;
            return null;
        }

        public EntityTypeEnum GetEntityType()
        {
            EntityTypeEnum res = EntityTypeEnum.Undef;
            if (this.entityType != null && this.entityType.Trim().ToLower() == EntityTypeNames[0].ToLower())
                res = EntityTypeEnum.CoManagedEntity;
            if (this.entityType != null && this.entityType.Trim().ToLower() == EntityTypeNames[1].ToLower())
                res = EntityTypeEnum.SelfManagedEntity;
            return res;
        }

        public AasElementSelfDescription GetSelfDescription()
        {
            return new AasElementSelfDescription("Entity", "Ent",
                SubmodelElementWrapper.AdequateElementEnum.Entity);
        }

        public object ToValueOnlySerialization()
        {
            var output = new Dictionary<string, object>();
            List<object> valueOnlyStatements = new List<object>();

            foreach (var sme in statements)
            {
                valueOnlyStatements.Add(sme.submodelElement.ToValueOnlySerialization());
            }

            var valueDict = new Dictionary<string, object>
            {
                { "statements", valueOnlyStatements },
                { "entityType",  entityType},
                { "assetId", assetRef.First.value },
            };

            output.Add(idShort, valueDict);
            return output;
        }
    }
}
