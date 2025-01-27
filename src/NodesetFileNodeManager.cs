
using Opc.Ua;
using Opc.Ua.Export;
using Opc.Ua.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdminShell
{
    public class NodesetFileNodeManager : CustomNodeManager2
    {
        public NodesetFileNodeManager(IServerInternal server, ApplicationConfiguration configuration)
        : base(server, configuration)
        {
            lock (Lock)
            {
                SystemContext.NodeIdFactory = this;

                List<string> namespaceUris = new();

                // directory validation
                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "NodeSets")))
                {
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "NodeSets"));
                }

                // check if we have existing nodesets in our nodesets directory
                IEnumerable<string> nodesetFiles = Directory.EnumerateFiles(Path.Combine(Directory.GetCurrentDirectory(), "NodeSets"));
                if (nodesetFiles.Count() > 0)
                {
                    foreach (string file in nodesetFiles)
                    {
                        LoadNamespaceUrisFromNodesetXml(namespaceUris, file);
                    }
                }

                NamespaceUris = namespaceUris;
            }
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

        public void AddNamespace(string namespaceUri)
        {
            lock (Lock)
            {
                var arguments = new List<string>(NamespaceUris)
                {
                    namespaceUri
                };

                // Update the table used by this NodeManager
                SetNamespaces(arguments.ToArray());

                // Register the new URI with the MasterNodeManager
                Server.NodeManager.RegisterNamespaceManager(namespaceUri, this);
            }
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

                // check if we have existing nodesets in our nodesets directory
                IEnumerable<string> nodesetFiles = Directory.EnumerateFiles(Path.Combine(Directory.GetCurrentDirectory(), "Nodesets"));
                if (nodesetFiles.Count() > 0)
                {
                    foreach (string file in nodesetFiles)
                    {
                        AddNodesFromNodesetXml(file);
                    }
                }
            }
        }

        public void AddNodesFromNodesetXml(string nodesetFile)
        {
            lock (Lock)
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

                    // add references for our top-level nodes to the objects folder
                    Server.NodeManager.GetManagerHandle(ObjectIds.ObjectsFolder, out INodeManager objectsFolderNodeManager);
                    string namespaceUri = nodeSet.NamespaceUris[0];
                    foreach (UANode node in nodeSet.Items)
                    {
                        if (node is UAObject uAObject)
                        {
                            if ((uAObject.ParentNodeId == ObjectIds.ObjectsFolder)
                              || uAObject.References.Where(r => (r.ReferenceType == "Organizes") && (r.Value == ObjectIds.ObjectsFolder)).ToList().Count > 0)
                            {
                                List<IReference> references = new()
                                {
                                    new NodeStateReference(ReferenceTypeIds.Organizes, false, new NodeId(NodeId.Parse(uAObject.NodeId).Identifier, (ushort)Server.NamespaceUris.GetIndex(namespaceUri)))
                                };

                                Dictionary<NodeId, IList<IReference>> dictionary = new()
                                {
                                    { ObjectIds.ObjectsFolder, references }
                                };

                                objectsFolderNodeManager.AddReferences(dictionary);
                            }
                        }
                    }
                }
            }
        }
    }
}
