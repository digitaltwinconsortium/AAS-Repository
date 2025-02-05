
using AdminShell;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Opc.Ua;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UaRestGateway.Server.Controllers
{
    [Authorize]
    [ApiController]
    public class UaServerController : ControllerBase
    {
        private readonly UAClient _client;


        public UaServerController(UAClient client)
        {
            _client = client;
        }

        [HttpPost]
        [Route("/api/ua/read")]
        public async Task<IActionResult> Read([FromBody] string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId))
            {
                // default to objects root
                nodeId = ObjectIds.RootFolder.ToString();
            }

            string value = await _client.VariableRead(nodeId).ConfigureAwait(false);

            return new ObjectResult(value);
        }

        [HttpPost]
        [Route("/api/ua/browse")]
        public async Task<IActionResult> Browse([FromBody] string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId))
            {
                // default to objects root
                nodeId = ObjectIds.RootFolder.ToString();
            }

            List<NodesetViewerNode> nodes = await _client.GetChildren(nodeId).ConfigureAwait(false);

            return new ObjectResult(nodes);
        }
    }
}