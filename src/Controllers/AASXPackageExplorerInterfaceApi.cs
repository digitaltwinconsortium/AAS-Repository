
using AasxRestServerLibrary;
using AasxServerBlazor;
using AdminShellNS;
using IO.Swagger.Attributes;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using static AdminShellNS.AdminShellV20;

namespace IO.Swagger.Controllers
{
    [ApiController]
    public class AASXPackageExplorerInterfaceApiController : ControllerBase
    {
        private AasxHttpContextHelper _helper = new AasxHttpContextHelper();

        public AASXPackageExplorerInterfaceApiController()
        {
            _helper.Packages = Program.env.ToArray();
        }

        [HttpGet]
        [Route("/server/listaas")]
        [ValidateModelState]
        public virtual IActionResult ListAAS()
        {
            ExpandoObject result = _helper.EvalGetListAAS(HttpContext);
            return new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK };
        }

        [HttpGet]
        [Route("/server/getaasx/{id}")]
        [ValidateModelState]
        public virtual IActionResult GetAASX([FromRoute][Required] int id)
        {
            Stream fileStream = _helper.EvalGetAASX(HttpContext, id);
            if (fileStream != null)
            {
                return new FileStreamResult(fileStream, "application/octet-stream");
            }
            else
            {
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
        }

        [HttpGet]
        [Route("/aas/{id}/core")]
        [ValidateModelState]
        public virtual IActionResult GetAASInfo([FromRoute][Required] int id)
        {
            ExpandoObject result = _helper.EvalGetAasAndAsset(HttpContext, id.ToString());

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ContractResolver = new AdminShellConverters.AdaptiveFilterContractResolver(false, false);
            return new JsonResult(result, settings) { StatusCode = (int)HttpStatusCode.OK };
        }

        [HttpGet]
        [Route("/aas/{aasId}/submodels/{smIdShort}/complete")]
        [ValidateModelState]
        public virtual IActionResult GetSubmodelComplete([FromRoute][Required] int aasId, [FromRoute][Required] string smIdShort)
        {
            // access the AAS
            AdministrationShell aas = null;
            int iPackage = -1;
            string aasid = aasId.ToString();

            if (_helper.Packages == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.NotFound);
            }

            if (Regex.IsMatch(aasid, @"^\d+$")) // only number, i.e. index
            {
                // Index
                int i = Convert.ToInt32(aasid);

                if (i > _helper.Packages.Length)
                {
                    return new StatusCodeResult((int)HttpStatusCode.NotFound);
                }

                if (_helper.Packages[i] == null
                    || _helper.Packages[i].AasEnv == null
                    || _helper.Packages[i].AasEnv.AdministrationShells == null
                    || _helper.Packages[i].AasEnv.AdministrationShells.Count < 1)
                {
                    return new StatusCodeResult((int)HttpStatusCode.NotFound);
                }

                aas = _helper.Packages[i].AasEnv.AdministrationShells[0];
                iPackage = i;
            }
            else
            {
                // Name
                if (aasid == "id")
                {
                    aas = _helper.Packages[0].AasEnv.AdministrationShells[0];
                    iPackage = 0;
                }
                else
                {
                    for (int i = 0; i < _helper.Packages.Length; i++)
                    {
                        if (_helper.Packages[i] != null)
                        {
                            if (_helper.Packages[i].AasEnv.AdministrationShells[0].idShort == aasid)
                            {
                                aas = _helper.Packages[i].AasEnv.AdministrationShells[0];
                                iPackage = i;
                                break;
                            }
                        }
                    }
                }
            }

            if (aas == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.NotFound);
            }

            // no, iterate & find
            foreach (var smref in aas.submodelRefs)
            {
                var sm = _helper.Packages[iPackage].AasEnv.FindSubmodel(smref);
                if (sm != null && sm.idShort != null && sm.idShort.Trim().ToLower() == smIdShort.Trim().ToLower())
                {
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.ContractResolver = new AdminShellConverters.AdaptiveFilterContractResolver(true, true);
                    return new JsonResult(sm, settings) { StatusCode = (int)HttpStatusCode.OK };
                }
            }

            // no
            return new StatusCodeResult((int)HttpStatusCode.NotFound);
        }
    }
}


