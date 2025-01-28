
using Models;
using Opc.Ua;
using Opc.Ua.Client;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AdminShell
{
    public class OpcSessionHelper
    {
        public ConcurrentDictionary<string, OpcSessionCacheData> OpcSessionCache = new ConcurrentDictionary<string, OpcSessionCacheData>();

        private static OpcSessionHelper _instance = null;
        private static Object _instanceLock = new Object();

        internal static string Delimiter { get; } = "__$__";

        public static OpcSessionHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_instanceLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new OpcSessionHelper();
                        }
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// Action to disconnect from the currently connected OPC UA server.
        /// </summary>
        public void Disconnect(string sessionID)
        {
            OpcSessionCacheData entry;
            if (OpcSessionCache.TryRemove(sessionID, out entry))
            {
                try
                {
                    if (entry.OPCSession != null)
                    {
                        entry.OPCSession.Close();
                    }
                }
                catch (Exception)
                {
                    // do nothing
                }
            }
        }

        public void DisconnectAll()
        {
            foreach (var entry in OpcSessionCache)
            {
                try
                {
                    if (entry.Value.OPCSession != null)
                    {
                        entry.Value.OPCSession.Close();
                    }
                }
                catch (Exception)
                {
                    // do nothing
                }
            }

            OpcSessionCache.Clear();
        }

        /// <summary>
        /// Ensures session is closed when server does not reply.
        /// </summary>
        private static void StandardClient_KeepAlive(ISession sender, KeepAliveEventArgs e)
        {
            if (e != null)
            {
                if (ServiceResult.IsBad(e.Status))
                {
                    e.CancelKeepAlive = true;

                    sender.Close();
                }
            }
        }

        /// <summary>
        /// Checks if there is an active OPC UA session for the provided browser session. If the persisted OPC UA session does not exist,
        /// a new OPC UA session to the given endpoint URL is established.
        /// </summary>
        public async Task<Session> GetSessionAsync(ApplicationConfiguration config, string sessionID, string endpointURL)
        {
            if (string.IsNullOrEmpty(endpointURL))
            {
                return null;
            }

            OpcSessionCacheData entry;
            if (OpcSessionCache.TryGetValue(sessionID, out entry))
            {
                if (entry.OPCSession != null)
                {
                    if (entry.OPCSession.Connected)
                    {
                        return entry.OPCSession;
                    }

                    try
                    {
                        entry.OPCSession.Close(500);
                        entry.OPCSession = null;
                        OpcSessionCache.TryRemove(sessionID, out entry);
                    }
                    catch (Exception e)
                    {
                        Trace.TraceError("Reason = {0}", e.Message);
                        throw;
                    }
                }
            }

            Uri endpointURI = new Uri(endpointURL);
            EndpointDescriptionCollection endpointCollection = DiscoverEndpoints(config, endpointURI, 10);
            EndpointDescription selectedEndpoint = SelectUaTcpEndpoint(endpointCollection);
            EndpointConfiguration endpointConfiguration = EndpointConfiguration.Create(config);
            ConfiguredEndpoint endpoint = new ConfiguredEndpoint(null, selectedEndpoint, endpointConfiguration);

            Session session = await Session.Create(
                    config,
                    endpoint,
                    true,
                    false,
                    string.Empty,
                    30000,
                    new UserIdentity(new AnonymousIdentityToken()),
                    null).ConfigureAwait(false);

            if (session != null)
            {
                session.KeepAlive += new KeepAliveEventHandler(StandardClient_KeepAlive);

                // Update our cache data
                OpcSessionCacheData newEntry = new OpcSessionCacheData
                {
                    EndpointURL = endpointURI,
                    OPCSession = session
                };
                OpcSessionCache.TryAdd(session.SessionId.ToString(), newEntry);
            }

            return session;
        }

        /// <summary>
        /// Uses a discovery client to discover the endpoint description of a given server
        /// </summary>
        private EndpointDescriptionCollection DiscoverEndpoints(ApplicationConfiguration config, Uri discoveryUrl, int timeout)
        {
            EndpointConfiguration configuration = EndpointConfiguration.Create(config);
            configuration.OperationTimeout = timeout;

            using (DiscoveryClient client = DiscoveryClient.Create(
                discoveryUrl,
                EndpointConfiguration.Create(config)))
            {
                try
                {
                    EndpointDescriptionCollection endpoints = client.GetEndpoints(null);
                    return ReplaceLocalHostWithRemoteHost(endpoints, discoveryUrl);
                }
                catch (Exception e)
                {
                    Trace.TraceError("Can not fetch endpoints from url: {0}", discoveryUrl);
                    Trace.TraceError("Reason = {0}", e.Message);
                    throw;
                }
            }
        }

        /// <summary>
        /// Selects the UA TCP endpoint with the highest security level
        /// </summary>
        private EndpointDescription SelectUaTcpEndpoint(EndpointDescriptionCollection endpointCollection)
        {
            EndpointDescription bestEndpoint = null;
            foreach (EndpointDescription endpoint in endpointCollection)
            {
                if (endpoint.TransportProfileUri == Profiles.UaTcpTransport)
                {
                    if ((bestEndpoint == null) ||
                        (endpoint.SecurityLevel > bestEndpoint.SecurityLevel))
                    {
                        bestEndpoint = endpoint;
                    }
                }
            }

            return bestEndpoint;
        }

        /// <summary>
        /// Replaces all instances of "LocalHost" in a collection of endpoint description with the real host name
        /// </summary>
        private EndpointDescriptionCollection ReplaceLocalHostWithRemoteHost(EndpointDescriptionCollection endpoints, Uri discoveryUrl)
        {
            EndpointDescriptionCollection updatedEndpoints = endpoints;

            foreach (EndpointDescription endpoint in updatedEndpoints)
            {
                endpoint.EndpointUrl = Utils.ReplaceLocalhost(endpoint.EndpointUrl, discoveryUrl.DnsSafeHost);

                StringCollection updatedDiscoveryUrls = new StringCollection();
                foreach (string url in endpoint.Server.DiscoveryUrls)
                {
                    updatedDiscoveryUrls.Add(Utils.ReplaceLocalhost(url, discoveryUrl.DnsSafeHost));
                }

                endpoint.Server.DiscoveryUrls = updatedDiscoveryUrls;
            }

            return updatedEndpoints;
        }
    }
}