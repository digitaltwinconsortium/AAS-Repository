
namespace AdminShell
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.IO;
    using System.Net;
    using System.Text.RegularExpressions;

    public class AasxHttpContextHelper
    {
        public AdminShellPackageEnv[] Packages = null;

        private AssetAdministrationShell FindAAS(string aasid, string queryString = null, string rawUrl = null)
        {
            if (Packages == null)
                return null;

            if (Regex.IsMatch(aasid, @"^\d+$")) // only number, i.e. index
            {
                // Index
                int i = Convert.ToInt32(aasid);

                if (i > Packages.Length)
                    return null;

                if (Packages[i] == null || Packages[i].AasEnv == null || Packages[i].AasEnv.AssetAdministrationShells == null
                    || Packages[i].AasEnv.AssetAdministrationShells.Count < 1)
                    return null;

                return Packages[i].AasEnv.AssetAdministrationShells[0];
            }
            else
            {
                // Name
                if (aasid == "id")
                {
                    return Packages[0].AasEnv.AssetAdministrationShells[0];
                }
                else
                {
                    for (int i = 0; i < Packages.Length; i++)
                    {
                        if (Packages[i] != null)
                        {
                            if (Packages[i].AasEnv.AssetAdministrationShells[0].idShort == aasid)
                            {
                                return Packages[i].AasEnv.AssetAdministrationShells[0];
                            }
                        }
                    }
                }

                return null;
            }
        }

        public class FindSubmodelElementResult
        {
            public AdminShell.Referable elem = null;
            public AdminShell.SubmodelElementWrapper wrapper = null;
            public AdminShell.Referable parent = null;

            public FindSubmodelElementResult(AdminShell.Referable elem = null, AdminShell.SubmodelElementWrapper wrapper = null, AdminShell.Referable parent = null)
            {
                this.elem = elem;
                this.wrapper = wrapper;
                this.parent = parent;
            }
        }

        public FindSubmodelElementResult FindSubmodelElement(AdminShell.Referable parent, List<AdminShell.SubmodelElementWrapper> wrappers, string[] elemids, int elemNdx = 0)
        {
            // trivial
            if (wrappers == null || elemids == null || elemNdx >= elemids.Length)
                return null;

            // dive into each
            foreach (var smw in wrappers)
                if (smw.submodelElement != null)
                {
                    // idShort need to match
                    if (smw.submodelElement.idShort.Trim().ToLower() != elemids[elemNdx].Trim().ToLower())
                        continue;

                    // leaf
                    if (elemNdx == elemids.Length - 1)
                    {
                        return new FindSubmodelElementResult(elem: smw.submodelElement, wrapper: smw, parent: parent);
                    }
                    else
                    {
                        // recurse into?
                        var xsmc = smw.submodelElement as AdminShell.SubmodelElementCollection;
                        if (xsmc != null)
                        {
                            var r = FindSubmodelElement(xsmc, xsmc.value, elemids, elemNdx + 1);
                            if (r != null)
                                return r;
                        }

                        var xop = smw.submodelElement as AdminShell.Operation;
                        if (xop != null)
                        {
                            var w2 = new List<AdminShell.SubmodelElementWrapper>();
                            for (int i = 0; i < 2; i++)
                                foreach (var opv in xop[i])
                                    if (opv.value != null)
                                        w2.Add(opv.value);

                            var r = FindSubmodelElement(xop, w2, elemids, elemNdx + 1);
                            if (r != null)
                                return r;
                        }
                    }
                }

            // nothing
            return null;
        }

        public ExpandoObject EvalGetAasAndAsset(HttpContext context, string aasid)
        {
            dynamic res = new ExpandoObject();

            // access the first AAS
            var aas = FindAAS(aasid, context.Request.QueryString.Value, context.Request.Path.Value);
            if (aas == null)
            {
                context.Response.StatusCode = (int) HttpStatusCode.NotFound;//, $"No AAS with id '{aasid}' found.");
                return null;
            }

            // try to get the asset as well
            var asset = this.Packages[findAasReturn.iPackage].AasEnv.FindAAS(findAasReturn.aas.id);

            // result
            res.AAS = aas;
            res.Asset = asset;

            return res;
        }

        public ExpandoObject EvalGetListAAS(HttpContext context)
        {
            dynamic res = new ExpandoObject();

            // get the list
            var aaslist = new List<string>();

            int aascount = Program.env.Count;

            for (int i = 0; i < aascount; i++)
            {
                if (Program.env[i] != null)
                {
                    var aas = Program.env[i].AasEnv.AssetAdministrationShells[0];
                    string idshort = aas.idShort;

                    aaslist.Add(i.ToString() + " : "
                        + idshort + " : "
                        + aas.id + " : "
                        + Program.envFileName[i]);
                }
            }

            res.aaslist = aaslist;
            return res;
        }

        public Stream EvalGetAASX(HttpContext context, int fileIndex)
        {
            dynamic res = new ExpandoObject();

            // save actual data as file
            lock (Program.changeAasxFile)
            {
                string fname = "./temp/" + Path.GetFileName(Program.envFileName[fileIndex]);
                Program.env[fileIndex].SaveAs(fname);

                // return as FILE
                return System.IO.File.OpenRead(fname);
            }
        }
    }
}
