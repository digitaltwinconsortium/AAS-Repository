
namespace AdminShell
{
    using I4AAS.Submodels;
    using Opc.Ua;
    using Opc.Ua.Export;
    using Opc.Ua.Server;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public class NodesetFileNodeManager : CustomNodeManager2
    {
        public NodesetFileNodeManager(IServerInternal server, ApplicationConfiguration configuration)
        : base(server, configuration)
        {
            SystemContext.NodeIdFactory = this;

            List<string> namespaces = new List<string>();
            foreach (string nodesetFile in UANodesetViewer._nodeSetFilenames)
            {
                using (Stream stream = new FileStream(nodesetFile, FileMode.Open))
                {
                    UANodeSet nodeSet = UANodeSet.Read(stream);
                    if ((nodeSet.NamespaceUris != null) && (nodeSet.NamespaceUris.Length > 0))
                    {
                        foreach (string ns in nodeSet.NamespaceUris)
                        {
                            if (!namespaces.Contains(ns))
                            {
                                namespaces.Add(ns);
                            }
                        }
                    }
                }
            }

            NamespaceUris = namespaces.ToArray();

            Server.MessageContext.Factory.AddEncodeableTypes(typeof(SubmodelDataType).GetTypeInfo().Assembly);
        }

        public override void CreateAddressSpace(IDictionary<NodeId, IList<IReference>> externalReferences)
        {
            lock (Lock)
            {
                IList<IReference> references = null;
                if (!externalReferences.TryGetValue(Opc.Ua.ObjectIds.ObjectsFolder, out references))
                {
                    externalReferences[Opc.Ua.ObjectIds.ObjectsFolder] = references = new List<IReference>();
                }

                if (UANodesetViewer._nodeSetFilenames.Count > 0)
                {
                    // we need as many passes as we have nodesetfiles to make sure all references can be resolved
                    for (int i = 0; i < UANodesetViewer._nodeSetFilenames.Count; i++)
                    {
                        foreach (string nodesetFile in UANodesetViewer._nodeSetFilenames)
                        {
                            ImportNodeset2Xml(externalReferences, nodesetFile, i);
                        }
                    }
                }

                // load our node instances and values for the I4AAS namespace from the PCF file
                if (NamespaceUris.Contains("http://opcfoundation.org/UA/I4AAS/"))
                {
                    AssetDescriptionFileDataType product = new();
                    string json = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "product_carbon_footprint.json"));
                    using (var decoder = new JsonDecoder(json, Server.MessageContext))
                    {
                        product.Namespaces = decoder.ReadStringArray(nameof(product.Namespaces));
                    }

                    ServiceMessageContext context = new ServiceMessageContext();
                    product.Namespaces.ForEach(x => context.NamespaceUris.Append(x));
                    context.Factory = Server.Factory;

                    using (var decoder = new JsonDecoder(json, context))
                    {
                        product.Decode(decoder);
                    }

                    var node = new AssetModelState(null);

                    node.Create(
                        SystemContext,
                        new NodeId(product.ModelIdShort, NamespaceIndexes[0]),
                        new QualifiedName(product.ModelIdShort, NamespaceIndexes[0]),
                        null,
                        true);

                    foreach (var submodel in product.Submodels)
                    {
                        if (submodel.Body is NameplateSubmodelDataType nameplate)
                        {
                            InitializeChildFromData(nameplate, node.Nameplate);
                            continue;
                        }

                        if (submodel.Body is SubmodelDataType instance)
                        {
                            SubmodelState child;

                            switch (instance)
                            {
                                case ProductCarbonFootprintDataType:
                                    child = new ProductCarbonFootprintSubmodelState(node);
                                    break;
                                default:
                                    child = new SubmodelState(node);
                                    break;
                            };

                            child.Create(
                                SystemContext,
                                new NodeId(instance.ModelIdShort, NamespaceIndexes[0]),
                                new QualifiedName(instance.ModelIdShort, NamespaceIndexes[0]),
                                null,
                                true);

                            node.AddChild(child);

                            InitializeChildFromData(instance, child);
                            continue;
                        }
                    }

                    // store it and all of its children in the pre-defined nodes dictionary for easy look up.
                    AddPredefinedNode(SystemContext, node);

                    // link root to objects folder.
                    references.Add(new NodeStateReference(Opc.Ua.ReferenceTypeIds.Organizes, false, node.NodeId));
                }
            }

            AddReverseReferences(externalReferences);
        }

        private void InitializeChildFromData(IEncodeable instance, NodeState node)
        {
            foreach (var property in instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var nodeProperty = node.GetType().GetProperty(property.Name);

                if (nodeProperty == null)
                {
                    continue;
                }

                var value = property.GetValue(instance);

                if (value == null)
                {
                    continue;
                }

                if (value is Opc.Ua.LocalizedText text && Opc.Ua.LocalizedText.IsNullOrEmpty(text))
                {
                    continue;
                }

                var child = nodeProperty.GetValue(node);

                if (child == null)
                {
                    if (!nodeProperty.PropertyType.IsSubclassOf(typeof(BaseInstanceState)))
                    {
                        continue;
                    }

                    var newChild = Activator.CreateInstance(nodeProperty.PropertyType, node) as BaseInstanceState;

                    newChild.Create(
                        SystemContext,
                        new NodeId(property.Name, NamespaceIndexes[0]),
                        new QualifiedName(property.Name, NamespaceIndexes[0]),
                        null,
                        true);

                    node.AddChild(newChild);
                    child = newChild;
                }

                var variable = child as BaseVariableState;

                if (variable != null)
                {
                    variable.Value = property.GetValue(instance);
                    continue;
                }

                var @object = child as BaseObjectState;

                if (@object != null && value is IEncodeable encodeable)
                {
                    InitializeChildFromData(encodeable, @object);
                    continue;
                }
            }
        }

        private void ImportNodeset2Xml(IDictionary<NodeId, IList<IReference>> externalReferences, string resourcepath, int pass)
        {
            using (Stream stream = new FileStream(resourcepath, FileMode.Open))
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
                    catch (Exception)
                    {
                        // do nothing
                    }
                }
            }
        }
    }
}
