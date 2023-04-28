
namespace AdminShell
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Swashbuckle.AspNetCore.Annotations;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    [ApiController]
    public class ConceptDescriptionRepositoryAPIApiController : ControllerBase
    {
        private readonly AssetAdministrationShellEnvironmentService _aasEnvService;

        public ConceptDescriptionRepositoryAPIApiController(AssetAdministrationShellEnvironmentService aasEnvService)
        {
            _aasEnvService = aasEnvService;
        }

        /// <summary>
        /// Deletes a Concept Description
        /// </summary>
        /// <param name="cdIdentifier">The Concept Description’s unique id (UTF8-BASE64-URL-encoded)</param>
        /// <response code="204">Concept Description deleted successfully</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpDelete]
        [Route("/concept-descriptions/{cdIdentifier}")]
        [SwaggerOperation("DeleteConceptDescriptionById")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult DeleteConceptDescriptionById([FromRoute][Required]byte[] cdIdentifier)
        {
            var decodedCdId = Encoding.UTF8.GetString(Convert.FromBase64String(Encoding.UTF8.GetString(cdIdentifier)));

            _aasEnvService.DeleteConceptDescriptionById(decodedCdId);

            return NoContent();
        }

        /// <summary>
        /// Returns all Concept Descriptions
        /// </summary>
        /// <param name="idShort">The Concept Description’s IdShort</param>
        /// <param name="isCaseOf">IsCaseOf reference (UTF8-BASE64-URL-encoded)</param>
        /// <param name="dataSpecificationRef">DataSpecification reference (UTF8-BASE64-URL-encoded)</param>
        /// <param name="limit">The maximum number of elements in the response array</param>
        /// <param name="cursor">A server-generated identifier retrieved from pagingMetadata that specifies from which position the result listing should continue</param>
        /// <response code="200">Requested Concept Descriptions</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/concept-descriptions")]
        [SwaggerOperation("GetAllConceptDescriptions")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<ConceptDescription>), description: "Requested Concept Descriptions")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetAllConceptDescriptions([FromQuery]string idShort, [FromQuery]byte[] isCaseOf, [FromQuery]byte[] dataSpecificationRef, [FromQuery]int? limit, [FromQuery]string cursor)
        {
            Reference reqIsCaseOf = JsonConvert.DeserializeObject<Reference>(Encoding.UTF8.GetString(Convert.FromBase64String(Encoding.UTF8.GetString(isCaseOf))));
            Reference reqDataSpecificationRef = JsonConvert.DeserializeObject<Reference>(Encoding.UTF8.GetString(Convert.FromBase64String(Encoding.UTF8.GetString(dataSpecificationRef))));

            var output = _aasEnvService.GetAllConceptDescriptions(idShort, reqIsCaseOf, reqDataSpecificationRef);

            return new ObjectResult(output);
        }

        /// <summary>
        /// Returns a specific Concept Description
        /// </summary>
        /// <param name="cdIdentifier">The Concept Description’s unique id (UTF8-BASE64-URL-encoded)</param>
        /// <response code="200">Requested Concept Description</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/concept-descriptions/{cdIdentifier}")]
        [SwaggerOperation("GetConceptDescriptionById")]
        [SwaggerResponse(statusCode: 200, type: typeof(ConceptDescription), description: "Requested Concept Description")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetConceptDescriptionById([FromRoute][Required]byte[] cdIdentifier)
        {
            var decodedCdId = Encoding.UTF8.GetString(Convert.FromBase64String(Encoding.UTF8.GetString(cdIdentifier)));

            var output = _aasEnvService.GetConceptDescriptionById(decodedCdId, out _);

            return new ObjectResult(output);
        }

        /// <summary>
        /// Creates a new Concept Description
        /// </summary>
        /// <param name="body">Concept Description object</param>
        /// <response code="201">Concept Description created successfully</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="409">Conflict, a resource which shall be created exists already. Might be thrown if a Submodel or SubmodelElement with the same ShortId is contained in a POST request.</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPost]
        [Route("/concept-descriptions")]
        [SwaggerOperation("PostConceptDescription")]
        [SwaggerResponse(statusCode: 201, type: typeof(ConceptDescription), description: "Concept Description created successfully")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 409, type: typeof(Result), description: "Conflict, a resource which shall be created exists already. Might be thrown if a Submodel or SubmodelElement with the same ShortId is contained in a POST request.")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult PostConceptDescription([FromBody]ConceptDescription body)
        {
            var output = _aasEnvService.CreateConceptDescription(body);

            return CreatedAtAction(nameof(PostConceptDescription), output);
        }

        /// <summary>
        /// Updates an existing Concept Description
        /// </summary>
        /// <param name="body">Concept Description object</param>
        /// <param name="cdIdentifier">The Concept Description’s unique id (UTF8-BASE64-URL-encoded)</param>
        /// <response code="204">Concept Description updated successfully</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPut]
        [Route("/concept-descriptions/{cdIdentifier}")]
        [SwaggerOperation("PutConceptDescriptionById")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult PutConceptDescriptionById([FromBody]ConceptDescription body, [FromRoute][Required]byte[] cdIdentifier)
        {
            var decodedCdId = Encoding.UTF8.GetString(Convert.FromBase64String(Encoding.UTF8.GetString(cdIdentifier)));

            _aasEnvService.UpdateConceptDescriptionById(body, decodedCdId);

            return NoContent();
        }
    }
}
