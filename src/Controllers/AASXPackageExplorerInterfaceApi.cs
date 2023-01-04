
namespace AdminShell
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Dynamic;
    using System.IO;
    using System.Net;
    using System.Text.RegularExpressions;

    [ApiController]
    public class AASXPackageExplorerInterfaceApiController : ControllerBase
    {
        private AdminShellPackageEnv[] Packages = Program.env.ToArray();

        [HttpGet]
        [Route("/server/listaas")]
        public virtual IActionResult ListAAS()
        {
            dynamic res = new ExpandoObject();

            var aaslist = new List<string>();

            int aascount = Program.env.Count;

            for (int i = 0; i < aascount; i++)
            {
                if (Program.env[i] != null)
                {
                    var aas = Program.env[i].AasEnv.AssetAdministrationShells[0];
                    string idshort = aas.IdShort;

                    aaslist.Add(i.ToString() + " : "
                        + idshort + " : "
                        + aas.Id + " : "
                        + Program.envFileName[i]);
                }
            }

            res.aaslist = aaslist;

            return new JsonResult(res) { StatusCode = (int)HttpStatusCode.OK };
        }

        [HttpGet]
        [Route("/server/getaasx/{Id}")]
        public virtual IActionResult GetAASX([FromRoute][Required] int id)
        {
            string fname = "./temp/" + Path.GetFileName(Program.envFileName[id]);
            lock (Program.changeAasxFile)
            {
                Program.env[id].SaveAs(fname);
            }

            Stream fileStream = System.IO.File.OpenRead(fname);
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
        [Route("/aas/{Id}/core")]
        public virtual IActionResult GetAASInfo([FromRoute][Required] int id)
        {
            dynamic res = new ExpandoObject();

            if (Packages == null)
                return new StatusCodeResult((int)HttpStatusCode.NotFound);

            if (Regex.IsMatch(id.ToString(), @"^\d+$")) // only number, i.e. index
            {
                if (id > Packages.Length)
                    return new StatusCodeResult((int)HttpStatusCode.NotFound);

                if (Packages[id] == null || Packages[id].AasEnv == null || Packages[id].AasEnv.AssetAdministrationShells == null
                    || Packages[id].AasEnv.AssetAdministrationShells.Count < 1)
                    return new StatusCodeResult((int)HttpStatusCode.NotFound);

                res.AAS = Packages[id].AasEnv.AssetAdministrationShells[0];
                res.Asset = Packages[id].AasEnv.FindAAS(id.ToString());
            }
            else
            {
                // Name
                if (id.ToString() == "Id")
                {
                    res.AAS = Packages[0].AasEnv.AssetAdministrationShells[0];
                    res.Asset = Packages[0].AasEnv.FindAAS(id.ToString());
                }
                else
                {
                    for (int i = 0; i < Packages.Length; i++)
                    {
                        if (Packages[i] != null)
                        {
                            if (Packages[i].AasEnv.AssetAdministrationShells[0].IdShort == id.ToString())
                            {
                                res.AAS = Packages[i].AasEnv.AssetAdministrationShells[0];
                                res.Asset = Packages[i].AasEnv.FindAAS(id.ToString());
                            }
                        }
                    }
                }
            }

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ContractResolver = new AdminShellConverters.AdaptiveFilterContractResolver(false, false);
            return new JsonResult(res, settings) { StatusCode = (int)HttpStatusCode.OK };
        }

        [HttpGet]
        [Route("/aas/{aasId}/Submodels/{smIdShort}/complete")]
        public virtual IActionResult GetSubmodelComplete([FromRoute][Required] int aasId, [FromRoute][Required] string smIdShort)
        {
            // access the AAS
            AssetAdministrationShell aas = null;
            int iPackage = -1;
            string aasid = aasId.ToString();

            if (Packages == null)
            {
                return new StatusCodeResult((int)HttpStatusCode.NotFound);
            }

            if (Regex.IsMatch(aasid, @"^\d+$")) // only number, i.e. index
            {
                if (aasId > Packages.Length)
                {
                    return new StatusCodeResult((int)HttpStatusCode.NotFound);
                }

                if (Packages[aasId] == null
                    || Packages[aasId].AasEnv == null
                    || Packages[aasId].AasEnv.AssetAdministrationShells == null
                    || Packages[aasId].AasEnv.AssetAdministrationShells.Count < 1)
                {
                    return new StatusCodeResult((int)HttpStatusCode.NotFound);
                }

                aas = Packages[aasId].AasEnv.AssetAdministrationShells[0];
                iPackage = aasId;
            }
            else
            {
                // Name
                if (aasid == "Id")
                {
                    aas = Packages[0].AasEnv.AssetAdministrationShells[0];
                    iPackage = 0;
                }
                else
                {
                    for (int i = 0; i < Packages.Length; i++)
                    {
                        if (Packages[i] != null)
                        {
                            if (Packages[i].AasEnv.AssetAdministrationShells[0].IdShort == aasid)
                            {
                                aas = Packages[i].AasEnv.AssetAdministrationShells[0];
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
            foreach (var smref in aas.Submodels)
            {
                var sm = Packages[iPackage].AasEnv.FindSubmodel(smref);
                if (sm != null && sm.IdShort != null && sm.IdShort.Trim().ToLower() == smIdShort.Trim().ToLower())
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


