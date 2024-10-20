
namespace AdminShell
{
    using Opc.Ua;
    using Opc.Ua.Export;
    using Opc.Ua.Server;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class I4AASNodeManager : CustomNodeManager2
    {
        public class NodeRecord
        {
            public NodeState _uanode = null;
            public Referable _referable = null;
            public string _identification = null;

            public NodeRecord() { }

            public NodeRecord(NodeState uanode, Referable referable)
            {
                _uanode = uanode;
                _referable = referable;
            }

            public NodeRecord(NodeState uanode, string identification)
            {
                _uanode = uanode;
                _identification = identification;
            }
        }
        
        public class AasReference : IReference
        {
            private NodeId _referenceTypeId = null;

            private bool _isInverse = false;
            
            private ExpandedNodeId _targetId = null;

            public NodeId ReferenceTypeId { get { return _referenceTypeId; } }
            
            public bool IsInverse { get { return _isInverse; } }
            
            public ExpandedNodeId TargetId { get { return _targetId; } }

            public AasReference()
            {
            }

            public AasReference(NodeId referenceTypeId, bool isInverse, ExpandedNodeId targetId)
            {
                _referenceTypeId = referenceTypeId;
                _isInverse = isInverse;
                _targetId = targetId;
            }
        }

        const int c_baseInterfaceTypeNodeId = 17602;
        const int c_hasDictionaryEntryNodeId = 17597;
        const int c_hasInterfaceNodeId = 17603;
        const int c_hasAddInNodeId = 17604;
        const int c_dictionaryEntryTypeNodeId = 17589;
        const int c_uriDictionaryEntryTypeNodeId = 17600;
        const int c_irdiDictionaryEntryTypeNodeId = 17598;
        const int c_dictionaryFolderTypeNodeId = 17591;

        const int c_referableNodeId = 1004;
        const int c_identificationNodeId = 1000;
        const int c_administrationNodeId = 1001;

        const int c_referableTypeNodeId = 2001;
        const int c_identifiableTypeNodeId = 2000;

        const int c_referenceNodeId = 1005;
        const int c_semanticIdNodeId = 1006;
        const int c_hasAasReferenceNodeId = 4000;

        const int c_dataSpecificationNodeId = 3000;
        const int c_dataSpecificationIEC61360NodeId = 3001;

        const int c_qualifierNodeId = 1002;
        const int c_assetKindNodeId = 1025;
        const int c_modelingKindNodeId = 1003;
        const int c_submodelElementNodeId = 1008;
        const int c_submodelWrapperNodeId = 1012;
        const int c_submodelNodeId = 1007;
        const int c_propertyNodeId = 1009;
        const int c_collectionNodeId = 1010;
        const int c_fileNodeId = 1013;
        const int c_blobNodeId = 1015;
        const int c_referenceElementNodeId = 1016;
        const int c_relationshipElementNodeId = 1017;
        const int c_operationVariableNodeId = 1018;
        const int c_operationNodeId = 1019;
        const int c_conceptDescriptionNodeId = 1021;
        const int c_assetNodeId = 1023;
        const int c_aasNodeId = 1024;

        public enum ModellingRule { None, Optional, OptionalPlaceholder, Mandatory, MandatoryPlaceholder }

        private Dictionary<NodeState, List<object>> _nodeStateAnnotations = new Dictionary<NodeState, List<object>>();

        private AASXPackageService _packageService = null;

        private ushort _namespaceIndex;
        private long _lastUsedId;

        const string c_exportFilename = "I4AAS_Export.nodeset2.xml";

        public IDictionary<NodeId, IList<IReference>> _nodeMgrExternalReferences = null;

        public NodeState _rootAAS = null;
        public NodeState _rootConceptDescriptions = null;
        public NodeState _rootDataSpecifications = null;
        public NodeState _rootMissingDictionaryEntries = null;

        private Dictionary<Referable, NodeRecord> _nodeRecordFromReferable = new();
        private Dictionary<string, NodeRecord> _nodeRecordFromIdentificationHash = new();

        public I4AASNodeManager(IServerInternal server, ApplicationConfiguration configuration)
        : base(server, configuration)
        {
            _packageService = (AASXPackageService)Program.AppHost.Services.GetService(typeof(AASXPackageService));

            List<string> namespaceUris =
            [
                "http://opcfoundation.org/UA/i4aas/",
                "http://digitaltwinconsortium.org/UA/i4aas/",
            ];

            NamespaceUris = namespaceUris;

            _namespaceIndex = Server.NamespaceUris.GetIndexOrAppend(namespaceUris[1]);

            _lastUsedId = 0;
        }

        public override NodeId New(ISystemContext context, NodeState node)
        {
            // for new nodes we create, pick our default namespace
            return new NodeId(Utils.IncrementIdentifier(ref _lastUsedId), (ushort)Server.NamespaceUris.GetIndex("http://digitaltwinconsortium.org/UA/i4aas/"));
        }

        public void SaveNodestateCollectionAsNodeSet2(ISystemContext context, NodeStateCollection nsc, Stream stream, bool filterSingleNodeIds)
        {
            Opc.Ua.Export.UANodeSet nodeSet = new()
            {
                LastModified = DateTime.UtcNow,
                LastModifiedSpecified = true
            };

            foreach (var n in nsc)
            {
                nodeSet.Export(context, n);
            }

            nodeSet.Write(stream);
        }

        public override void CreateAddressSpace(IDictionary<NodeId, IList<IReference>> externalReferences)
        {
            lock (Lock)
            {
                IList<IReference> objectsFolderReferences = null;
                if (!externalReferences.TryGetValue(ObjectIds.ObjectsFolder, out objectsFolderReferences))
                {
                    externalReferences[ObjectIds.ObjectsFolder] = objectsFolderReferences = new List<IReference>();
                }

                AddNodesFromNodesetXml("./NodeSets/I4AAS.NodeSet2.xml");

                _rootAAS = CreateFolder(null, "AASROOT");
                objectsFolderReferences.Add(new NodeStateReference(ReferenceTypes.Organizes, false, _rootAAS.NodeId));

                _rootConceptDescriptions = CreateObject(
                    _rootAAS,
                    "Concept Descriptions",
                    ReferenceTypeIds.HasComponent,
                    c_dictionaryFolderTypeNodeId);

                _rootMissingDictionaryEntries = CreateObject(
                    _rootAAS,
                    "Dictionary Entries",
                    ReferenceTypeIds.HasComponent,
                    c_dictionaryFolderTypeNodeId);

                foreach (AssetAdministrationShellEnvironment env in _packageService.Packages.Values)
                {
                    CreateInstanceObjects(env);
                }

                try
                {
                    NodeStateCollection nodesToExport = new();
                    foreach (NodeState node in PredefinedNodes.Values)
                    {
                        // only export nodes belonging to the I4AAS namespace
                        if (node.NodeId.NamespaceIndex != _namespaceIndex)
                        {
                            continue;
                        }

                        nodesToExport.Add(node);
                    }

                    // export nodeset XML
                    Utils.Trace("Writing export file: " + c_exportFilename);
                    using (var stream = new StreamWriter(c_exportFilename))
                    {
                        SaveNodestateCollectionAsNodeSet2(SystemContext, nodesToExport, stream.BaseStream, false);
                    }   
                }
                catch (Exception ex)
                {
                    Utils.Trace(ex, "When exporting to {0}", c_exportFilename);
                }

                AddReverseReferences(externalReferences);
                base.CreateAddressSpace(externalReferences);
            }
        }

        private void AddNodesFromNodesetXml(string nodesetFile)
        {
            using (Stream stream = new FileStream(nodesetFile, FileMode.Open))
            {
                UANodeSet nodeSet = UANodeSet.Read(stream);

                NodeStateCollection predefinedNodes = new NodeStateCollection();

                nodeSet.Import(SystemContext, predefinedNodes);

                for (int i = 0; i < predefinedNodes.Count; i++)
                {
                    try
                    {
                        AddPredefinedNode(SystemContext, predefinedNodes[i]);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message, ex);
                    }
                }
            }
        }

