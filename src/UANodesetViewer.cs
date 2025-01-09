
namespace AdminShell
{
    using Opc.Ua;
    using Opc.Ua.Client;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class UANodesetViewer
    {
        public async Task<Session> GetSession(string sessionId)
        {
            Session session = null;

            try
            {
                session =  await OpcSessionHelper.Instance.GetSessionAsync(Program.App.ApplicationConfiguration, sessionId, "opc.tcp://localhost/").ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);

                if ((session != null) && session.Connected)
                {
                    OpcSessionHelper.Instance.Disconnect(session.SessionId.ToString());
                }

            }

            return session;
        }

        public async Task<List<NodesetViewerNode>> GetChildren(string nodeId, string sessionId)
        {
            List<NodesetViewerNode> nodes = null;
            Session session = null;
            ReferenceDescriptionCollection references = null;

            try
            {
                session = await OpcSessionHelper.Instance.GetSessionAsync(Program.App.ApplicationConfiguration, sessionId, "opc.tcp://localhost/").ConfigureAwait(false);

                BrowseDescription nodeToBrowse = new()
                {
                    NodeId = new NodeId(nodeId),
                    BrowseDirection = BrowseDirection.Forward,
                    ReferenceTypeId = ReferenceTypeIds.HierarchicalReferences,
                    IncludeSubtypes = true,
                    NodeClassMask = (uint)(NodeClass.Object | NodeClass.Variable | NodeClass.ObjectType),
                    ResultMask = (uint)BrowseResultMask.All
                };

                references = Browse(session, nodeToBrowse);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);

                if ((session != null) && session.Connected)
                {
                    OpcSessionHelper.Instance.Disconnect(session.SessionId.ToString());
                }
            }

            if (references.Count > 0)
            {
                nodes = new List<NodesetViewerNode>();

                foreach (ReferenceDescription description in references)
                {
                    nodes.Add(new NodesetViewerNode()
                    {
                        Id = ExpandedNodeId.ToNodeId(description.NodeId, session.NamespaceUris).ToString(),
                        Text = description.DisplayName.ToString(),
                        Children = new List<NodesetViewerNode>(),
                        SessionId = session.SessionId.ToString()
                    });
                }
            }

            return nodes;
        }

        private ReferenceDescriptionCollection Browse(Session session, BrowseDescription nodeToBrowse)
        {
            ReferenceDescriptionCollection references = new ReferenceDescriptionCollection();

            BrowseDescriptionCollection nodesToBrowse = new BrowseDescriptionCollection
            {
                nodeToBrowse
            };

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

            return references;
        }

        public async Task<string> VariableRead(string nodeId, string sessionId)
        {
            string value = string.Empty;
            Session session = null;

            try
            {
                DataValueCollection values = null;
                DiagnosticInfoCollection diagnosticInfos = null;
                ReadValueIdCollection nodesToRead = new ReadValueIdCollection();

                ReadValueId valueId = new ReadValueId();
                valueId.NodeId = new NodeId(nodeId);
                valueId.AttributeId = Attributes.Value;
                valueId.IndexRange = null;
                valueId.DataEncoding = null;
                nodesToRead.Add(valueId);

                session = await OpcSessionHelper.Instance.GetSessionAsync(Program.App.ApplicationConfiguration, sessionId, "opc.tcp://localhost/").ConfigureAwait(false);

                ResponseHeader responseHeader = session.Read(null, 0, TimestampsToReturn.Both, nodesToRead, out values, out diagnosticInfos);

                if ((values.Count > 0) && (values[0].Value != null))
                {
                    value = values[0].ToString();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);

                if ((session != null) && session.Connected)
                {
                    OpcSessionHelper.Instance.Disconnect(session.SessionId.ToString());
                }
            }

            return value;
        }
    }
}
