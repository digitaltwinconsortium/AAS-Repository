
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdminShell
{
    /// <summary>
    /// REST API for Asset Administration Shell (Registry API)
    /// </summary>
    [Authorize]
    [ApiController]
    public class AssetAdministrationShellRegistryApiController : ControllerBase
    {
        private readonly AssetAdministrationShellEnvironmentService _aasEnvService;

        public AssetAdministrationShellRegistryApiController(AssetAdministrationShellEnvironmentService aasEnvService)
        {
            _aasEnvService = aasEnvService;
        }

        /// <summary>
        /// Returns all Asset Administration Shell Descriptors
        /// </summary>
        /// <param name="limit">The maximum number of elements in the response array</param>
        /// <param name="cursor">A server-generated identifier retrieved from pagingMetadata that specifies from which position the result listing should continue</param>
        /// <param name="assetKind">The Asset&#x27;s kind (Instance or Type)</param>
        /// <param name="assetType">The Asset&#x27;s type (UTF8-BASE64-URL-encoded)</param>
        /// <response code="200">Requested Asset Administration Shell Descriptors</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/api/v3.0/shell-descriptors")]
        [SwaggerOperation("GetAllAssetAdministrationShellDescriptors")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<AssetAdministrationShellDescriptor>), description: "Requested Asset Administration Shell Descriptors")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetAllAssetAdministrationShellDescriptors([FromQuery]int? limit, [FromQuery]string cursor, [FromQuery]AssetKind assetKind, [FromQuery][RegularExpression("/^([\\\\x09\\\\x0a\\\\x0d\\\\x20-\\\\ud7ff\\\\ue000-\\\\ufffd]|\\\\ud800[\\\\udc00-\\\\udfff]|[\\\\ud801-\\\\udbfe][\\\\udc00-\\\\udfff]|\\\\udbff[\\\\udc00-\\\\udfff])*$/")][StringLength(2048, MinimumLength=1)]string assetType)
        {
            List<AssetAdministrationShell> aasList = _aasEnvService.GetAllAssetAdministrationShells();

            var output = new List<AssetAdministrationShellDescriptor>();

            int cIncluded = 0; // Use to respect the requested "limit"

            foreach (AssetAdministrationShell myshell in aasList)
            {
                // Check if the AssetKind matches the requested one
                if (assetKind == myshell.AssetInformation.AssetKind)
                {
                    // List<ModelReference> listSubmodels = myshell.Submodels;
                    // List<Endpoint> listEp = listSubmodels[0].Endpoints;

                    AssetAdministrationShellDescriptor descriptor = new AssetAdministrationShellDescriptor
                    {
                        Administration = myshell.Administration,
                        AssetKind = myshell.AssetInformation.AssetKind,
                        /* AssetType = ssetType, *
                        /* Endpoints = ep, */
                        GlobalAssetId = myshell.AssetInformation.GlobalAssetId,
                        IdShort = myshell.IdShort,
                        Id = myshell.Id,
                        /* SpecificAsset Ids = */
                        /* SubmodelDescriptors = */
                    };
                    output.Add(descriptor);

                    cIncluded++;
                    if (limit.HasValue && cIncluded >= limit.Value)
                    {
                        break; // Stop if we reached the limit
                    }
                }
            }



            // TODO: Implement the logic to retrieve all Asset Administration Shell Descriptors based on the provided parameters.
            return new ObjectResult(output);
        }

        /// <summary>
        /// Returns a specific Asset Administration Shell Descriptor
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id (UTF8-BASE64-URL-encoded)</param>
        /// <response code="200">Requested Asset Administration Shell Descriptor</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/api/v3.0/shell-descriptors/{aasIdentifier}")]
        [SwaggerOperation("GetAssetAdministrationShellDescriptorById")]
        [SwaggerResponse(statusCode: 200, type: typeof(AssetAdministrationShellDescriptor), description: "Requested Asset Administration Shell Descriptor")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetAssetAdministrationShellDescriptorById([FromRoute][Required]string aasIdentifier)
        {
            string decodedAasIdentifier = Base64UrlEncoder.Decode(aasIdentifier);
            AssetAdministrationShell myshell = _aasEnvService.GetAssetAdministrationShellById(decodedAasIdentifier);
            AssetAdministrationShellDescriptor descriptor = new AssetAdministrationShellDescriptor
            {
                Administration = myshell.Administration,
                AssetKind = myshell.AssetInformation.AssetKind,
                /* AssetType = assetType, */
                /* Endpoints = ep, */
                GlobalAssetId = myshell.AssetInformation.GlobalAssetId,
                IdShort = myshell.IdShort,
                Id = myshell.Id,
                /* SpecificAsset Ids = */
                /* SubmodelDescriptors = */
            };

            // TODO: Implement the logic to retrieve a specific Asset Administration Shell Descriptor based on the provided aasIdentifier.
            return new ObjectResult(descriptor);
        }
    }
}
