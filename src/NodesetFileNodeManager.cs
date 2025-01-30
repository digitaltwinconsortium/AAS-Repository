using Newtonsoft.Json;
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
            SystemContext.NodeIdFactory = this;

            List<string> namespaceUris = new();

            // check if we have existing nodesets in our nodesets directory
            IEnumerable<string> nodesetFiles = Directory.EnumerateFiles(Path.Combine(Directory.GetCurrentDirectory(), "NodeSets"), "*.xml");
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
                    foreach (string namespaceUri in nodeSet.NamespaceUris)
                    {
                        if (!namespaceUris.Contains(namespaceUri))
                        {
                            namespaceUris.Add(namespaceUri);
                        }
                    }
                }
            }
        }

        public void AddNamespace(string filePath)
        {
            using (FileStream stream = new(filePath, FileMode.Open, FileAccess.Read))
            {
                UANodeSet nodeSet = UANodeSet.Read(stream);

                if ((nodeSet.NamespaceUris != null) && (nodeSet.NamespaceUris.Length > 0))
                {
                    foreach (string namespaceUri in nodeSet.NamespaceUris)
                    {
                        if (!NamespaceUris.Contains(namespaceUri))
                        {
                            List<string> updatedNamespaces = new List<string>(NamespaceUris)
                            {
                                namespaceUri
                            };

                            // Update the table used by this NodeManager
                            SetNamespaces(updatedNamespaces.ToArray());

                            // Register the new URI with the MasterNodeManager
                            Server.NodeManager.RegisterNamespaceManager(namespaceUri, this);
                        }
                    }
                }
            }
        }

        public override void CreateAddressSpace(IDictionary<NodeId, IList<IReference>> externalReferences)
        {
            IList<IReference> objectsFolderReferences = null;
            if (!externalReferences.TryGetValue(ObjectIds.ObjectsFolder, out objectsFolderReferences))
            {
                externalReferences[ObjectIds.ObjectsFolder] = objectsFolderReferences = new List<IReference>();
            }

            // check if we have existing nodesets in our nodesets directory
            IEnumerable<string> nodesetFiles = Directory.EnumerateFiles(Path.Combine(Directory.GetCurrentDirectory(), "Nodesets"), "*.xml");
            if (nodesetFiles.Count() > 0)
            {
                foreach (string file in nodesetFiles)
                {
                    AddNodesFromNodesetXml(file);
                }
            }
        }

        public void AddNodesFromNodesetXml(string nodesetFile)
        {
            using (Stream stream = new FileStream(nodesetFile, FileMode.Open))
            {
                // import nodes
                UANodeSet nodeSet = UANodeSet.Read(stream);
                NodeStateCollection predefinedNodes = new NodeStateCollection();
                nodeSet.Import(SystemContext, predefinedNodes);
                Server.NodeManager.GetManagerHandle(ObjectIds.ObjectsFolder, out INodeManager objectsFolderNodeManager);
                string namespaceUri = nodeSet.NamespaceUris[0];

                // add nodes
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

                // patch the values from our values file
                try
                {
                    string valuesFile = Path.Combine(nodesetFile.Replace(".NodeSet2.xml", "_Values.json"));
                    if (System.IO.File.Exists(valuesFile))
                    {
                        Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(System.IO.File.ReadAllText(valuesFile));
                        foreach (KeyValuePair<string, string> value in values)
                        {
                            NodeId nodeId = new NodeId(NodeId.Parse(value.Key).Identifier, (ushort)Server.NamespaceUris.GetIndex(namespaceUri));
                            if (Find(nodeId) is BaseVariableState variable)
                            {
                                variable.Value = new Variant(value.Value);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    // skip loading values
                }
            }
        }
    }
}