        public void AddNodeRecord(NodeRecord nr)
        {
            if (nr._referable != null && !_nodeRecordFromReferable.ContainsKey(nr._referable))
            {
                _nodeRecordFromReferable.Add(nr._referable, nr);
            }

            if (nr._identification != null && nr._identification != "")
            {
                var hash = nr._identification.Trim().ToUpper();
                if (!_nodeRecordFromIdentificationHash.ContainsKey(hash))
                {
                    _nodeRecordFromIdentificationHash.Add(hash, nr);
                }
            }
        }

        public NodeRecord LookupNodeRecordFromReferable(Referable referable)
        {
            if (_nodeRecordFromReferable == null || !_nodeRecordFromReferable.ContainsKey(referable))
            {
                return null;
            }

            return _nodeRecordFromReferable[referable];
        }

        public NodeRecord LookupNodeRecordFromIdentification(string identification)
        {
            var hash = identification.Trim().ToUpper();
            if (_nodeRecordFromReferable == null || !_nodeRecordFromIdentificationHash.ContainsKey(hash))
            {
                return null;
            }

            return _nodeRecordFromIdentificationHash[hash];
        }

        public static Referable FindReferableByReference(AssetAdministrationShellEnvironment environment, Reference reference, int keyIndex = 0)
        {
            if (environment == null || reference == null)
            {
                return null;
            }

            var keyList = reference?.Keys;
            if (keyList == null || keyList.Count == 0 || keyIndex >= keyList.Count)
            {
                return null;
            }

            var firstKeyType = keyList[keyIndex].Type;
            var firstKeyId = keyList[keyIndex].Value;

            switch (firstKeyType)
            {
                case KeyElements.AssetAdministrationShell:
                {
                    var aas = environment.AssetAdministrationShells.Where(
                        shell => shell.IdShort.Equals(firstKeyId,
                        StringComparison.OrdinalIgnoreCase)).First();

                    if (aas == null || keyIndex >= keyList.Count - 1)
                    {
                        return aas;
                    }

                    return FindReferableByReference(environment, reference, ++keyIndex);
                }

                case KeyElements.GlobalReference:
                {
                    var keyedAas = environment.AssetAdministrationShells.Where(
                        globalRef => globalRef.AssetInformation.GlobalAssetId.Keys[0].Value.Equals(firstKeyId,
                        StringComparison.OrdinalIgnoreCase)).First();

                    if (keyedAas != null)
                    {
                        return keyedAas;
                    }

                    return null;
                }

                case KeyElements.ConceptDescription:
                {
                    var keyedAas = environment.ConceptDescriptions.Where(
                        description => description.IdShort.Equals(firstKeyId,
                        StringComparison.OrdinalIgnoreCase)).First();

                    if (keyedAas != null)
                    {
                        return keyedAas;
                    }

                    return null;
                }

                case KeyElements.Submodel:
                {
                    var submodel = environment.Submodels.Where(
                        description => description.IdShort.Equals(firstKeyId,
                        StringComparison.OrdinalIgnoreCase)).First();
                    
                    if (submodel == null)
                    {
                        return null;
                    }

                    if (keyIndex >= keyList.Count - 1)
                    {
                        return submodel;
                    }

                    return FindReferableByReference(environment, reference, ++keyIndex);
                }
            }

            return null;
        }

