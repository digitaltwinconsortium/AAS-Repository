
using Opc.Ua;
using Opc.Ua.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdminShell
{
    public class UAClient
    {
        private Session _session;

        public async Task<List<NodesetViewerNode>> GetChildren(string nodeId)
        {
            List<NodesetViewerNode> nodes = null;
            ReferenceDescriptionCollection references = null;

            try
            {
                if (_session == null || !_session.Connected)
                {
                    _session = await CreaterSessionAsync(Program.App.ApplicationConfiguration, "opc.tcp://localhost/").ConfigureAwait(false);
                }

                if (_session == null || !_session.Connected)
                {
                    return null;
                }

                BrowseDescription nodeToBrowse = new()
                {
                    NodeId = new NodeId(nodeId),
                    BrowseDirection = BrowseDirection.Forward,
                    ReferenceTypeId = ReferenceTypeIds.HierarchicalReferences,
                    IncludeSubtypes = true,
                    NodeClassMask = (uint)(NodeClass.Object | NodeClass.Variable | NodeClass.ObjectType),
                    ResultMask = (uint)BrowseResultMask.All
                };

                references = Browse(_session, nodeToBrowse);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetChildren: " + ex.Message);
            }

            if ((references != null) && (references.Count > 0))
            {
                nodes = new List<NodesetViewerNode>();

                foreach (ReferenceDescription description in references)
                {
                    nodes.Add(new NodesetViewerNode()
                    {
                        Id = ExpandedNodeId.ToNodeId(description.NodeId, _session.NamespaceUris).ToString(),
                        Text = description.DisplayName.ToString(),
                        Children = new List<NodesetViewerNode>()
                    });
                }
            }

            return nodes;
        }

        private async Task<Session> CreaterSessionAsync(ApplicationConfiguration config, string endpointURL)
        {
            if (string.IsNullOrEmpty(endpointURL))
            {
                return null;
            }

            EndpointDescription selectedEndpoint = CoreClientUtils.SelectEndpoint(endpointURL, true);
            ConfiguredEndpoint configuredEndpoint = new ConfiguredEndpoint(null, selectedEndpoint, EndpointConfiguration.Create(config));
            return await Session.Create(
                    config,
                    configuredEndpoint,
                    true,
                    false,
                    string.Empty,
                    30000,
                    new UserIdentity(new AnonymousIdentityToken()),
                    null).ConfigureAwait(false);
        }

        private ReferenceDescriptionCollection Browse(Session session, BrowseDescription nodeToBrowse)
        {
            ReferenceDescriptionCollection references = new ReferenceDescriptionCollection();

            BrowseDescriptionCollection nodesToBrowse = new BrowseDescriptionCollection
            {
                nodeToBrowse
            };

            try
            {
                session.Browse(
                null,
                null,
                0,
                nodesToBrowse,
                out BrowseResultCollection results,
                out DiagnosticInfoCollection diagnosticInfos);

                ClientBase.ValidateResponse(results, nodesToBrowse);
                ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToBrowse);

                do
                {
                    if (StatusCode.IsBad(results[0].StatusCode))
                    {
                        break;
                    }

                    for (int i = 0; i < results[0].References.Count; i++)
                    {
                        references.Add(results[0].References[i]);
                    }

                    if (results[0].References.Count == 0 || results[0].ContinuationPoint == null)
                    {
                        break;
                    }

                    ByteStringCollection continuationPoints = new ByteStringCollection
                {
                    results[0].ContinuationPoint
                };

                    session.BrowseNext(
                        null,
                        false,
                        continuationPoints,
                        out results,
                        out diagnosticInfos);

                    ClientBase.ValidateResponse(results, continuationPoints);
                    ClientBase.ValidateDiagnosticInfos(diagnosticInfos, continuationPoints);
                }
                while (true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Browse: " + ex.Message);
            }

            return references;
        }

        public async Task<string> VariableRead(string nodeId)
        {
            string value = string.Empty;

            try
            {
                DataValueCollection values = null;
                DiagnosticInfoCollection diagnosticInfos = null;
                ReadValueIdCollection nodesToRead = new();

                ReadValueId valueId = new();
                valueId.NodeId = new NodeId(nodeId);
                valueId.AttributeId = Attributes.Value;
                valueId.IndexRange = null;
                valueId.DataEncoding = null;
                nodesToRead.Add(valueId);

                if (_session == null || !_session.Connected)
                {
                    _session = await CreaterSessionAsync(Program.App.ApplicationConfiguration, "opc.tcp://localhost/").ConfigureAwait(false);
                }

                if (_session == null || !_session.Connected)
                {
                    return string.Empty;
                }

                ResponseHeader responseHeader = _session.Read(null, 0, TimestampsToReturn.Both, nodesToRead, out values, out diagnosticInfos);

                ClientBase.ValidateResponse(values, nodesToRead);
                ClientBase.ValidateDiagnosticInfos(diagnosticInfos, nodesToRead);

                if ((values.Count > 0) && (values[0].Value != null))
                {
                    value = values[0].ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("VariableRead: " + ex.Message);
            }

            return value;
        }
    }
}
