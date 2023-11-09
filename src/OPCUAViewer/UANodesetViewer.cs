
namespace AdminShell
{
    using Kusto.Cloud.Platform.Utils;
    using Newtonsoft.Json;
    using Opc.Ua;
    using Opc.Ua.Client;
    using Opc.Ua.Configuration;
    using Opc.Ua.Export;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    public class UANodesetViewer
    {
        public static List<string> _nodeSetFilenames = new();

        private HttpClient _client = new HttpClient();

        private Dictionary<string, ApplicationInstance> _applications = new();

        private bool _isRunning = false;

        private string _instanceUrl = string.Empty;

        private Dictionary<string, string> _namespacesInCloudLibrary = new();


        public void Login(string instanceUrl, string clientId, string secret, string key)
        {
            if (!_isRunning
             && !string.IsNullOrEmpty(clientId)
             && !string.IsNullOrEmpty(secret)
             && !string.IsNullOrEmpty(instanceUrl)
             && (_instanceUrl != instanceUrl))
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", "basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(clientId + ":" + secret)));
                _instanceUrl = instanceUrl;

                // get namespaces
                string address = "https://uacloudlibrary.opcfoundation.org/infomodel/namespaces";
                HttpResponseMessage response = _client.Send(new HttpRequestMessage(HttpMethod.Get, address));
                string[] identifiers = JsonConvert.DeserializeObject<string[]>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());

                _namespacesInCloudLibrary.Clear();
                foreach (string nodeset in identifiers)
                {
                    string[] tuple = nodeset.Split(",");

                    if (_namespacesInCloudLibrary.ContainsKey(tuple[0]))
                    {
                        _namespacesInCloudLibrary[tuple[0]] = tuple[1];
                    }
                    else
                    {
                        _namespacesInCloudLibrary.Add(tuple[0], tuple[1]);
                    }
                }

                response = _client.Send(new HttpRequestMessage(HttpMethod.Get, instanceUrl));
                AddressSpace addressSpace = JsonConvert.DeserializeObject<AddressSpace>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());

                // store the file locally
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "nodeset2.xml");
                System.IO.File.WriteAllText(filePath, addressSpace.Nodeset.NodesetXml);
                _nodeSetFilenames.Add(filePath);

                ValidateNamespacesAndModels(true);

                // start the UA server
                StartServerAsync(key).GetAwaiter().GetResult();

                _isRunning = true;
            }
        }

        public void LoadLocalNodesetFile(string name, string key)
        {
            // fix the path to the local nodeset directory and add
            string path = Path.Combine(Directory.GetCurrentDirectory(), "NodeSets", name.Substring(name.LastIndexOf('/') + 1));
            _nodeSetFilenames.Add(path);

            ValidateNamespacesAndModels(true);

            // start the UA server
            StartServerAsync(key).GetAwaiter().GetResult();
        }

        private string ValidateNamespacesAndModels(bool autodownloadreferences)
        {
            // Collect all models as well as all required/referenced model namespace URIs listed in each file
            List<string> models = new List<string>();
            List<string> modelreferences = new List<string>();
            foreach (string nodesetFile in _nodeSetFilenames)
            {
                using (Stream stream = new FileStream(nodesetFile, FileMode.Open))
                {
                    UANodeSet nodeSet = UANodeSet.Read(stream);

                    // validate namespace URIs
                    if ((nodeSet.NamespaceUris != null) && (nodeSet.NamespaceUris.Length > 0))
                    {
                        foreach (string ns in nodeSet.NamespaceUris)
                        {
                            if (string.IsNullOrEmpty(ns) || !Uri.IsWellFormedUriString(ns, UriKind.Absolute))
                            {
                                return "Nodeset file " + nodesetFile + " contains an invalid Namespace URI: \"" + ns + "\"";
                            }
                        }
                    }
                    else
                    {
                        return "'NamespaceUris' entry missing in " + nodesetFile + ". Please add it!";
                    }

                    // validate model URIs
                    if ((nodeSet.Models != null) && (nodeSet.Models.Length > 0))
                    {
                        foreach (ModelTableEntry model in nodeSet.Models)
                        {
                            if (model != null)
                            {
                                if (Uri.IsWellFormedUriString(model.ModelUri, UriKind.Absolute))
                                {
                                    // ignore the default namespace which is always present and don't add duplicates
                                    if ((model.ModelUri != "http://opcfoundation.org/UA/") && !models.Contains(model.ModelUri))
                                    {
                                        models.Add(model.ModelUri);
                                    }
                                }
                                else
                                {
                                    return "Nodeset file " + nodesetFile + " contains an invalid Model Namespace URI: \"" + model.ModelUri + "\"";
                                }

                                if ((model.RequiredModel != null) && (model.RequiredModel.Length > 0))
                                {
                                    foreach (ModelTableEntry requiredModel in model.RequiredModel)
                                    {
                                        if (requiredModel != null)
                                        {
                                            if (Uri.IsWellFormedUriString(requiredModel.ModelUri, UriKind.Absolute))
                                            {
                                                // ignore the default namespace which is always required and don't add duplicates
                                                if ((requiredModel.ModelUri != "http://opcfoundation.org/UA/") && !modelreferences.Contains(requiredModel.ModelUri))
                                                {
                                                    modelreferences.Add(requiredModel.ModelUri);
                                                }
                                            }
                                            else
                                            {
                                                return "Nodeset file " + nodesetFile + " contains an invalid referenced Model Namespace URI: \"" + requiredModel.ModelUri + "\"";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        return "'Model' entry missing in " + nodesetFile + ". Please add it!";
                    }
                }
            }

            // now check if we have all references for each model we want to load
            foreach (string modelreference in modelreferences)
            {
                if (!models.Contains(modelreference))
                {
                    if (!autodownloadreferences)
                    {
                        return "Referenced OPC UA model " + modelreference + " is missing from selected list of nodeset files, please add the corresponding nodeset file to the list of loaded files!";
                    }
                    else
                    {
                        try
                        {
                            // check if the reference is already available locally
                            bool refFound = false;
                            foreach (string fileName in Directory.EnumerateFiles(Path.Combine(Directory.GetCurrentDirectory(), "NodeSets")))
                            {
                                if (System.IO.File.ReadAllText(fileName).Contains("<Model ModelUri=\"" + modelreference))
                                {
                                    _nodeSetFilenames.Add(fileName);
                                    refFound = true;
                                    break;
                                }
                            }

                            if (!refFound)
                            {
                                // try to auto-download the missing references from the UA Cloud Library
                                string address = "https://uacloudlibrary.opcfoundation.org/infomodel/download/" + Uri.EscapeDataString(_namespacesInCloudLibrary[modelreference]);
                                HttpResponseMessage response = _client.Send(new HttpRequestMessage(HttpMethod.Get, address));
                                AddressSpace addressSpace = JsonConvert.DeserializeObject<AddressSpace>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());

                                // store the file on the webserver
                                string filePath = Path.Combine(Directory.GetCurrentDirectory(), addressSpace.Category.Name + ".nodeset2.xml");
                                System.IO.File.WriteAllText(filePath, addressSpace.Nodeset.NodesetXml);
                                _nodeSetFilenames.Add(filePath);
                            }
                        }
                        catch (Exception ex)
                        {
                            return "Could not download referenced nodeset " + modelreference + ": " + ex.Message;
                        }
                    }
                }
            }

            return string.Empty; // no error
        }

        private async Task StartServerAsync(string key)
        {
            if (!_applications.ContainsKey(key))
            {
                // create new OPC UA app
                ApplicationInstance app = new();

                // update server port
                int currentPort = (4840 + _applications.Count - 1);
                int newPort = (4840 + _applications.Count);
                string configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Application.Config.xml");
                string configFileContent = System.IO.File.ReadAllText(configFilePath).Replace(currentPort.ToString(), newPort.ToString());
                System.IO.File.WriteAllText(configFilePath, configFileContent);

                // load the application configuration.
                ApplicationConfiguration config = await app.LoadApplicationConfiguration(Path.Combine(Directory.GetCurrentDirectory(), "Application.Config.xml"), false).ConfigureAwait(false);

                // check the application certificate.
                await app.CheckApplicationInstanceCertificate(false, 0).ConfigureAwait(false);

                // create cert validator
                config.CertificateValidator = new CertificateValidator();
                config.CertificateValidator.CertificateValidation += new CertificateValidationEventHandler(CertificateValidator_CertificateValidation);

                // start the server.
                await app.Start(new SimpleServer()).ConfigureAwait(false);

                _applications.Add(key, app);
            }
        }

        private static void CertificateValidator_CertificateValidation(CertificateValidator validator, CertificateValidationEventArgs e)
        {
            if (e.Error.StatusCode == Opc.Ua.StatusCodes.BadCertificateUntrusted)
            {
                // accept all OPC UA client certificates
                e.Accept = true;
            }
        }

        public async Task<NodesetViewerNode> GetRootNode(string key)
        {
            ReferenceDescriptionCollection references;
            Byte[] continuationPoint;
            NodesetViewerNode node;

            bool lastRetry = false;
            string endpointURL = "opc.tcp://localhost:4840/";
            Session session = null;

            // update server port
            int port = 4840 + _applications.Keys.ToListIfNotAlready().IndexOf(key);
            endpointURL = endpointURL.Replace("4840", port.ToString());

            while (!lastRetry)
            {
                try
                {
                    session = await OpcSessionHelper.Instance.GetSessionAsync(_applications[key].ApplicationConfiguration, key, endpointURL).ConfigureAwait(false);

                    session.Browse(
                        null,
                        null,
                        ObjectIds.RootFolder,
                        0u,
                        BrowseDirection.Forward,
                        ReferenceTypeIds.HierarchicalReferences,
                        true,
                        0,
                        out continuationPoint,
                        out references);
                    node = new NodesetViewerNode() { Id = ObjectIds.RootFolder.ToString(), Text = "Root", Children = (references?.Count != 0) };

                    return node;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);

                    if ((session != null) && session.Connected)
                    {
                        OpcSessionHelper.Instance.Disconnect(session.SessionId.ToString());
                    }

                    lastRetry = true;
                }
            }

            return null;
        }

        public async Task<List<NodesetViewerNode>> GetChildren(string jstreeNode, string key)
        {
            string node = OpcSessionHelper.GetNodeIdFromJsTreeNode(jstreeNode);

            ReferenceDescriptionCollection references = null;
            Byte[] continuationPoint;
            var nodes = new List<NodesetViewerNode>();

            Session session = null;
            string endpointUrl = "opc.tcp://localhost:4840/";

            // update server port
            int port = 4840 + _applications.Keys.ToListIfNotAlready().IndexOf(key);
            endpointUrl = endpointUrl.Replace("4840", port.ToString());

            try
            {
                session = await OpcSessionHelper.Instance.GetSessionAsync(_applications[key].ApplicationConfiguration, key, endpointUrl).ConfigureAwait(false);
                endpointUrl = session.ConfiguredEndpoint.EndpointUrl.AbsoluteUri;
            }
            catch (Exception ex)
            {
                // do nothing, since we still want to show the tree
                Trace.TraceError("Can not read published nodes for endpoint '{0}'.", endpointUrl);
                Trace.TraceError(ex.Message);
            }

            bool lastRetry = false;
            while (!lastRetry)
            {
                try
                {
                    try
                    {
                        if (session.Disposed)
                        {
                            session.Reconnect();
                        }

                        session.Browse(
                            null,
                            null,
                            node,
                            0u,
                            BrowseDirection.Forward,
                            ReferenceTypeIds.HierarchicalReferences,
                            true,
                            0,
                            out continuationPoint,
                            out references);
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError("Can not browse node '{0}'", node);
                        Trace.TraceError(ex.Message);
                    }

                    if (references != null)
                    {
                        var idList = new List<string>();
                        foreach (var nodeReference in references)
                        {
                            bool idFound = false;
                            foreach (var id in idList)
                            {
                                if (id == nodeReference.NodeId.ToString())
                                {
                                    idFound = true;
                                }
                            }
                            if (idFound == true)
                            {
                                continue;
                            }

                            ReferenceDescriptionCollection childReferences = null;
                            Byte[] childContinuationPoint;

                            INode currentNode = null;
                            try
                            {
                                if (session.Disposed)
                                {
                                    session.Reconnect();
                                }

                                session.Browse(
                                    null,
                                    null,
                                    ExpandedNodeId.ToNodeId(nodeReference.NodeId, session.NamespaceUris),
                                    0u,
                                    BrowseDirection.Forward,
                                    ReferenceTypeIds.HierarchicalReferences,
                                    true,
                                    0,
                                    out childContinuationPoint,
                                    out childReferences);

                                currentNode = session.ReadNode(ExpandedNodeId.ToNodeId(nodeReference.NodeId, session.NamespaceUris));
                            }
                            catch (Exception ex)
                            {
                                Trace.TraceError("Can not browse or read node '{0}'", nodeReference.NodeId);
                                Trace.TraceError(ex.Message);

                                // skip this node
                                continue;
                            }

                            byte currentNodeAccessLevel = 0;
                            byte currentNodeEventNotifier = 0;
                            bool currentNodeExecutable = false;

                            VariableNode variableNode = currentNode as VariableNode;
                            if (variableNode != null)
                            {
                                currentNodeAccessLevel = variableNode.UserAccessLevel;
                            }

                            ObjectNode objectNode = currentNode as ObjectNode;
                            if (objectNode != null)
                            {
                                currentNodeEventNotifier = objectNode.EventNotifier;
                            }

                            ViewNode viewNode = currentNode as ViewNode;
                            if (viewNode != null)
                            {
                                currentNodeEventNotifier = viewNode.EventNotifier;
                            }

                            MethodNode methodNode = currentNode as MethodNode;
                            if (methodNode != null)
                            {
                                currentNodeExecutable = methodNode.UserExecutable;
                            }

                            nodes.Add(new NodesetViewerNode()
                            {
                                Id = ("__" + node + OpcSessionHelper.Delimiter + nodeReference.NodeId.ToString()),
                                Text = nodeReference.DisplayName.ToString(),
                                Children = (childReferences.Count == 0) ? false : true
                            });
                            idList.Add(nodeReference.NodeId.ToString());
                        }

                        // If there are no children, then this is a call to read the properties of the node itself.
                        if (nodes.Count == 0)
                        {
                            INode currentNode = null;

                            try
                            {
                                currentNode = session.ReadNode(new NodeId(node));
                            }
                            catch (Exception ex)
                            {
                                Trace.TraceError("Can not read node '{0}'", new NodeId(node));
                                Trace.TraceError(ex.Message);
                            }

                            if (currentNode == null)
                            {
                                byte currentNodeAccessLevel = 0;
                                byte currentNodeEventNotifier = 0;
                                bool currentNodeExecutable = false;

                                VariableNode variableNode = currentNode as VariableNode;

                                if (variableNode != null)
                                {
                                    currentNodeAccessLevel = variableNode.UserAccessLevel;
                                }

                                ObjectNode objectNode = currentNode as ObjectNode;

                                if (objectNode != null)
                                {
                                    currentNodeEventNotifier = objectNode.EventNotifier;
                                }

                                ViewNode viewNode = currentNode as ViewNode;

                                if (viewNode != null)
                                {
                                    currentNodeEventNotifier = viewNode.EventNotifier;
                                }

                                MethodNode methodNode = currentNode as MethodNode;

                                if (methodNode != null)
                                {
                                    currentNodeExecutable = methodNode.UserExecutable;
                                }

                                nodes.Add(new NodesetViewerNode()
                                {
                                    Id = jstreeNode,
                                    Text = currentNode.DisplayName.ToString() + " (ns=" + session.NamespaceUris.ToArray()[currentNode.NodeId.NamespaceIndex] + ";" + currentNode.NodeId.ToString() + ")",
                                    Children = false
                                });
                            }
                        }
                    }

                    return nodes;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);

                    if ((session != null) && session.Connected)
                    {
                        OpcSessionHelper.Instance.Disconnect(session.SessionId.ToString());
                    }

                    lastRetry = true;
                }
            }

            return null;
        }

        public async Task<string> VariableRead(string jstreeNode, string key)
        {
            var node = OpcSessionHelper.GetNodeIdFromJsTreeNode(jstreeNode);
            bool lastRetry = false;

            Session session = null;
            string endpointUrl = "opc.tcp://localhost:4840/";

            // update server port
            int port = 4840 + _applications.Keys.ToListIfNotAlready().IndexOf(key);
            endpointUrl = endpointUrl.Replace("4840", port.ToString());

            while (!lastRetry)
            {
                try
                {
                    DataValueCollection values = null;
                    DiagnosticInfoCollection diagnosticInfos = null;
                    ReadValueIdCollection nodesToRead = new ReadValueIdCollection();
                    ReadValueId valueId = new ReadValueId();
                    valueId.NodeId = new NodeId(node);
                    valueId.AttributeId = Attributes.Value;
                    valueId.IndexRange = null;
                    valueId.DataEncoding = null;
                    nodesToRead.Add(valueId);

                    session = await OpcSessionHelper.Instance.GetSessionAsync(_applications[key].ApplicationConfiguration, key, endpointUrl).ConfigureAwait(false);
                    ResponseHeader responseHeader = session.Read(null, 0, TimestampsToReturn.Both, nodesToRead, out values, out diagnosticInfos);

                    string actionResult = string.Empty;
                    if ((values.Count > 0) && (values[0].Value != null))
                    {
                        actionResult = $"= {values[0]}";
                    }

                    return actionResult;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);

                    if ((session != null) && session.Connected)
                    {
                        OpcSessionHelper.Instance.Disconnect(session.SessionId.ToString());
                    }

                    lastRetry = true;
                }
            }

            return string.Empty;
        }
    }
}
