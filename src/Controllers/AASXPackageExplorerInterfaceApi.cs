
namespace AdminShell
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Dynamic;
    using System.IO;
    using System.Net;

    [ApiController]
    public class AASXPackageExplorerInterfaceApiController : ControllerBase
    {
        private readonly AASXPackageService _packageService;

        public AASXPackageExplorerInterfaceApiController(AASXPackageService packageService)
        {
            _packageService = packageService;
        }

        [HttpGet]
        [Route("/server/listaas")]
        public virtual IActionResult ListAAS()
        {
            dynamic res = new ExpandoObject();

            var aaslist = new List<string>();

            int i = 0;
            foreach (AssetAdministrationShellEnvironment env in _packageService.Packages.Values)
            {
                foreach (AssetAdministrationShell aas in env.AssetAdministrationShells)
                {
                    aaslist.Add(i.ToString() + " : " + aas.IdShort + " : " + aas.Identification.Id + " : " + aas.Identification.IdType);
                    i++;
                }
            }

            res.aaslist = aaslist;

            return new JsonResult(res) { StatusCode = (int)HttpStatusCode.OK };
        }

        [HttpGet]
        [Route("/server/getaasx/{Id}")]
        public virtual IActionResult GetAASX([FromRoute][Required] int id)
        {
            int i = 0;
            foreach (KeyValuePair<string, AssetAdministrationShellEnvironment> package in _packageService.Packages)
            {
                foreach (AssetAdministrationShell aas in package.Value.AssetAdministrationShells)
                {
                    if (i == id)
                    {
                        Stream fileStream = _packageService.GetPackageStream(package.Key);
                        if (fileStream != null)
                        {
                            return new FileStreamResult(fileStream, "application/octet-stream");
                        }
                        else
                        {
                            return new StatusCodeResult((int)HttpStatusCode.NotFound);
                        }
                    }

                    i++;
                }
            }

            return new StatusCodeResult((int)HttpStatusCode.NotFound);
        }

        [HttpGet]
        [Route("/aas/{Id}/core")]
        public virtual IActionResult GetAASInfo([FromRoute][Required] int id)
        {
            dynamic res = new ExpandoObject();

            int i = 0;
            foreach (KeyValuePair<string, AssetAdministrationShellEnvironment> package in _packageService.Packages)
            {
                foreach (AssetAdministrationShell aas in package.Value.AssetAdministrationShells)
                {
                    if (i == id)
                    {
                        res.AAS = aas;
                        res.Asset = aas.Asset;

                        JsonSerializerSettings settings = new JsonSerializerSettings();
                        settings.ContractResolver = new AdminShellConverters.AdaptiveFilterContractResolver(false, false);

                        return new JsonResult(res, settings) { StatusCode = (int)HttpStatusCode.OK };
                    }

                    i++;
                }
            }

            return new StatusCodeResult((int)HttpStatusCode.NotFound);
        }

        [HttpGet]
        [Route("/aas/{aasId}/Submodels/{smIdShort}/complete")]
        public virtual IActionResult GetSubmodelComplete([FromRoute][Required] int aasId, [FromRoute][Required] string smIdShort)
        {
            int i = 0;
            foreach (KeyValuePair<string, AssetAdministrationShellEnvironment> package in _packageService.Packages)
            {
                foreach (AssetAdministrationShell aas in package.Value.AssetAdministrationShells)
                {
                    if (i == aasId)
                    {
                        foreach (SubmodelReference smref in aas.Submodels)
                        {
                            // can only refs with 1 key
                            if (smref.Count != 1)
                            {
                                return null;
                            }

                            var key = smref.Keys[0];
                            if (key.Type.ToString().ToLower().Trim() != "submodel")
                            {
                                return null;
                            }

                            foreach (var sm in package.Value.Submodels)
                            {
                                if ((sm.Identification.Id.ToLower().Trim() == key.Value.ToLower().Trim())
                                && (sm.IdShort != null)
                                && (sm.IdShort.Trim().ToLower() == smIdShort.Trim().ToLower()))
                                {
                                    JsonSerializerSettings settings = new JsonSerializerSettings();
                                    settings.ContractResolver = new AdminShellConverters.AdaptiveFilterContractResolver(true, true);

                                    return new JsonResult(sm, settings)
                                    {
                                        StatusCode = (int)HttpStatusCode.OK
                                    };
                                }
                            }
                        }
                    }

                    i++;
                }
            }

            return new StatusCodeResult((int)HttpStatusCode.NotFound);
        }
    }
}


