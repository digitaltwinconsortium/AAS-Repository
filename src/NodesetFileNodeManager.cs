
namespace AdminShell
{
    using Opc.Ua;
    using Opc.Ua.Export;
    using Opc.Ua.Server;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class NodesetFileNodeManager : CustomNodeManager2
    {
        private long _lastUsedId = 0;

        public NodeState _rootAssetAdminShells = null;
        public NodeState _rootSubmodels = null;
        public NodeState _rootConceptDescriptions = null;

        public NodesetFileNodeManager(IServerInternal server, ApplicationConfiguration configuration)
        : base(server, configuration)
        {
            SystemContext.NodeIdFactory = this;

            List<string> namespaceUris = new()
            {
                "http://opcfoundation.org/UA/AAS-Repository/"
            };

            LoadNamespaceUrisFromNodesetXml(namespaceUris, "I4AAS.NodeSet2.xml");

            NamespaceUris = namespaceUris;
        }

        private void LoadNamespaceUrisFromNodesetXml(List<string> namespaceUris, string nodesetFile)
        {
            using (FileStream stream = new(nodesetFile, FileMode.Open, FileAccess.Read))
            {
                UANodeSet nodeSet = UANodeSet.Read(stream);

                if ((nodeSet.NamespaceUris != null) && (nodeSet.NamespaceUris.Length > 0))
                {
                    foreach (string ns in nodeSet.NamespaceUris)
                    {
                        if (!namespaceUris.Contains(ns))
                        {
                            namespaceUris.Add(ns);
                        }
                    }
                }
            }
        }

        public override NodeId New(ISystemContext context, NodeState node)
        {
            // for new nodes we create, pick our default namespace
            return new NodeId(Utils.IncrementIdentifier(ref _lastUsedId), (ushort)Server.NamespaceUris.GetIndex("http://opcfoundation.org/UA/AAS-Repository/"));
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

                AddNodesFromNodesetXml("I4AAS.NodeSet2.xml");

                _rootAssetAdminShells = CreateFolder(null, "Asset Admin Shells");
                objectsFolderReferences.Add(new NodeStateReference(ReferenceTypes.Organizes, false, _rootAssetAdminShells.NodeId));

                _rootSubmodels = CreateFolder(null, "Submodels");
                objectsFolderReferences.Add(new NodeStateReference(ReferenceTypes.Organizes, false, _rootSubmodels.NodeId));

                _rootConceptDescriptions = CreateFolder(null, "Concept Descriptions");
                objectsFolderReferences.Add(new NodeStateReference(ReferenceTypes.Organizes, false, _rootConceptDescriptions.NodeId));

                AddReverseReferences(externalReferences);
                base.CreateAddressSpace(externalReferences);
            }
        }

        public FolderState CreateFolder(NodeState parent, string browseDisplayName)
        {
            FolderState newFolder = new(parent)
            {
                BrowseName = browseDisplayName,
                DisplayName = browseDisplayName,
                NodeId = new NodeId(browseDisplayName, (ushort)Server.NamespaceUris.GetIndex("http://opcfoundation.org/UA/AAS-Repository/")),
                TypeDefinitionId = ObjectTypeIds.FolderType
            };

            AddPredefinedNode(SystemContext, newFolder);

            if (parent != null)
            {
                parent.AddChild(newFolder);
            }

            return newFolder;
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
    }
}
