
namespace AdminShell
{
    using Opc.Ua.Client;
    using System;

    public class OpcSessionCacheData
    {
        public bool Trusted { get; set; }

        public Session OPCSession { get; set; }

        public string CertThumbprint { get; set; }

        public Uri EndpointURL { get; set; }

        public OpcSessionCacheData()
        {
            Trusted = false;
            EndpointURL = new Uri("opc.tcp://localhost:4840");
            CertThumbprint = string.Empty;
            OPCSession = null;
        }
    }
}