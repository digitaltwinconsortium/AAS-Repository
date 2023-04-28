
namespace AdminShell
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Swashbuckle.AspNetCore.Annotations;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Net.Mime;
    using System.Text;

    [ApiController]
    public class AASXFileServerAPIApiController : ControllerBase
    {
        private AASXPackageService _packageService;

        public AASXFileServerAPIApiController(AASXPackageService packageService)
        {
            _packageService = packageService;
        }

        /// <summary>
        /// Deletes a specific AASX package from the server
        /// </summary>
        /// <param name="packageId">The package Id (UTF8-BASE64-URL-encoded)</param>
        /// <response code="204">Deleted successfully</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpDelete]
        [Route("/packages/{packageId}")]
        [SwaggerOperation("DeleteAASXByPackageId")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult DeleteAASXByPackageId([FromRoute][Required]byte[] packageId)
        {
            string packageIDDecoded = Encoding.UTF8.GetString(Convert.FromBase64String(Encoding.UTF8.GetString(packageId)));

            if (_packageService.Packages.ContainsKey(packageIDDecoded))
            {
                _packageService.Delete(packageIDDecoded);

                VisualTreeBuilderService.SignalNewData(TreeUpdateMode.RebuildAndCollapse);
            }
            else
            {
                throw new Exception($"Package with packageId {packageIDDecoded} not found.");
            }

            return NoContent();
        }

        /// <summary>
        /// Returns a specific AASX package from the server
        /// </summary>
        /// <param name="packageId">The package Id (UTF8-BASE64-URL-encoded)</param>
        /// <response code="200">Requested AASX package</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/packages/{packageId}")]
        [SwaggerOperation("GetAASXByPackageId")]
        [SwaggerResponse(statusCode: 200, type: typeof(byte[]), description: "Requested AASX package")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetAASXByPackageId([FromRoute][Required]byte[] packageId)
        {
            string packageIDDecoded = Encoding.UTF8.GetString(Convert.FromBase64String(Encoding.UTF8.GetString(packageId)));

            Stream stream = _packageService.GetPackageStream(packageIDDecoded);
            if (stream != null)
            {
                string fileName = _packageService.GetAASXFileName(packageIDDecoded);

                // content-disposition is used to downloaded AASX files from the web browser
                ContentDisposition contentDisposition = new()
                {
                    FileName = fileName
                };

                HttpContext.Response.Headers.Add("Content-Disposition", contentDisposition.ToString());
                HttpContext.Response.Headers.Add("X-FileName", fileName);
                HttpContext.Response.ContentLength = stream.Length;
                HttpContext.Response.Body = stream;

                return new EmptyResult();
            }
            else
            {
                throw new Exception($"Package with packageId {packageIDDecoded} not found.");
            }
        }

        /// <summary>
        /// Returns a list of available AASX packages at the server
        /// </summary>
        /// <param name="aasId">The Asset Administration Shell’s unique id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="limit">The maximum number of elements in the response array</param>
        /// <param name="cursor">A server-generated identifier retrieved from pagingMetadata that specifies from which position the result listing should continue</param>
        /// <response code="200">Requested package list</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/packages")]
        [SwaggerOperation("GetAllAASXPackageIds")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<PackageDescription>), description: "Requested package list")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetAllAASXPackageIds([FromQuery]byte[] aasId, [FromQuery]int? limit, [FromQuery]string cursor)
        {
            string aasIDDecoded = Encoding.UTF8.GetString(Convert.FromBase64String(Encoding.UTF8.GetString(aasId)));

            var output = new List<PackageDescription>();

            foreach (KeyValuePair<string, AssetAdministrationShellEnvironment> package in _packageService.Packages)
            {
                var aasIdList = new List<string>();
                foreach (var aas in package.Value.AssetAdministrationShells)
                {
                    aasIdList.Add(aas.IdShort);
                }

                PackageDescription packageDescription = new()
                {
                    PackageId = package.Key,
                    AasIds = aasIdList
                };

                output.Add(packageDescription);
            }

            // filter on aasId
            if (output.Any())
            {
                if (!string.IsNullOrEmpty(aasIDDecoded))
                {
                    output = output.Where(x => x.AasIds.Contains(aasIDDecoded)).ToList();
                }
            }

            return new ObjectResult(output);
        }
    }
}
