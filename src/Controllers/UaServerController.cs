using AdminShell;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Opc.Ua;
using System;
using System.Threading.Tasks;
using StatusCodes = Opc.Ua.StatusCodes;

namespace UaRestGateway.Server.Controllers
{
    [ApiController]
    public class UaServerController : CommonController
    {
        public UaServerController(ILogger<UaServerController> logger, UAClient client)
        : base(logger, client)
        {
        }

        private bool IsRequestCompressed()
        {
            if (Request.Headers.TryGetValue("Content-Encoding", out StringValues header))
            {
                string token = string.Join(" ", header.ToArray()).Trim();

                if (token.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private async Task<T> Decode<T>(IServiceMessageContext context) where T : IServiceRequest, new()
        {
            return (T)await MessageUtils.Decode<T>(context, Request.Body, compressed: IsRequestCompressed(), envelopeExpected: false);
        }

        private async Task<IActionResult> Encode<T>(IServiceMessageContext context, T response) where T : IEncodeable
        {
            var compressed = IsRequestCompressed();

            var stream = await MessageUtils.Encode<T>(context, response, compressed);

            if (compressed)
            {
                Response.Headers.Append("Content-Encoding", "gzip");
            }

            return File(stream, "application/json");
        }

        private async Task<IActionResult> Fault(Exception e)
        {
            _logger.LogWarning(e, "Fault calling Service.");

            IServiceMessageContext context = _server.MessageContext ?? ServiceMessageContext.GlobalContext;

            var sr = new ServiceResult(e, StatusCodes.BadUnexpectedError);

            ServiceFault fault = new ServiceFault()
            {
                ResponseHeader = new ResponseHeader()
                {
                    Timestamp = DateTime.UtcNow,
                    ServiceResult = sr.StatusCode,
                    StringTable = new StringCollection(new string[] { sr.ToString() }),
                    ServiceDiagnostics = new DiagnosticInfo() { LocalizedText = 0 }
                }
            };

            var stream = await MessageUtils.Encode<ServiceFault>(context, fault, false);

            return File(stream, "application/json");
        }

        [HttpPost]
        [Route("read")]
        public async Task<IActionResult> Read()
        {
            try
            {
                var context = GetSessionContext(HttpContext);
                var request = await Decode<ReadRequest>(_server.MessageContext);
                var response = await MessageUtils.Read(context, _server, request);
                return await Encode(_server.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        [HttpPost]
        [Route("write")]
        public async Task<IActionResult> Write()
        {
            try
            {
                var context = GetSessionContext(HttpContext);
                var request = await Decode<WriteRequest>(_server.MessageContext);
                var response = await MessageUtils.Write(context, _server, request);
                return await Encode(_server.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        [HttpPost]
        [Route("call")]
        public async Task<IActionResult> Call()
        {
            try
            {
                var context = GetSessionContext(HttpContext);
                var request = await Decode<CallRequest>(_server.MessageContext);
                var response = await MessageUtils.Call(context, _server, request);
                return await Encode(_server.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        [HttpPost]
        [Route("browse")]
        public async Task<IActionResult> Browse()
        {
            try
            {
                var context = GetSessionContext(HttpContext);
                var request = await Decode<BrowseRequest>(_server.MessageContext);
                var response = await MessageUtils.Browse(context, _server, request);
                return await Encode(_server.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        [HttpPost]
        [Route("browsenext")]
        public async Task<IActionResult> BrowseNext()
        {
            try
            {
                var context = GetSessionContext(HttpContext);
                var request = await Decode<BrowseNextRequest>(_server.MessageContext);
                var response = await MessageUtils.BrowseNext(context, _server, request);
                return await Encode(_server.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        [HttpPost]
        [Route("translate")]
        public async Task<IActionResult> Translate()
        {
            try
            {
                var context = GetSessionContext(HttpContext);
                var request = await Decode<TranslateBrowsePathsToNodeIdsRequest>(_server.MessageContext);
                var response = await MessageUtils.TranslateBrowsePathsToNodeIds(context, _server, request);
                return await Encode(_server.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        [HttpPost]
        [Route("historyread")]
        public async Task<IActionResult> HistoryRead()
        {
            try
            {
                var context = GetSessionContext(HttpContext);
                var request = await Decode<HistoryReadRequest>(_server.MessageContext);
                var response = await MessageUtils.HistoryRead(context, _server, request);
                return await Encode(_server.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }

        [HttpPost]
        [Route("historyupdate")]
        public async Task<IActionResult> HistoryUpdate()
        {
            try
            {
                var context = GetSessionContext(HttpContext);
                var request = await Decode<HistoryUpdateRequest>(_server.MessageContext);
                var response = await MessageUtils.HistoryUpdate(context, _server, request);
                return await Encode(_server.MessageContext, response);
            }
            catch (Exception e)
            {
                return await Fault(e);
            }
        }
    }
}