        public void CreateInstanceObjects(AssetAdministrationShellEnvironment env)
        {
            if (_rootAAS == null)
            {
                return;
            }

            if (env.ConceptDescriptions != null && _rootConceptDescriptions != null)
            {
                foreach (ConceptDescription cd in env.ConceptDescriptions)
                {
                    CreateVariable<string>(_rootConceptDescriptions, cd.Identification.Id, c_conceptDescriptionNodeId, (cd.Description.Count > 0)? cd.Description[0]?.Text : string.Empty);
                }
            }

            if (env.AssetAdministrationShells != null)
            {
                foreach (var aas in env.AssetAdministrationShells)
                {
                    CreateObject(_rootAAS, env, aas);
                }
            }
        }

        public NodeState CreateObject(NodeState parent, AssetAdministrationShellEnvironment env, AssetAdministrationShell aas)
        {
            if (env == null || aas == null)
            {
                return null;
            }

            string extraName = null;
            string browseName = "AssetAdministrationShell";
            if (aas.IdShort != null && aas.IdShort.Trim().Length > 0)
            {
                extraName = "AssetAdministrationShell:" + aas.IdShort;
                browseName = aas.IdShort;
            }

            var o = CreateObject(parent, browseName, ReferenceTypeIds.HasComponent, c_administrationNodeId, extraName);

            AddNodeRecord(new NodeRecord(o, aas));

            CreateVariable<string>(o, "Referable", c_referableTypeNodeId, aas.Identification.Value);
            CreateVariable<string>(o, "Identification", c_identifiableTypeNodeId, aas.Id);
            CreateVariable<string>(o, "Administration", c_administrationNodeId, aas.Administration?.ToString());

            if (aas.EmbeddedDataSpecifications != null && aas.EmbeddedDataSpecifications != null)
            {
                foreach (var ds in aas.EmbeddedDataSpecifications)
                {
                    CreateVariable<string>(o, "DataSpecification", c_dataSpecificationNodeId, ds.DataSpecification.ToString());
                }
            }

            CreateVariable<string>(o, "DerivedFrom", c_referenceNodeId, aas.DerivedFrom.ToString());

            if (aas.AssetInformation != null)
            {
                CreateVariable<string>(o, "Asset", c_assetNodeId, aas.AssetInformation.ToString());
            }

            if (aas.Submodels != null && aas.Submodels.Count > 0)
            {
                foreach (var smr in aas.Submodels)
                {
                    CreateVariable<string>(o, "Submodel", c_submodelNodeId, smr.ToString());
                }
            }

            return o;
        }

