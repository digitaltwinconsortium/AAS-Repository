
namespace AdminShell
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
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
    public class AASXFileServerApiController : ControllerBase
    {
        private readonly ILogger _logger;
        private AASXPackageService _packageService;

        public AASXFileServerApiController(ILoggerFactory logger, AASXPackageService packageService)
        {
            _logger = logger.CreateLogger("AASXFileServerApiController");
            _packageService= packageService;
        }

        /// <summary>
        /// Deletes a specific AASX package from the server
        /// </summary>
        /// <param name="packageId">The Package Id (UTF8-BASE64-URL-encoded)</param>
        /// <response code="204">Deleted successfully</response>
        /// <response code="404">Not Found</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpDelete]
        [Route("/packages/{packageId}")]
        [SwaggerOperation("DeleteAASXByPackageId")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult DeleteAASXByPackageId([FromRoute][Required] string packageId)
        {
            if (_packageService.Packages.ContainsKey(Encoding.UTF8.GetString(Convert.FromBase64String(packageId))))
            {
                _packageService.Delete(packageId);

                VisualTreeBuilderService.SignalNewData(TreeUpdateMode.RebuildAndCollapse);
            }
            else
            {
                throw new Exception($"Package with packageId {packageId} not found.");
            }

            return NoContent();
        }

        /// <summary>
        /// Returns a specific AASX package from the server
        /// </summary>
        /// <param name="packageId">The package Id (UTF8-BASE64-URL-encoded)</param>
        /// <response code="200">Requested AASX package</response>
        /// <response code="404">Not Found</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/packages/{packageId}")]
        [SwaggerOperation("GetAASXByPackageId")]
        [SwaggerResponse(statusCode: 200, type: typeof(byte[]), description: "Requested AASX package")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetAASXByPackageId([FromRoute][Required] string packageId)
        {
            Stream stream = _packageService.GetPackageStream(Encoding.UTF8.GetString(Convert.FromBase64String(packageId)));
            if (stream != null)
            {
                string fileName = _packageService.GetAASXFileName(Encoding.UTF8.GetString(Convert.FromBase64String(packageId)));

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
                throw new Exception($"Package with packageId {packageId} not found.");
            }
        }

        /// <summary>
        /// Returns a list of available AASX packages at the server
        /// </summary>
        /// <param name="aasId">The Asset Administration Shell’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <response code="200">Requested package list</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/packages")]
        [SwaggerOperation("GetAllAASXPackageIds")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<PackageDescription>), description: "Requested package list")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetAllAASXPackageIds([FromQuery] string aasId)
        {
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

            //Filter on aasId
            if (output.Any())
            {
                if (!string.IsNullOrEmpty(Encoding.UTF8.GetString(Convert.FromBase64String(aasId))))
                {
                    output = output.Where(x => x.AasIds.Contains(Encoding.UTF8.GetString(Convert.FromBase64String(aasId)))).ToList();
                }
            }

            return new ObjectResult(output);
        }

        /// <summary>
        /// Creates an AASX package at the server
        /// </summary>
        /// <param name="fileName">Filename of the AASX package</param>
        /// <param name="aasIds">Included AAS Ids</param>
        /// <param name="file">AASX PAckage</param>
        /// <returns></returns>
        [HttpPost]
        [Route("/packages")]
        [SwaggerOperation("PostAASXPackage")]
        public virtual IActionResult PostAASXPackage([FromForm] string aasIds, [FromForm] IFormFile file, [FromForm] string fileName)
        {
            var stream = new MemoryStream();
            file.CopyTo(stream);

            // check if AASX already exists
            if (_packageService.Packages.ContainsKey(fileName))
            {
                throw new Exception($"File already exists");
            }

            _packageService.Save(fileName, stream.ToArray());

            VisualTreeBuilderService.SignalNewData(TreeUpdateMode.RebuildAndCollapse);

            var packageId = _packageService.GetPackageID(fileName);

            return CreatedAtAction(nameof(PostAASXPackage), packageId);
        }

        /// <summary>
        /// Updates the AASX package at the server
        /// </summary>
        /// <param name="packageId">Package ID from the package list (BASE64-URL-encoded)</param>
        /// <param name="fileName">AASX Package Name</param>
        /// <param name="file">AASX Package</param>
        /// <param name="aasIds">Included AAS Identifiers</param>
        /// <returns></returns>
        [HttpPut]
        [Route("/packages/{packageId}")]
        [SwaggerOperation("PutAASXPackageById")]
        public virtual IActionResult PutAASXPackageById([FromRoute][Required] string packageId, [FromForm] string fileName, [FromForm] IFormFile file, [FromForm] string aasIds)
        {
            var stream = new MemoryStream();
            file.CopyTo(stream);

            // check if AASX already exists
            if (!_packageService.Packages.ContainsKey(Encoding.UTF8.GetString(Convert.FromBase64String(packageId))))
            {
                throw new Exception($"Requested package with packageId {packageId} not found.");
            }

            _packageService.Packages.Remove(Encoding.UTF8.GetString(Convert.FromBase64String(packageId)));
            _packageService.Save(fileName, stream.ToArray());

            VisualTreeBuilderService.SignalNewData(TreeUpdateMode.RebuildAndCollapse);

            return NoContent();
        }
    }
}
