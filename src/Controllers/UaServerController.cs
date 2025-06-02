
using AdminShell;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Opc.Ua;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace UaRestGateway.Server.Controllers
{
    /// <summary>
    /// REST API for OPC / UA reading and browsing nodes.)
    /// </summary>
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

        [HttpPost]
        [Route("/openapi/opcua/read")]
        public async Task<IActionResult> Read()
        {
            ReadRequest request = await Decode<ReadRequest>().ConfigureAwait(false);

            IServiceResponse response = await _client.Read(request).ConfigureAwait(false);

            return await Encode(response).ConfigureAwait(false);
        }


        [HttpPost]
        [Route("/openapi/opcua/browse")]
        public async Task<IActionResult> Browse()
        {
            BrowseRequest request = await Decode<BrowseRequest>().ConfigureAwait(false);

            IServiceResponse response = await _client.Browse(request).ConfigureAwait(false);

            return await Encode(response).ConfigureAwait(false);
        }

        private bool IsRequestCompressed()
        {
            if (Request.Headers.TryGetValue("Content-Encoding", out StringValues header))
            {
                var token = string.Join(" ", header.ToArray()).Trim();

                if (token.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private async Task<T> Decode<T>() where T : IServiceRequest, new()
        {
            return (T)await Decode<T>(Request.Body, compressed: IsRequestCompressed(), envelopeExpected: false).ConfigureAwait(false);
        }

        private async Task<IActionResult> Encode<T>(T response) where T : IEncodeable
        {
            var compressed = IsRequestCompressed();

            var stream = await Encode<T>(response, compressed).ConfigureAwait(false);

            if (compressed)
            {
                Response.Headers.Append("Content-Encoding", "gzip");
            }

            return File(stream, "application/json");
        }

        private async Task<IServiceRequest> Decode<T>(
            Stream stream,
            bool? compressed = null,
            bool envelopeExpected = false) where T : IServiceRequest
        {
            var mstrm = stream;

            if (compressed == null)
            {
                mstrm = new MemoryStream();
                await stream.CopyToAsync(mstrm).ConfigureAwait(false);
                mstrm.Position = 0;

                if (mstrm.ReadByte() == 0x1F && mstrm.ReadByte() == 0x8B)
                {
                    mstrm = await Decompress(mstrm).ConfigureAwait(false);
                }
                else
                {
                    mstrm.Position = 0;
                }
            }
            else if (compressed == true)
            {
                mstrm = await Decompress(stream).ConfigureAwait(false);
            }

            using (var reader = new StreamReader(mstrm))
            {
                var json = await reader.ReadToEndAsync().ConfigureAwait(false);

                using (var decoder = new JsonDecoder(json, ServiceMessageContext.GlobalContext))
                {
                    decoder.UpdateNamespaceTable = true;
                    var envelope = new ServiceMessageEnvelope();

                    if (envelopeExpected)
                    {
                        envelope.ServiceId = decoder.ReadExpandedNodeId(nameof(ServiceMessageEnvelope.ServiceId));
                        envelope.LocaleIds = decoder.ReadStringArray(nameof(ServiceMessageEnvelope.LocaleIds));

                        var serviceType = typeof(T);

                        envelope.Body = decoder.ReadEncodeable(nameof(ServiceMessageEnvelope.Body), serviceType);
                        return envelope.Body as IServiceRequest;
                    }

                    var request = Activator.CreateInstance(typeof(T)) as IServiceRequest;
                    request.Decode(decoder);
                    return request;
                }
            }
        }

        private async Task<MemoryStream> Encode<T>(
            T response,
            bool compress = false,
            bool envelopeRequired = false) where T : IEncodeable
        {
            var stream = new MemoryStream();

            using (var encoder = new JsonEncoder(ServiceMessageContext.GlobalContext, JsonEncodingType.Compact, stream: stream, leaveOpen: true))
            {
                if (envelopeRequired)
                {
                    encoder.WriteExpandedNodeId(nameof(ServiceMessageEnvelope.ServiceId), response.TypeId);
                    encoder.WriteEncodeable(nameof(ServiceMessageEnvelope.Body), response, null);
                }
                else
                {
                    response.Encode(encoder);
                }

                encoder.Close();
                stream.Position = 0;
            }

            if (compress)
            {
                return new MemoryStream(await Compress(stream).ConfigureAwait(false));
            }

            return stream;
        }

        private async Task<byte[]> Compress(MemoryStream istrm)
        {
            using (var ostrm = new MemoryStream())
            {
                using (var zstrm = new GZipStream(ostrm, CompressionMode.Compress))
                {
                    await istrm.CopyToAsync(zstrm).ConfigureAwait(false);
                    zstrm.Close();
                    return ostrm.ToArray();
                }
            }
        }

        private async Task<MemoryStream> Decompress(Stream istrm)
        {
            using (var zstrm = new GZipStream(istrm, CompressionMode.Decompress))
            {
                var ostrm = new MemoryStream();
                await zstrm.CopyToAsync(ostrm).ConfigureAwait(false);
                ostrm.Position = 0;
                return ostrm;
            }
        }
    }
}