
namespace AdminShell
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Swashbuckle.AspNetCore.Annotations;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Net.Mime;
    using System.Web;

    [ApiController]
    public class AssetAdministrationShellRepositoryAPIApiController : ControllerBase
    {
        private readonly AssetAdministrationShellEnvironmentService _aasEnvService;
        private readonly AASXPackageService _packageService;

        public AssetAdministrationShellRepositoryAPIApiController(AssetAdministrationShellEnvironmentService aasEnvService, AASXPackageService packageService)
        {
            _aasEnvService = aasEnvService;
            _packageService = packageService;
        }

        /// <summary>
        /// Deletes an Asset Administration Shell
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <response code="204">Asset Administration Shell deleted successfully</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpDelete]
        [Route("/shells/{aasIdentifier}")]
        [SwaggerOperation("DeleteAssetAdministrationShellById")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult DeleteAssetAdministrationShellById([FromRoute][Required]string aasIdentifier)
        {
            _aasEnvService.DeleteAssetAdministrationShellById(aasIdentifier);

            return NoContent();
        }

        /// <summary>
        /// Deletes file content of an existing submodel element at a specified path within submodel elements hierarchy
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated)</param>
        /// <response code="200">Submodel element updated successfully</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpDelete]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}/submodel-elements/{idShortPath}/attachment")]
        [SwaggerOperation("DeleteFileByPathAasRepository")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult DeleteFileByPathAasRepository([FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromRoute][Required]string idShortPath)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes the submodel from the Asset Administration Shell and the Repository.
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <response code="204">Submodel deleted successfully</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpDelete]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}/$metadata")]
        [SwaggerOperation("DeleteSubmodelByIdAasRepository")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult DeleteSubmodelByIdAasRepository([FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier)
        {
            _aasEnvService.DeleteSubmodelById(submodelIdentifier);

            return NoContent();
        }

        /// <summary>
        /// Deletes a submodel element at a specified path within the submodel elements hierarchy
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated)</param>
        /// <response code="204">Submodel element deleted successfully</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpDelete]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}/submodel-elements/{idShortPath}")]
        [SwaggerOperation("DeleteSubmodelElementByPathAasRepository")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult DeleteSubmodelElementByPathAasRepository([FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromRoute][Required]string idShortPath)
        {
            _aasEnvService.DeleteSubmodelElementByPath(aasIdentifier, submodelIdentifier, idShortPath);

            return NoContent();
        }

        /// <summary>
        /// Deletes the submodel reference from the Asset Administration Shell. Does not delete the submodel itself!
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <response code="204">Submodel reference deleted successfully</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpDelete]
        [Route("/shells/{aasIdentifier}/submodel-refs/{submodelIdentifier}")]
        [SwaggerOperation("DeleteSubmodelReferenceByIdAasRepository")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult DeleteSubmodelReferenceByIdAasRepository([FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier)
        {
            _aasEnvService.DeleteSubmodelReferenceById(aasIdentifier, submodelIdentifier);

            return NoContent();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <response code="200">Thumbnail deletion successful</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpDelete]
        [Route("/shells/{aasIdentifier}/asset-information/thumbnail")]
        [SwaggerOperation("DeleteThumbnailAasRepository")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult DeleteThumbnailAasRepository([FromRoute][Required]string aasIdentifier)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns all Asset Administration Shells
        /// </summary>
        /// <param name="assetIds">A list of specific Asset identifiers</param>
        /// <param name="idShort">The Asset Administration Shell’s IdShort</param>
        /// <param name="limit">The maximum number of elements in the response array</param>
        /// <param name="cursor">A server-generated identifier retrieved from pagingMetadata that specifies from which position the result listing should continue</param>
        /// <response code="200">Requested Asset Administration Shells</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells")]
        [SwaggerOperation("GetAllAssetAdministrationShells")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<AssetAdministrationShell>), description: "Requested Asset Administration Shells")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetAllAssetAdministrationShells([FromQuery]List<string> assetIds, [FromQuery]string idShort, [FromQuery]int? limit, [FromQuery]string cursor)
        {
            var output = _aasEnvService.GetAllAssetAdministrationShells(assetIds, idShort);

            return new ObjectResult(output);
        }

        /// <summary>
        /// Returns References to all Asset Administration Shells
        /// </summary>
        /// <param name="assetIds">A list of specific Asset identifiers</param>
        /// <param name="idShort">The Asset Administration Shell’s IdShort</param>
        /// <param name="limit">The maximum number of elements in the response array</param>
        /// <param name="cursor">A server-generated identifier retrieved from pagingMetadata that specifies from which position the result listing should continue</param>
        /// <response code="200">Requested Asset Administration Shells as a list of References</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells/$reference")]
        [SwaggerOperation("GetAllAssetAdministrationShellsReference")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<Reference>), description: "Requested Asset Administration Shells as a list of References")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetAllAssetAdministrationShellsReference([FromQuery]List<string> assetIds, [FromQuery]string idShort, [FromQuery]int? limit, [FromQuery]string cursor)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns all submodel elements including their hierarchy
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="limit">The maximum number of elements in the response array</param>
        /// <param name="cursor">A server-generated identifier retrieved from pagingMetadata that specifies from which position the result listing should continue</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <param name="extent">Determines to which extent the resource is being serialized</param>
        /// <response code="200">List of found submodel elements</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}/submodel-elements")]
        [SwaggerOperation("GetAllSubmodelElementsAasRepository")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<SubmodelElement>), description: "List of found submodel elements")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetAllSubmodelElementsAasRepository([FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromQuery]int? limit, [FromQuery]string cursor, [FromQuery]string level, [FromQuery]string extent)
        {
            var output = _aasEnvService.GetAllSubmodelElements(HttpUtility.UrlDecode(aasIdentifier), HttpUtility.UrlDecode(submodelIdentifier));

            return new ObjectResult(output);
        }

        /// <summary>
        /// Returns all submodel elements including their hierarchy
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="limit">The maximum number of elements in the response array</param>
        /// <param name="cursor">A server-generated identifier retrieved from pagingMetadata that specifies from which position the result listing should continue</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <response code="200">List of found submodel elements</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}/submodel-elements/$metadata")]
        [SwaggerOperation("GetAllSubmodelElementsMetadataAasRepository")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<SubmodelElement>), description: "List of found submodel elements")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetAllSubmodelElementsMetadataAasRepository([FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromQuery]int? limit, [FromQuery]string cursor, [FromQuery]string level)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns all submodel elements including their hierarchy
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="limit">The maximum number of elements in the response array</param>
        /// <param name="cursor">A server-generated identifier retrieved from pagingMetadata that specifies from which position the result listing should continue</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <param name="extent">Determines to which extent the resource is being serialized</param>
        /// <response code="200">List of found submodel elements in the Path notation</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}/submodel-elements/$path")]
        [SwaggerOperation("GetAllSubmodelElementsPathAasRepository")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<string>), description: "List of found submodel elements in the Path notation")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetAllSubmodelElementsPathAasRepository([FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromQuery]int? limit, [FromQuery]string cursor, [FromQuery]string level, [FromQuery]string extent)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns all submodel elements as a list of References
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="limit">The maximum number of elements in the response array</param>
        /// <param name="cursor">A server-generated identifier retrieved from pagingMetadata that specifies from which position the result listing should continue</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <response code="200">List of References of the found submodel elements</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}/submodel-elements/$reference")]
        [SwaggerOperation("GetAllSubmodelElementsReferenceAasRepository")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<Reference>), description: "List of References of the found submodel elements")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetAllSubmodelElementsReferenceAasRepository([FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromQuery]int? limit, [FromQuery]string cursor, [FromQuery]string level)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns all submodel elements including their hierarchy in the ValueOnly representation
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="limit">The maximum number of elements in the response array</param>
        /// <param name="cursor">A server-generated identifier retrieved from pagingMetadata that specifies from which position the result listing should continue</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <response code="200">List of found submodel elements in their ValueOnly representation</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}/submodel-elements/$value")]
        [SwaggerOperation("GetAllSubmodelElementsValueOnlyAasRepository")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<SubmodelElement>), description: "List of found submodel elements in their ValueOnly representation")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetAllSubmodelElementsValueOnlyAasRepository([FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromQuery]int? limit, [FromQuery]string cursor, [FromQuery]string level)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns all submodel references
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="limit">The maximum number of elements in the response array</param>
        /// <param name="cursor">A server-generated identifier retrieved from pagingMetadata that specifies from which position the result listing should continue</param>
        /// <response code="200">Requested submodel references</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells/{aasIdentifier}/submodel-refs")]
        [SwaggerOperation("GetAllSubmodelReferencesAasRepository")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<Reference>), description: "Requested submodel references")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetAllSubmodelReferencesAasRepository([FromRoute][Required]string aasIdentifier, [FromQuery]int? limit, [FromQuery]string cursor)
        {
            var output = _aasEnvService.GetAllSubmodelReferences(aasIdentifier);

            return new ObjectResult(output);
        }

        /// <summary>
        /// Returns a specific Asset Administration Shell
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <response code="200">Requested Asset Administration Shell</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells/{aasIdentifier}")]
        [SwaggerOperation("GetAssetAdministrationShellById")]
        [SwaggerResponse(statusCode: 200, type: typeof(AssetAdministrationShell), description: "Requested Asset Administration Shell")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetAssetAdministrationShellById([FromRoute][Required]string aasIdentifier)
        {
            var output = _aasEnvService.GetAssetAdministrationShellById(aasIdentifier, out _);

            return new ObjectResult(output);
        }

        /// <summary>
        /// Returns a specific Asset Administration Shell as a Reference
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <response code="200">Requested Asset Administration Shell</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells/{aasIdentifier}/$reference")]
        [SwaggerOperation("GetAssetAdministrationShellByIdReferenceAasRepository")]
        [SwaggerResponse(statusCode: 200, type: typeof(Reference), description: "Requested Asset Administration Shell")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetAssetAdministrationShellByIdReferenceAasRepository([FromRoute][Required]string aasIdentifier)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the Asset Information
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <response code="200">Requested Asset Information</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells/{aasIdentifier}/asset-information")]
        [SwaggerOperation("GetAssetInformationAasRepository")]
        [SwaggerResponse(statusCode: 200, type: typeof(AssetInformation), description: "Requested Asset Information")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetAssetInformationAasRepository([FromRoute][Required]string aasIdentifier)
        {
            var output = _aasEnvService.GetAssetInformationFromAas(aasIdentifier);

            return new ObjectResult(output);
        }

        /// <summary>
        /// Downloads file content from a specific submodel element from the Submodel at a specified path
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated)</param>
        /// <response code="200">Requested file</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}/submodel-elements/{idShortPath}/attachment")]
        [SwaggerOperation("GetFileByPathAasRepository")]
        [SwaggerResponse(statusCode: 200, type: typeof(byte[]), description: "Requested file")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetFileByPathAasRepository([FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromRoute][Required]string idShortPath)
        {
            var fileName = _aasEnvService.GetFileByPath(HttpUtility.UrlDecode(aasIdentifier), HttpUtility.UrlDecode(submodelIdentifier), HttpUtility.UrlDecode(idShortPath), out byte[] content, out long fileSize);

            //content-disposition so that the aasx file can be doenloaded from the web browser.
            ContentDisposition contentDisposition = new()
            {
                FileName = fileName
            };

            HttpContext.Response.Headers.Add("Content-Disposition", contentDisposition.ToString());
            HttpContext.Response.ContentLength = fileSize;
            HttpContext.Response.Body.WriteAsync(content);

            return File(content, "APPLICATION/octet-stream", fileName);
        }

        /// <summary>
        /// Returns the Operation result of an asynchronous invoked Operation
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated)</param>
        /// <param name="handleId">The returned handle id of an operation’s asynchronous invocation used to request the current state of the operation’s execution</param>
        /// <response code="200">Operation result object</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}/submodel-elements/{idShortPath}/operation-results/{handleId}")]
        [SwaggerOperation("GetOperationAsyncResultAasRepository")]
        [SwaggerResponse(statusCode: 200, type: typeof(OperationResult), description: "Operation result object")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetOperationAsyncResultAasRepository([FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromRoute][Required]string idShortPath, [FromRoute][Required]string handleId)
        {
            var output = _aasEnvService.GetOperationAsyncResult(aasIdentifier, submodelIdentifier, idShortPath, handleId);

            return new ObjectResult(output);
        }

        /// <summary>
        /// Returns the ValueOnly notation of the Operation result of an asynchronous invoked Operation
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated)</param>
        /// <param name="handleId">The returned handle id of an operation’s asynchronous invocation used to request the current state of the operation’s execution</param>
        /// <response code="200">Operation result object</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}/submodel-elements/{idShortPath}/operation-results/{handleId}/$value")]
        [SwaggerOperation("GetOperationAsyncResultValueOnlyAasRepository")]
        [SwaggerResponse(statusCode: 200, type: typeof(OperationResult), description: "Operation result object")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetOperationAsyncResultValueOnlyAasRepository([FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromRoute][Required]string idShortPath, [FromRoute][Required]string handleId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the Operation status of an asynchronous invoked Operation
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated)</param>
        /// <param name="handleId">The returned handle id of an operation’s asynchronous invocation used to request the current state of the operation’s execution</param>
        /// <response code="200">Operation result object containing information that the &#x27;executionState&#x27; is still &#x27;Running&#x27;</response>
        /// <response code="302">The target resource is available but at a different location.</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}/submodel-elements/{idShortPath}/operation-status/{handleId}")]
        [SwaggerOperation("GetOperationAsyncStatusAasRepository")]
        [SwaggerResponse(statusCode: 200, type: typeof(OperationResult), description: "Operation result object containing information that the &#x27;executionState&#x27; is still &#x27;Running&#x27;")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetOperationAsyncStatusAasRepository([FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromRoute][Required]string idShortPath, [FromRoute][Required]string handleId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the Submodel
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <param name="extent">Determines to which extent the resource is being serialized</param>
        /// <response code="200">Requested Submodel</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}")]
        [SwaggerOperation("GetSubmodelByIdAasRepository")]
        [SwaggerResponse(statusCode: 200, type: typeof(Submodel), description: "Requested Submodel")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetSubmodelByIdAasRepository([FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromQuery]string level, [FromQuery]string extent)
        {
            var output = _aasEnvService.GetSubmodelById(submodelIdentifier, out _);

            return new ObjectResult(output);
        }

        /// <summary>
        /// Returns the Submodel&#x27;s metadata elements
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <response code="200">Requested Submodel</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}/$metadata")]
        [SwaggerOperation("GetSubmodelByIdMetadataAasRepository")]
        [SwaggerResponse(statusCode: 200, type: typeof(Submodel), description: "Requested Submodel")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetSubmodelByIdMetadataAasRepository([FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromQuery]string level)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the Submodel&#x27;s metadata elements
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <response code="200">Requested Submodel in Path notation</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}/$path")]
        [SwaggerOperation("GetSubmodelByIdPathAasRepository")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<string>), description: "Requested Submodel in Path notation")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetSubmodelByIdPathAasRepository([FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromQuery]string level)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the Submodel as a Reference
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <response code="200">Requested Submodel as a Reference</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}/$reference")]
        [SwaggerOperation("GetSubmodelByIdReferenceAasRepository")]
        [SwaggerResponse(statusCode: 200, type: typeof(Reference), description: "Requested Submodel as a Reference")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetSubmodelByIdReferenceAasRepository([FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromQuery]string level)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the Submodel&#x27;s ValueOnly representation
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <param name="extent">Determines to which extent the resource is being serialized</param>
        /// <response code="200">Requested Submodel</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}/$value")]
        [SwaggerOperation("GetSubmodelByIdValueOnlyAasRepository")]
        [SwaggerResponse(statusCode: 200, type: typeof(Submodel), description: "Requested Submodel")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetSubmodelByIdValueOnlyAasRepository([FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromQuery]string level, [FromQuery]string extent)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a specific submodel element from the Submodel at a specified path
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated)</param>
        /// <param name="limit">The maximum number of elements in the response array</param>
        /// <param name="cursor">A server-generated identifier retrieved from pagingMetadata that specifies from which position the result listing should continue</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <param name="extent">Determines to which extent the resource is being serialized</param>
        /// <response code="200">Requested submodel element</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}/submodel-elements/{idShortPath}")]
        [SwaggerOperation("GetSubmodelElementByPathAasRepository")]
        [SwaggerResponse(statusCode: 200, type: typeof(SubmodelElement), description: "Requested submodel element")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetSubmodelElementByPathAasRepository([FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromRoute][Required]string idShortPath, [FromQuery]int? limit, [FromQuery]string cursor, [FromQuery]string level, [FromQuery]string extent)
        {
            var output = _aasEnvService.GetSubmodelElementByPath(aasIdentifier, submodelIdentifier, idShortPath);

            return new ObjectResult(output);
        }

        /// <summary>
        /// Returns the metadata attributes if a specific submodel element from the Submodel at a specified path
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated)</param>
        /// <param name="limit">The maximum number of elements in the response array</param>
        /// <param name="cursor">A server-generated identifier retrieved from pagingMetadata that specifies from which position the result listing should continue</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <response code="200">Requested submodel element</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}/submodel-elements/{idShortPath}/$metadata")]
        [SwaggerOperation("GetSubmodelElementByPathMetadataAasRepository")]
        [SwaggerResponse(statusCode: 200, type: typeof(SubmodelElement), description: "Requested submodel element")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetSubmodelElementByPathMetadataAasRepository([FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromRoute][Required]string idShortPath, [FromQuery]int? limit, [FromQuery]string cursor, [FromQuery]string level)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a specific submodel element from the Submodel at a specified path in the Path notation
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated)</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <response code="200">Requested submodel element in the Path notation</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}/submodel-elements/{idShortPath}/$path")]
        [SwaggerOperation("GetSubmodelElementByPathPathAasRepository")]
        [SwaggerResponse(statusCode: 200, type: typeof(string), description: "Requested submodel element in the Path notation")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetSubmodelElementByPathPathAasRepository([FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromRoute][Required]string idShortPath, [FromQuery]string level)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the Reference of a specific submodel element from the Submodel at a specified path
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated)</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <response code="200">Requested submodel element in its ValueOnly representation</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}/submodel-elements/{idShortPath}/$reference")]
        [SwaggerOperation("GetSubmodelElementByPathReferenceAasRepository")]
        [SwaggerResponse(statusCode: 200, type: typeof(Reference), description: "Requested submodel element in its ValueOnly representation")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetSubmodelElementByPathReferenceAasRepository([FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromRoute][Required]string idShortPath, [FromQuery]string level)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a specific submodel element from the Submodel at a specified path in the ValueOnly representation
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated)</param>
        /// <param name="limit">The maximum number of elements in the response array</param>
        /// <param name="cursor">A server-generated identifier retrieved from pagingMetadata that specifies from which position the result listing should continue</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <param name="extent">Determines to which extent the resource is being serialized</param>
        /// <response code="200">Requested submodel element in its ValueOnly representation</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}/submodel-elements/{idShortPath}/$value")]
        [SwaggerOperation("GetSubmodelElementByPathValueOnlyAasRepository")]
        [SwaggerResponse(statusCode: 200, type: typeof(SubmodelElement), description: "Requested submodel element in its ValueOnly representation")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetSubmodelElementByPathValueOnlyAasRepository([FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromRoute][Required]string idShortPath, [FromQuery]int? limit, [FromQuery]string cursor, [FromQuery]string level, [FromQuery]string extent)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <response code="200">The thumbnail of the Asset Information.</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells/{aasIdentifier}/asset-information/thumbnail")]
        [SwaggerOperation("GetThumbnailAasRepository")]
        [SwaggerResponse(statusCode: 200, type: typeof(byte[]), description: "The thumbnail of the Asset Information.")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetThumbnailAasRepository([FromRoute][Required]string aasIdentifier)
        {
            byte[] content = _packageService.GetThumbnailFromPackage(_packageService.GetAASXFileNameFromAASIndentifier(HttpUtility.UrlDecode(aasIdentifier)), out string filename);
            return File(content, "APPLICATION/octet-stream", filename);
        }

        /// <summary>
        /// Synchronously invokes an Operation at a specified path
        /// </summary>
        /// <param name="body">Operation request object</param>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated)</param>
        /// <response code="200">Operation result object</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPost]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}/submodel-elements/{idShortPath}/invoke")]
        [SwaggerOperation("InvokeOperationAasRepository")]
        [SwaggerResponse(statusCode: 200, type: typeof(OperationResult), description: "Operation result object")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult InvokeOperationAasRepository([FromBody]OperationRequest body, [FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromRoute][Required]string idShortPath)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Asynchronously invokes an Operation at a specified path
        /// </summary>
        /// <param name="body">Operation request object</param>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated)</param>
        /// <response code="202">The server has accepted the request.</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPost]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}/submodel-elements/{idShortPath}/invoke-async")]
        [SwaggerOperation("InvokeOperationAsyncAasRepository")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult InvokeOperationAsyncAasRepository([FromBody]OperationRequest body, [FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromRoute][Required]string idShortPath)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Asynchronously invokes an Operation at a specified path
        /// </summary>
        /// <param name="body">Operation request object</param>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated)</param>
        /// <response code="202">The server has accepted the request.</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPost]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}/submodel-elements/{idShortPath}/invoke-async/$value")]
        [SwaggerOperation("InvokeOperationAsyncValueOnlyAasRepository")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult InvokeOperationAsyncValueOnlyAasRepository([FromBody]OperationRequest body, [FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromRoute][Required]string idShortPath)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Synchronously invokes an Operation at a specified path
        /// </summary>
        /// <param name="body">Operation request object</param>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated)</param>
        /// <response code="200">Operation result object</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPost]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}/submodel-elements/{idShortPath}/invoke/$value")]
        [SwaggerOperation("InvokeOperationValueOnlyAasRepository")]
        [SwaggerResponse(statusCode: 200, type: typeof(OperationResult), description: "Operation result object")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult InvokeOperationValueOnlyAasRepository([FromBody]OperationRequest body, [FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromRoute][Required]string idShortPath)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the Submodel
        /// </summary>
        /// <param name="body">Submodel object</param>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <response code="204">Submodel updated successfully</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPatch]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}")]
        [SwaggerOperation("PatchSubmodelAasRepository")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult PatchSubmodelAasRepository([FromBody]Submodel body, [FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromQuery]string level)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the metadata attributes of the Submodel
        /// </summary>
        /// <param name="body">Submodel object</param>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <response code="204">Submodel updated successfully</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPatch]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}/$metadata")]
        [SwaggerOperation("PatchSubmodelByIdMetadataAasRepository")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult PatchSubmodelByIdMetadataAasRepository([FromBody]Submodel body, [FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromQuery]string level)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates teh values of the Submodel
        /// </summary>
        /// <param name="body">Submodel object in the ValueOnly representation</param>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <response code="204">Submodel updated successfully</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPatch]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}/$value")]
        [SwaggerOperation("PatchSubmodelByIdValueOnlyAasRepository")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult PatchSubmodelByIdValueOnlyAasRepository([FromBody]Submodel body, [FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromQuery]string level)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates an existing submodel element value at a specified path within submodel elements hierarchy
        /// </summary>
        /// <param name="body">The updated value of the submodel element</param>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated)</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <response code="204">Submodel element updated successfully</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPatch]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}/submodel-elements/{idShortPath}")]
        [SwaggerOperation("PatchSubmodelElementValueByPathAasRepository")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult PatchSubmodelElementValueByPathAasRepository([FromBody]SubmodelElement body, [FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromRoute][Required]string idShortPath, [FromQuery]string level)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the metadata attributes of an existing submodel element value at a specified path within submodel elements hierarchy
        /// </summary>
        /// <param name="body">The updated metadata attributes of the submodel element</param>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated)</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <response code="204">Submodel element updated successfully</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPatch]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}/submodel-elements/{idShortPath}/$metadata")]
        [SwaggerOperation("PatchSubmodelElementValueByPathMetadata")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult PatchSubmodelElementValueByPathMetadata([FromBody]SubmodelElement body, [FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromRoute][Required]string idShortPath, [FromQuery]string level)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the value of an existing submodel element value at a specified path within submodel elements hierarchy
        /// </summary>
        /// <param name="body">The updated value of the submodel element</param>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated)</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <response code="204">Submodel element updated successfully</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPatch]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}/submodel-elements/{idShortPath}/$value")]
        [SwaggerOperation("PatchSubmodelElementValueByPathValueOnly")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult PatchSubmodelElementValueByPathValueOnly([FromBody]SubmodelElement body, [FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromRoute][Required]string idShortPath, [FromQuery]string level)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new Asset Administration Shell
        /// </summary>
        /// <param name="body">Asset Administration Shell object</param>
        /// <response code="201">Asset Administration Shell created successfully</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="409">Conflict, a resource which shall be created exists already. Might be thrown if a Submodel or SubmodelElement with the same ShortId is contained in a POST request.</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPost]
        [Route("/shells")]
        [SwaggerOperation("PostAssetAdministrationShell")]
        [SwaggerResponse(statusCode: 201, type: typeof(AssetAdministrationShell), description: "Asset Administration Shell created successfully")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 409, type: typeof(Result), description: "Conflict, a resource which shall be created exists already. Might be thrown if a Submodel or SubmodelElement with the same ShortId is contained in a POST request.")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult PostAssetAdministrationShell([FromBody]AssetAdministrationShell body)
        {
            var output = _aasEnvService.CreateAssetAdministrationShell(body);

            return CreatedAtAction(nameof(PostAssetAdministrationShell), output);
        }

        /// <summary>
        /// Creates a new submodel element
        /// </summary>
        /// <param name="body">Requested submodel element</param>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <param name="extent">Determines to which extent the resource is being serialized</param>
        /// <response code="201">Submodel element created successfully</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="409">Conflict, a resource which shall be created exists already. Might be thrown if a Submodel or SubmodelElement with the same ShortId is contained in a POST request.</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPost]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}/submodel-elements")]
        [SwaggerOperation("PostSubmodelElementAasRepository")]
        [SwaggerResponse(statusCode: 201, type: typeof(SubmodelElement), description: "Submodel element created successfully")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 409, type: typeof(Result), description: "Conflict, a resource which shall be created exists already. Might be thrown if a Submodel or SubmodelElement with the same ShortId is contained in a POST request.")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult PostSubmodelElementAasRepository([FromBody]SubmodelElement body, [FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromQuery]string level, [FromQuery]string extent)
        {
            var output = _aasEnvService.CreateSubmodelElement(body, aasIdentifier, submodelIdentifier);

            return CreatedAtAction(nameof(PostSubmodelElementAasRepository), output);
        }

        /// <summary>
        /// Creates a new submodel element at a specified path within submodel elements hierarchy
        /// </summary>
        /// <param name="body">Requested submodel element</param>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated)</param>
        /// <response code="201">Submodel element created successfully</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="409">Conflict, a resource which shall be created exists already. Might be thrown if a Submodel or SubmodelElement with the same ShortId is contained in a POST request.</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPost]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}/submodel-elements/{idShortPath}")]
        [SwaggerOperation("PostSubmodelElementByPathAasRepository")]
        [SwaggerResponse(statusCode: 201, type: typeof(SubmodelElement), description: "Submodel element created successfully")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 409, type: typeof(Result), description: "Conflict, a resource which shall be created exists already. Might be thrown if a Submodel or SubmodelElement with the same ShortId is contained in a POST request.")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult PostSubmodelElementByPathAasRepository([FromBody]SubmodelElement body, [FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromRoute][Required]string idShortPath)
        {
            var output = _aasEnvService.CreateSubmodelElementByPath(body, aasIdentifier, submodelIdentifier, idShortPath);

            return CreatedAtAction(nameof(PostSubmodelElementByPathAasRepository), output);
        }

        /// <summary>
        /// Creates a submodel reference at the Asset Administration Shell
        /// </summary>
        /// <param name="body">Reference to the Submodel</param>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <response code="201">Submodel reference created successfully</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="409">Conflict, a resource which shall be created exists already. Might be thrown if a Submodel or SubmodelElement with the same ShortId is contained in a POST request.</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPost]
        [Route("/shells/{aasIdentifier}/submodel-refs")]
        [SwaggerOperation("PostSubmodelReferenceAasRepository")]
        [SwaggerResponse(statusCode: 201, type: typeof(Reference), description: "Submodel reference created successfully")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 409, type: typeof(Result), description: "Conflict, a resource which shall be created exists already. Might be thrown if a Submodel or SubmodelElement with the same ShortId is contained in a POST request.")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult PostSubmodelReferenceAasRepository([FromBody]Reference body, [FromRoute][Required]string aasIdentifier)
        {
            var output = _aasEnvService.CreateSubmodelReference(body, aasIdentifier);

            return CreatedAtAction(nameof(PostSubmodelReferenceAasRepository), output);
        }

        /// <summary>
        /// Updates an existing Asset Administration Shell
        /// </summary>
        /// <param name="body">Asset Administration Shell object</param>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <response code="204">Asset Administration Shell updated successfully</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPut]
        [Route("/shells/{aasIdentifier}")]
        [SwaggerOperation("PutAssetAdministrationShellById")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult PutAssetAdministrationShellById([FromBody]AssetAdministrationShell body, [FromRoute][Required]string aasIdentifier)
        {
            _aasEnvService.UpdateAssetAdministrationShellById(body, aasIdentifier);

            return NoContent();
        }

        /// <summary>
        /// Updates the Asset Information
        /// </summary>
        /// <param name="body">Asset Information object</param>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <response code="204">Asset Information updated successfully</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPut]
        [Route("/shells/{aasIdentifier}/asset-information")]
        [SwaggerOperation("PutAssetInformationAasRepository")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult PutAssetInformationAasRepository([FromBody]AssetInformation body, [FromRoute][Required]string aasIdentifier)
        {
            _aasEnvService.UpdateAssetInformation(body, aasIdentifier);

            return NoContent();
        }


        /// <summary>
        /// Updates the Submodel
        /// </summary>
        /// <param name="body">Submodel object</param>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <response code="204">Submodel updated successfully</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPut]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}")]
        [SwaggerOperation("PutSubmodelByIdAasRepository")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult PutSubmodelByIdAasRepository([FromBody]Submodel body, [FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromQuery]string level)
        {
            _aasEnvService.UpdateSubmodel(body, aasIdentifier, submodelIdentifier);

            return NoContent();
        }

        /// <summary>
        /// Updates an existing submodel element at a specified path within submodel elements hierarchy
        /// </summary>
        /// <param name="body">Requested submodel element</param>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id</param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated)</param>
        /// <response code="204">Submodel element updated successfully</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPut]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}/submodel-elements/{idShortPath}")]
        [SwaggerOperation("PutSubmodelElementByPathAasRepository")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult PutSubmodelElementByPathAasRepository([FromBody]SubmodelElement body, [FromRoute][Required]string aasIdentifier, [FromRoute][Required]string submodelIdentifier, [FromRoute][Required]string idShortPath)
        {
            _aasEnvService.UpdateSubmodelElementByPath(body, aasIdentifier, submodelIdentifier, idShortPath);

            return NoContent();
        }

        /// <summary>
        /// Updates an existing submodel element at a specified path within submodel elements hierarchy
        /// </summary>
        /// <param name="body">Requested submodel element</param>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique id </param>
        /// <param name="submodelIdentifier">The Submodel’s unique id</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated)</param>
        /// <response code="204">Submodel element updated successfully</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPut]
        [Route("/shells/{aasIdentifier}/submodels/{submodelIdentifier}/file-submodel-elements/{idShortPath}")]
        [SwaggerOperation("PutFileSubmodelElementByPathAasRepository")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult PutFileSubmodelElementByPathAasRepository([FromBody] Stream fileContent, [FromQuery][Required] string filename, [FromRoute][Required] string aasIdentifier, [FromRoute][Required] string submodelIdentifier, [FromRoute][Required] string idShortPath)
        {
            _aasEnvService.UpdateFileByPath(HttpUtility.UrlDecode(aasIdentifier), HttpUtility.UrlDecode(submodelIdentifier), HttpUtility.UrlDecode(idShortPath), filename, "application/octet-stream", fileContent);

            return NoContent();
        }
    }
}
