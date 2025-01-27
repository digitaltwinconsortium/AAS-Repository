using AdminShell;
using Microsoft.AspNetCore.Mvc;
using Opc.Ua;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UaRestGateway.Server.Controllers
{
    [ApiController]
    public class UaServerController : ControllerBase
    {
        private readonly UAClient _client;


        public UaServerController(UAClient client)
        {
            _client = client;
        }

        [HttpPost]
        [Route("read")]
        public async Task<IActionResult> Read([FromBody] string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId))
            {
                // default to objects root
                nodeId = ObjectIds.RootFolder.ToString();
            }

            string value = await _client.VariableRead(nodeId, string.Empty).ConfigureAwait(false);

            return new ObjectResult(value);
        }

        [HttpPost]
        [Route("browse")]
        public async Task<IActionResult> Browse([FromBody] string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId))
            {
                // default to objects root
                nodeId = ObjectIds.RootFolder.ToString();
            }

            List<NodesetViewerNode> nodes = await _client.GetChildren(nodeId, string.Empty).ConfigureAwait(false);

            // remove session ID
            if (nodes != null)
            {
                foreach (NodesetViewerNode node in nodes)
                {
                    node.SessionId = string.Empty;
                }
            }

            return new ObjectResult(nodes);
        }
    }
}