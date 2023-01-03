
namespace AdminShell
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.IO;
    using System.Text.RegularExpressions;

    public class AasxHttpContextHelper
    {
        public AdminShellPackageEnv[] Packages = null;

        public class FindSubmodelElementResult
        {
            public Referable elem = null;
            public SubmodelElementWrapper wrapper = null;
            public Referable parent = null;

            public FindSubmodelElementResult(Referable elem = null, SubmodelElementWrapper wrapper = null, Referable parent = null)
            {
                this.elem = elem;
                this.wrapper = wrapper;
                this.parent = parent;
            }
        }

        public FindSubmodelElementResult FindSubmodelElement(Referable parent, List<SubmodelElementWrapper> wrappers, string[] elemids, int elemNdx = 0)
        {
            // trivial
            if (wrappers == null || elemids == null || elemNdx >= elemids.Length)
                return null;

            // dive into each
            foreach (var smw in wrappers)
                if (smw.submodelElement != null)
                {
                    // idShort need to match
                    if (smw.submodelElement.IdShort.Trim().ToLower() != elemids[elemNdx].Trim().ToLower())
                        continue;

                    // leaf
                    if (elemNdx == elemids.Length - 1)
                    {
                        return new FindSubmodelElementResult(elem: smw.submodelElement, wrapper: smw, parent: parent);
                    }
                    else
                    {
                        // recurse into?
                        var xsmc = smw.submodelElement as SubmodelElementCollection;
                        if (xsmc != null)
                        {
                            var r = FindSubmodelElement(xsmc, xsmc.Value, elemids, elemNdx + 1);
                            if (r != null)
                                return r;
                        }

                        var xop = smw.submodelElement as Operation;
                        if (xop != null)
                        {
                            var w2 = new List<SubmodelElementWrapper>();
                            for (int i = 0; i < 2; i++)
                                foreach (var opv in xop[i])
                                    if (opv.Value != null)
                                        w2.Add(opv.Value);

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

            if (Packages == null)
                return null;

            if (Regex.IsMatch(aasid, @"^\d+$")) // only number, i.e. index
            {
                int i = Convert.ToInt32(aasid);

                if (i > Packages.Length)
                    return null;

                if (Packages[i] == null || Packages[i].AasEnv == null || Packages[i].AasEnv.AssetAdministrationShells == null
                    || Packages[i].AasEnv.AssetAdministrationShells.Count < 1)
                    return null;

                res.AAS = Packages[i].AasEnv.AssetAdministrationShells[0];
                res.Asset = Packages[i].AasEnv.FindAAS(aasid);
                return res;
            }
            else
            {
                // Name
                if (aasid == "id")
                {
                    res.AAS = Packages[0].AasEnv.AssetAdministrationShells[0];
                    res.Asset = Packages[0].AasEnv.FindAAS(aasid);
                    return res;
                }
                else
                {
                    for (int i = 0; i < Packages.Length; i++)
                    {
                        if (Packages[i] != null)
                        {
                            if (Packages[i].AasEnv.AssetAdministrationShells[0].IdShort == aasid)
                            {
                                res.AAS = Packages[i].AasEnv.AssetAdministrationShells[0];
                                res.Asset = Packages[i].AasEnv.FindAAS(aasid);
                                return res;
                            }
                        }
                    }
                }

                return null;
            }
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
