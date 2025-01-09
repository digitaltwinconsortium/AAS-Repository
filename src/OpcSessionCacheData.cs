
namespace AdminShell
{
    using Opc.Ua.Client;
    using System;

    public class OpcSessionCacheData
    {
        public Session OPCSession { get; set; }

        public Uri EndpointURL { get; set; }

        public OpcSessionCacheData()
        {
            EndpointURL = new Uri("opc.tcp://localhost/");
            OPCSession = null;
        }
    }
}