        public ReferenceTypeState CreateAddReferenceType(string browseDisplayName, string inverseName, uint preferredNumId = 0, bool useZeroNS = false, NodeId sourceId = null)
        {
            ReferenceTypeState x = new()
            {
                BrowseName = browseDisplayName,
                DisplayName = browseDisplayName,
                InverseName = inverseName,
                Symmetric = false,
                IsAbstract = false,
                NodeId = new NodeId(browseDisplayName, _namespaceIndex)
            };
            
            AddPredefinedNode(SystemContext, x);

            if (sourceId == null)
            {
                sourceId = new NodeId(32, 0);
            }

            AddExternalReferencePublic(sourceId, ReferenceTypeIds.HasSubtype, false, x.NodeId, _nodeMgrExternalReferences);

            return x;
        }

        public void AddExternalReferencePublic(
            NodeId sourceId,
            NodeId referenceTypeId,
            bool isInverse,
            NodeId targetId,
            IDictionary<NodeId, IList<IReference>> externalReferences)
        {
            if (externalReferences != null)
            {
                AddExternalReference(sourceId, referenceTypeId, isInverse, targetId, externalReferences);
            }
        }

        public FolderState CreateFolder(NodeState parent, string browseDisplayName)
        {
            FolderState x = new(parent)
            {
                BrowseName = browseDisplayName,
                DisplayName = browseDisplayName,
                NodeId = new NodeId(browseDisplayName, _namespaceIndex),
                TypeDefinitionId = ObjectTypeIds.FolderType
            };

            AddPredefinedNode(SystemContext, x);

            if (parent != null)
            {
                parent.AddChild(x);
            }

            return x;
        }

        public DataTypeState CreateDataType(string browseDisplayName, NodeId superTypeId, uint preferredNumId = 0)
        {
            DataTypeState x = new()
            {
                BrowseName = browseDisplayName,
                DisplayName = browseDisplayName,
                Description = new Opc.Ua.LocalizedText("en", browseDisplayName),
                SuperTypeId = superTypeId,
                NodeId = new NodeId(browseDisplayName, _namespaceIndex)
            };

            AddPredefinedNode(SystemContext, x);

            return x;
        }

