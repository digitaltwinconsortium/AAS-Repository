
namespace AdminShell
{
    using Opc.Ua;
    using Opc.Ua.Server;
    using System.Collections.Generic;

    public class SimpleServer : StandardServer
    {
        protected override MasterNodeManager CreateMasterNodeManager(IServerInternal server, ApplicationConfiguration configuration)
        {
            List<INodeManager> nodeManagers = new()
            {
                new NodesetFileNodeManager(server, configuration)
            };

            return new MasterNodeManager(server, configuration, null, nodeManagers.ToArray());
        }

        protected override ServerProperties LoadServerProperties()
        {
            ServerProperties properties = new ServerProperties
            {
                ManufacturerName = "Digital Twin Consortium",
                ProductName = "AAS-Repository",
                ProductUri = "",
                SoftwareVersion = Utils.GetAssemblySoftwareVersion(),
                BuildNumber = Utils.GetAssemblyBuildNumber(),
                BuildDate = Utils.GetAssemblyTimestamp()
            };

            return properties;
        }
    }
}
