
namespace AdminShell
{
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using System.Collections.Generic;

    [ApiController]
    public class DescriptionAPIApiController : ControllerBase
    {
        /// <summary>
        /// Returns the self-describing information of a network resource (ServiceDescription)
        /// </summary>
        /// <response code="200">Requested Description</response>
        /// <response code="401">Unauthorized, e.g. the server refused the authorization attempt.</response>
        /// <response code="403">Forbidden</response>
        [HttpGet]
        [Route("/description")]
        [SwaggerOperation("GetDescription")]
        [SwaggerResponse(statusCode: 200, type: typeof(ServiceDescription), description: "Requested Description")]
        [SwaggerResponse(statusCode: 401, type: typeof(Result), description: "Unauthorized, e.g. the server refused the authorization attempt.")]
        [SwaggerResponse(statusCode: 403, type: typeof(Result), description: "Forbidden")]
        public virtual IActionResult GetDescription()
        {
            return new ObjectResult(new ServiceDescription()
            {  Profiles = new List<ServiceDescription.ProfilesEnum>()
                {
                    ServiceDescription.ProfilesEnum.AasxFileServerServiceSpecificationV30Enum,
                    ServiceDescription.ProfilesEnum.AssetAdministrationShellRepositoryServiceSpecificationV30Enum,
                    ServiceDescription.ProfilesEnum.RepositoryServiceSpecificationV30Enum,
                    ServiceDescription.ProfilesEnum.SubmodelRepositoryServiceSpecificationV30Enum,
                    ServiceDescription.ProfilesEnum.SubmodelServiceSpecificationV30Enum
                }
            });
        }
    }
}
