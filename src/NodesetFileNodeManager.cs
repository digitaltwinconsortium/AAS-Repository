
namespace AdminShell
{
    using Opc.Ua;
    using Opc.Ua.Export;
    using Opc.Ua.Server;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class NodesetFileNodeManager : CustomNodeManager2
    {
        private long _lastUsedId = 0;

        public NodeState _rootAssetAdminShells = null;
        public NodeState _rootSubmodels = null;
        public NodeState _rootConceptDescriptions = null;

        private const string c_aasRoot = "ns=2;i=1";
        private const string c_submodelRoot = "ns=2;i=2";
        private const string c_conceptDescriptionRoot = "ns=2;i=3";

        public NodesetFileNodeManager(IServerInternal server, ApplicationConfiguration configuration)
        : base(server, configuration)
        {
            SystemContext.NodeIdFactory = this;

            List<string> namespaceUris = new();
            LoadNamespaceUrisFromNodesetXml(namespaceUris, "I4AAS.NodeSet2.xml");

            // check if we have existing nodesets in our nodesets directory
            IEnumerable<string> nodesetFiles = Directory.EnumerateFiles(Path.Combine(Directory.GetCurrentDirectory(), "Nodesets"));
            if (nodesetFiles.Count() > 0)
            {
                foreach (string file in nodesetFiles)
                {
                    LoadNamespaceUrisFromNodesetXml(namespaceUris, file);
                }
            }

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
            return new NodeId(Utils.IncrementIdentifier(ref _lastUsedId), (ushort)Server.NamespaceUris.GetIndex("http://opcfoundation.org/UA/I4AAS/"));
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

                _rootAssetAdminShells = CreateFolder(FindNodeInAddressSpace(ObjectIds.ObjectsFolder), "Asset Admin Shells");
                _rootSubmodels = CreateFolder(FindNodeInAddressSpace(ObjectIds.ObjectsFolder), "Submodels");
                _rootConceptDescriptions = CreateFolder(FindNodeInAddressSpace(ObjectIds.ObjectsFolder), "Concept Descriptions");

                // check if we have existing nodesets in our nodesets directory
                IEnumerable<string> nodesetFiles = Directory.EnumerateFiles(Path.Combine(Directory.GetCurrentDirectory(), "Nodesets"));
                if (nodesetFiles.Count() > 0)
                {
                    foreach (string file in nodesetFiles)
                    {
                        AddNodesFromNodesetXml(file);
                    }
                }

                AddReverseReferences(externalReferences);
                base.CreateAddressSpace(externalReferences);
            }
        }

        public FolderState CreateFolder(NodeState parent, string browseDisplayName)
        {
            FolderState folder = new(parent)
            {
                BrowseName = browseDisplayName,
                DisplayName = browseDisplayName,
                TypeDefinitionId = ObjectTypeIds.FolderType
            };

            folder.NodeId = New(SystemContext, folder);

            AddPredefinedNode(SystemContext, folder);

            if (parent != null)
            {
                parent.AddChild(folder);
            }

            return folder;
        }

        private void AddNodesFromNodesetXml(string nodesetFile)
        {
            using (Stream stream = new FileStream(nodesetFile, FileMode.Open))
            {
                UANodeSet nodeSet = UANodeSet.Read(stream);
                List<UANode> nodes = new();

                // first fixup our nodeset by removing the "Asset Admin Shells", "Submodels" and "Concept Descriptions" top-level nodes
                // and pointing the child nodes to our instance of these top-level nodes
                foreach (UANode node in nodeSet.Items)
                {
                    if (nodesetFile != "I4AAS.NodeSet2.xml")
                    {
                        if ((node.DisplayName[0].Value == "Asset Admin Shells") || (node.DisplayName[0].Value == "Submodels") || (node.DisplayName[0].Value == "Concept Descriptions"))
                        {
                            continue;
                        }

                        // update the parent of the nodes pointing to top-level nodes from the nodeset file to our own top-level nodes
                        if (node is UAObject uAObject)
                        {
                            List<Opc.Ua.Export.Reference> refList = uAObject.References.ToList();

                            Opc.Ua.Export.Reference reference = new();
                            reference.ReferenceType = ReferenceTypeIds.Organizes.ToString();
                            reference.IsForward = false;

                            if (uAObject.ParentNodeId == "ns=1;i=1")
                            {
                                uAObject.ParentNodeId = c_aasRoot;
                                reference.Value = c_aasRoot;
                                refList.Add(reference);
                            }

                            if (uAObject.ParentNodeId == "ns=1;i=2")
                            {
                                uAObject.ParentNodeId = c_submodelRoot;
                                reference.Value = c_submodelRoot;
                                refList.Add(reference);
                            }

                            if (uAObject.ParentNodeId == "ns=1;i=3")
                            {
                                uAObject.ParentNodeId = c_conceptDescriptionRoot;
                                reference.Value = c_conceptDescriptionRoot;
                                refList.Add(reference);
                            }

                            uAObject.References = refList.ToArray();
                        }
                    }

                    nodes.Add(node);
                }

                nodeSet.Items = nodes.ToArray();
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
