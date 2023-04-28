
namespace AdminShell
{
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Text;

    [ApiController]
    public class SerializationAPIApiController : ControllerBase
    {
        private readonly AssetAdministrationShellEnvironmentService _aasEnvService;

        public SerializationAPIApiController(AssetAdministrationShellEnvironmentService aasEnvSerive)
        {
            _aasEnvService = aasEnvSerive;
        }

        /// <summary>
        /// Returns an appropriate serialization based on the specified format (see SerializationFormat)
        /// </summary>
        /// <param name="aasIds">The Asset Administration Shells&#x27; unique ids (UTF8-BASE64-URL-encoded)</param>
        /// <param name="submodelIds">The Submodels&#x27; unique ids (UTF8-BASE64-URL-encoded)</param>
        /// <param name="includeConceptDescriptions">Include Concept Descriptions?</param>
        /// <response code="200">Requested serialization based on SerializationFormat</response>
        /// <response code="400">Bad Request, e.g. the request parameters of the format of the request body is wrong.</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/serialization")]
        [SwaggerOperation("GenerateSerializationByIds")]
        [SwaggerResponse(statusCode: 200, type: typeof(byte[]), description: "Requested serialization based on SerializationFormat")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request, e.g. the request parameters of the format of the request body is wrong.")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        [SwaggerResponse(statusCode: 500, type: typeof(Result), description: "Internal Server Error")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GenerateSerializationByIds([FromQuery]List<string> aasIds, [FromQuery]List<string> submodelIds, [FromQuery]bool? includeConceptDescriptions)
        {
            dynamic outputEnv = new ExpandoObject();

            var decodedAasIds = new List<string>();
            foreach (var aasId in aasIds)
            {
                decodedAasIds.Add(Encoding.UTF8.GetString(Convert.FromBase64String(aasId)));
            }

            var decodedSubmodelIds = new List<string>();
            foreach (var submodelId in submodelIds)
            {
                decodedSubmodelIds.Add(Encoding.UTF8.GetString(Convert.FromBase64String(submodelId)));
            }

            outputEnv.AssetAdministrationShells = new List<AssetAdministrationShell>();
            outputEnv.Submodels = new List<Submodel>();

            var aasList = _aasEnvService.GetAllAssetAdministrationShells();
            foreach (var aasId in decodedAasIds)
            {
                var foundAas = aasList.Where(a => a.Identification.Id.Equals(aasId));
                if (foundAas.Any())
                {
                    outputEnv.AssetAdministrationShells.Add(foundAas.First());
                }
            }

            var submodelList = _aasEnvService.GetAllSubmodels();
            foreach (var submodelId in decodedSubmodelIds)
            {
                var foundSubmodel = submodelList.Where(s => s.Identification.Id.Equals(submodelId));
                if (foundSubmodel.Any())
                {
                    outputEnv.Submodels.Add(foundSubmodel.First());
                }
            }

            return new ObjectResult(outputEnv);
        }
    }
}
