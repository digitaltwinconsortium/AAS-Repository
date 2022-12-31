/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;


//namespace AdminShell
namespace AdminShell_V30
{
    public partial class AdminShellV30
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
            [XmlIgnore]
            
             // do NOT count children!
            private SubmodelElementWrapperCollection _statements = null;

            
            public SubmodelElementWrapperCollection statements
            {
                get { return _statements; }
                set { _statements = value; _statements.Parent = this; }
            }

            [XmlIgnore]
            [JsonProperty(PropertyName = "statements")]
            public SubmodelElement[] JsonStatements
            {
                get
                {
                    var res = new ListOfSubmodelElement();
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

            
            public string entityType = "";

            [JsonProperty(PropertyName = "asset")]
            public ModelReference assetRef = null;

            // enumerates its children

            public IEnumerable<SubmodelElementWrapper> EnumerateChildren()
            {
                if (this.statements != null)
                    foreach (var smw in this.statements)
                        yield return smw;
            }

            public EnumerationPlacmentBase GetChildrenPlacement(SubmodelElement child)
            {
                return null;
            }

            public object AddChild(SubmodelElementWrapper smw, EnumerationPlacmentBase placement = null)
            {
                if (smw == null)
                    return null;
                if (this.statements == null)
                    this.statements = new SubmodelElementWrapperCollection();
                if (smw.submodelElement != null)
                    smw.submodelElement.parent = this;
                this.statements.Add(smw);
                return smw;
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
                    this.assetRef = new ModelReference(ent.assetRef);
            }

            public Entity(EntityTypeEnum entityType, string idShort = null, ModelReference assetRef = null,
                string category = null, Identifier semanticIdKey = null)
            {
                CreateNewLogic(idShort, null, semanticIdKey);

                this.entityType = EntityTypeNames[(int)entityType];
                this.assetRef = assetRef;
            }

#if !DoNotUseAasxCompatibilityModels
            // not available in V1.0

            public Entity(AasxCompatibilityModels.AdminShellV20.Entity src)
                : base(src)
            {
                if (src.statements != null)
                {
                    this.statements = new SubmodelElementWrapperCollection();
                    foreach (var smw in src.statements)
                        this.statements.Add(new SubmodelElementWrapper(smw.submodelElement));
                }
                this.entityType = src.entityType;
                if (src.assetRef != null)
                    this.assetRef = new ModelReference(src.assetRef);
            }
#endif

            public static Entity CreateNew(
                string idShort = null, string category = null, Identifier semanticIdKey = null)
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
                return new AasElementSelfDescription("Entity", "Ent",
                    SubmodelElementWrapper.AdequateElementEnum.Entity);
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
}
