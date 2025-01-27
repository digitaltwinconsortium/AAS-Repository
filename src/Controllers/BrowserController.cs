using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Opc.Ua.Export;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdminShell
{
    public class BrowserController : Controller
    {
        public ActionResult Index()
        {
            return View("Index", new BrowserModel() { StatusMessage = string.Empty });
        }

        [HttpPost]
        public async Task<ActionResult> LocalFileOpen(IFormFile[] files, bool autodownloadreferences = false)
        {
            try
            {
                if ((files == null) || (files.Length == 0))
                {
                    throw new ArgumentException("No files specified!");
                }

                foreach (IFormFile file in files)
                {
                    if ((file.Length == 0) || (file.ContentType != "text/xml"))
                    {
                        throw new ArgumentException("Invalid file specified!");
                    }

                    // file name validation
                    new FileInfo(file.FileName);

                    // store the file on the webserver
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "NodeSets", file.FileName);
                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream).ConfigureAwait(false);
                    }

                    SimpleServer server = (SimpleServer)Program.App.Server;
                    NodesetFileNodeManager nodeManager = (NodesetFileNodeManager)server.CurrentInstance.NodeManager.NodeManagers[2];

                    // add the namespace to the server
                    using (FileStream stream = new(filePath, FileMode.Open, FileAccess.Read))
                    {
                        UANodeSet nodeSet = UANodeSet.Read(stream);

                        if ((nodeSet.NamespaceUris != null) && (nodeSet.NamespaceUris.Length > 0))
                        {
                            foreach (string ns in nodeSet.NamespaceUris)
                            {
                                if (!nodeManager.NamespaceUris.Contains(ns))
                                {
                                    nodeManager.AddNamespace(ns);
                                }
                            }
                        }
                    }

                    // add the nodes to the server
                    nodeManager.AddNodesFromNodesetXml(filePath);

                    // disconnect all client sessions
                    OpcSessionHelper.Instance.DisconnectAll();

                    Console.WriteLine($"Nodeset {file.FileName} loaded successfully.");
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return View("Index", new BrowserModel() { StatusMessage = ex.Message });
            }

            return View("Index", new BrowserModel() { StatusMessage = "Nodeset loaded successfully." });
        }
    }
}