        public BaseObjectState CreateObject(
            NodeState parent,
            string browseDisplayName,
            NodeId referenceTypeFromParentId = null,
            NodeId typeDefinitionId = null,
            string extraName = null)
        {
            BaseObjectState x = new(parent)
            {
                BrowseName = browseDisplayName,
                DisplayName = browseDisplayName,
                Description = new Opc.Ua.LocalizedText("en", browseDisplayName)
            };

            if (extraName != null)
            {
                x.DisplayName = extraName;
            }

            if (typeDefinitionId != null)
            {
                x.TypeDefinitionId = typeDefinitionId;
            }

            x.NodeId = new NodeId(browseDisplayName, _namespaceIndex);

            AddPredefinedNode(SystemContext, x);

            if (parent != null)
            {
                parent.AddChild(x);
            }

            if (referenceTypeFromParentId != null)
            {
                if (parent != null)
                {
                    if (!parent.ReferenceExists(referenceTypeFromParentId, false, x.NodeId))
                    {
                        parent.AddReference(referenceTypeFromParentId, false, x.NodeId);
                    }

                    if (referenceTypeFromParentId == ReferenceTypeIds.HasComponent)
                    {
                        x.AddReference(referenceTypeFromParentId, true, parent.NodeId);
                    }

                    if (referenceTypeFromParentId == ReferenceTypeIds.HasProperty)
                    {
                        x.AddReference(referenceTypeFromParentId, true, parent.NodeId);
                    }
                }
            }

            return x;
        }

        public BaseDataVariableState<T> CreateVariable<T>(
            NodeState parent,
            string browseDisplayName,
            NodeId dataTypeId,
            T value,
            NodeId referenceTypeFromParentId = null,
            NodeId typeDefinitionId = null,
            int valueRank = -2,
            bool defaultSettings = false)
        {
            if (defaultSettings)
            {
                referenceTypeFromParentId = ReferenceTypeIds.HasProperty;
                typeDefinitionId = VariableTypeIds.PropertyType;
                if (valueRank == -2)
                {
                    valueRank = -1;
                }
            }

            BaseDataVariableState<T> x = new(parent)
            {
                BrowseName = browseDisplayName,
                DisplayName = browseDisplayName,
                Description = new Opc.Ua.LocalizedText("en", browseDisplayName),
                DataType = dataTypeId
            };

            if (valueRank > -2)
            {
                x.ValueRank = valueRank;
            }

            x.Value = (T)value;
            x.NodeId = new NodeId(browseDisplayName, _namespaceIndex);

            AddPredefinedNode(SystemContext, x);

            if (parent != null)
            {
                parent.AddChild(x);
            }

            if (referenceTypeFromParentId != null)
            {
                if (parent != null)
                {
                    if (!parent.ReferenceExists(referenceTypeFromParentId, false, x.NodeId))
                    {
                        parent.AddReference(referenceTypeFromParentId, false, x.NodeId);
                    }

                    if (referenceTypeFromParentId == ReferenceTypeIds.HasComponent)
                    {
                        x.AddReference(referenceTypeFromParentId, true, parent.NodeId);
                    }

                    if (referenceTypeFromParentId == ReferenceTypeIds.HasProperty)
                    {
                        x.AddReference(referenceTypeFromParentId, true, parent.NodeId);
                    }
                }
            }

            if (typeDefinitionId != null)
            {
                x.TypeDefinitionId = typeDefinitionId;
            }

            x.AccessLevel = AccessLevels.CurrentReadOrWrite;
            x.UserAccessLevel = AccessLevels.CurrentReadOrWrite;

            return x;
        }

        public DataTypeState CreateUaNodeForPathType(uint preferredTypeNumId = 0)
        {
            return CreateDataType("AASPathType", DataTypeIds.String);
        }

        public void CreateUaNodeForIdentification(uint preferredTypeNumId = 0)
        {
            NodeState node = CreateObject(null, "AASIdentifierType", ObjectTypeIds.BaseObjectType, preferredTypeNumId, "AAS:Identifier");

            CreateVariable<string>(node, "IdType", DataTypeIds.String, null);

            CreateVariable<string>(node, "Id", DataTypeIds.String, null);
        }

      
        public void CreateUaNodeForAdministration(uint preferredTypeNumId = 0)
        {
            NodeState node = CreateObject(null, "AASAdministrativeInformationType", ObjectTypeIds.BaseObjectType, preferredTypeNumId, "AAS:AdministrativeInformation");

            CreateVariable<string>(node, "Version", DataTypeIds.String, null);

            CreateVariable<string>(node, "Revision", DataTypeIds.String, null);
        }

