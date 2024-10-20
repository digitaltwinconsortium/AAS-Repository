
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

        private AASXPackageService _packageService = null;

        private ushort _namespaceIndex;
        private long _lastUsedId;

        const string c_exportFilename = "I4AAS_Export.nodeset2.xml";

        public NodeState _rootAAS = null;
        public NodeState _rootConceptDescriptions = null;
        public NodeState _rootMissingDictionaryEntries = null;

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

            CreateVariable<string>(o, "Referable", c_referableTypeNodeId, aas.Identification.Value);
            CreateVariable<string>(o, "Identification", c_identifiableTypeNodeId, aas.Id);
            CreateVariable<string>(o, "Administration", c_administrationNodeId, aas.Administration?.ToString());

            if (aas.EmbeddedDataSpecifications != null)
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
                for (int i = 0; i < aas.Submodels.Count; i++)
                {
                    CreateVariable<string>(o, "Submodel Reference " + i.ToString(), c_submodelNodeId, aas.Submodels[i].Keys[0].Value);
                }
            }

            if (env.Submodels != null && env.Submodels.Count > 0)
            {
                for (int i = 0; i < env.Submodels.Count; i++)
                {
                    CreateVariable<string>(o, "Submodel Definition " + i.ToString(), c_submodelNodeId, env.Submodels[i].IdShort);
                }
            }

            return o;
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
    }
}
