/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AasxCompatibilityModels
{
    #region AdminShell_V2_0

    public partial class AdminShellV20
    {
        public class Entity : SubmodelElement, IManageSubmodelElements, IEnumerateChildren
        {
            public enum EntityTypeEnum { CoManagedEntity = 0, SelfManagedEntity = 1, Undef = 3 }
            public static string[] EntityTypeNames = new string[] { "CoManagedEntity", "SelfManagedEntity" };

            // for JSON only

            [XmlIgnore]
            [JsonProperty(PropertyName = "modelType")]
            public new JsonModelTypeWrapper JsonModelType
            {
                get { return new JsonModelTypeWrapper(GetElementName()); }
            }

            // from this very class

            [JsonIgnore]
            [SkipForHash] // do NOT count children!
            public SubmodelElementWrapperCollection statements = new SubmodelElementWrapperCollection();

            [XmlIgnore]
            [JsonProperty(PropertyName = "statements")]
            public SubmodelElement[] JsonStatements
            {
                get
                {
                    var res = new List<SubmodelElement>();
                    if (statements != null)
                        foreach (var smew in statements)
                            res.Add(smew.submodelElement);
                    return res.ToArray();
                }
                set
                {
                    if (value != null)
                    {
                        this.statements = new SubmodelElementWrapperCollection();
                        foreach (var x in value)
                        {
                            var smew = new SubmodelElementWrapper() { submodelElement = x };
                            this.statements.Add(smew);
                        }
                    }
                }
            }

            // further members

            [CountForHash]
            public string entityType = "";

            [JsonProperty(PropertyName = "asset")]
            public AssetRef assetRef = null;

            // enumerates its children

            public IEnumerable<SubmodelElementWrapper> EnumerateChildren()
            {
                if (this.statements != null)
                    foreach (var smw in this.statements)
                        yield return smw;
            }

            public void AddChild(SubmodelElementWrapper smw)
            {
                if (smw == null)
                    return;
                if (this.statements == null)
                    this.statements = new SubmodelElementWrapperCollection();
                this.statements.Add(smw);
            }

            // constructors

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
                    this.assetRef = new AssetRef(ent.assetRef);
            }

            public Entity(EntityTypeEnum entityType, string idShort = null, AssetRef assetRef = null)
            {
                this.entityType = EntityTypeNames[(int)entityType];
                this.idShort = idShort;
                this.assetRef = assetRef;
            }

#if !DoNotUseAasxCompatibilityModels
            // not available in V1.0
#endif

            public static Entity CreateNew(string idShort = null, string category = null, Key semanticIdKey = null)
            {
                var x = new Entity();
                x.CreateNewLogic(idShort, category, semanticIdKey);
                return (x);
            }

            // from IManageSubmodelElements
            public void Add(SubmodelElement sme)
            {
                if (statements == null)
                    statements = new SubmodelElementWrapperCollection();
                var sew = new SubmodelElementWrapper();
                sme.parent = this; // track parent here!
                sew.submodelElement = sme;
                statements.Add(sew);
            }

            public void Remove(SubmodelElement sme)
            {
                if (statements != null)
                    statements.Remove(sme);
            }

            // management of elememts

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

            // entity type

            public EntityTypeEnum GetEntityType()
            {
                EntityTypeEnum res = EntityTypeEnum.Undef;
                if (this.entityType != null && this.entityType.Trim().ToLower() == EntityTypeNames[0].ToLower())
                    res = EntityTypeEnum.CoManagedEntity;
                if (this.entityType != null && this.entityType.Trim().ToLower() == EntityTypeNames[1].ToLower())
                    res = EntityTypeEnum.SelfManagedEntity;
                return res;
            }

            // name

            public override AasElementSelfDescription GetSelfDescription()
            {
                return new AasElementSelfDescription("Entity", "Ent");
            }

            public override object ToValueOnlySerialization()
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

    #endregion
}