        public NodeState CreateElements(NodeState parent, AdministrativeInformation administration = null)
        {
            if (parent == null)
            {
                return null;
            }

            if (administration == null)
            {
                return null;
            }

            var o = CreateObject(parent, "Administration", ReferenceTypeIds.HasComponent, c_administrationNodeId);

            CreateVariable<string>(o, "Version", DataTypeIds.String, administration.Version);

            CreateVariable<string>(o, "Revision", DataTypeIds.String, administration.Revision);

            return o;
        }
      
        public void CreateUaNodeForQualifier(uint preferredTypeNumId = 0)
        {
            NodeState node = CreateObject(null, "AASQualifierType", ObjectTypeIds.BaseObjectType, preferredTypeNumId, "AAS:Qualifier");

            CreateObject(node, null, "SemanticId");

            CreateVariable<string>(node, "Type", DataTypeIds.String, null);

            CreateVariable<string>(node, "Value", DataTypeIds.String, null);

            CreateElements(node, new Qualifier(){ Value = "ValueId" } );
        }

        public NodeState CreateElements(NodeState parent, Qualifier qualifier = null)
        {
            if (parent == null)
            {
                return null;
            }

            if (qualifier == null)
            {
                return null;
            }

            string extraName = null;

            if (qualifier.Type != null && qualifier.Type.Length > 0)
            {
                extraName = "Qualifier:" + qualifier.Type;

                if (qualifier.Value != null && qualifier.Value.Length > 0)
                {
                    extraName += "=" + qualifier.Value;
                }
            }

            NodeState node = CreateObject(parent, "Qualifier", ReferenceTypeIds.HasComponent, c_qualifierNodeId, extraName);

            CreateObject(node, qualifier.SemanticId, "SemanticId");

            CreateVariable<string>(node, "Type", DataTypeIds.String, qualifier.Type);

            CreateVariable<string>(node, "Value", DataTypeIds.String, qualifier.Value);

            CreateElements(node, qualifier);

            return node;
        }

        public NodeState CreateElements(NodeState parent, AssetKind kind = AssetKind.Type)
        {
            return CreateVariable<string>(parent, "Kind", DataTypeIds.String, string.Empty);
        }

        public NodeState CreateElements(NodeState parent)
        {
            return CreateVariable<string>(parent, "Kind", DataTypeIds.String, string.Empty);
        }

        public NodeState CreateElements(NodeState parent, Referable refdata = null)
        {
            if (parent == null)
            {
                return null;
            }

            if (refdata == null)
            {
                return null;
            }

            parent.Description = AasUaUtils.GetBestUaDescriptionFromAasDescription(refdata?.Description);

            return null;
        }

        public void CreateKeys(NodeState parent, List<Key> keys = null)
        {
            if (parent == null)
            {
                return;
            }

            var keyo = CreateVariable<string[]>(parent, "Values", DataTypeIds.Structure, null);
            if (keyo != null)
            {
                Reference newRef = new Reference(){ Keys = keys };
                keyo.Value = AasUaUtils.ToOpcUaReferenceList(newRef)?.ToArray();
            }
        }

        public void CreateKeys(NodeState parent, List<Identifiable> ids = null)
        {
            List<Key> keys = new List<Key>();
            if (parent == null)
            {
                return;
            }

            if (ids != null)
            {
                foreach (var id in ids)
                {
                    var key = new Key(KeyElements.GlobalReference.ToString(), id.Id.ToString());
                    keys.Add(key);
                }
            }

            var keyo = CreateVariable<string[]>(parent, "Values", DataTypeIds.Structure, null);
            if (keyo != null)
            {
                Reference newRef = new Reference(){ Keys = keys };
                keyo.Value = AasUaUtils.ToOpcUaReferenceList(newRef)?.ToArray();
            }
        }

        public NodeState CreateObject(NodeState parent, Reference semid = null, string browseDisplayName = null)
        {
            if (parent == null)
            {
                return null;
            }

            if (semid == null)
            {
                return null;
            }

            var o = CreateObject(parent, browseDisplayName ?? "SemanticId", ReferenceTypeIds.HasComponent, c_semanticIdNodeId);

            CreateKeys(o, semid.Keys);

            return o;
        }
    }
}
