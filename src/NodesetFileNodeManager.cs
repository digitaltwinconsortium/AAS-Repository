
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
        public NodesetFileNodeManager(IServerInternal server, ApplicationConfiguration configuration)
        : base(server, configuration)
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
    }
}
