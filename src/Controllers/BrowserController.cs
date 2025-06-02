using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AdminShell
{
    /// <summary>
    /// Browser-related support: uploading NodeSet files.
    /// </summary>
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
                    nodeManager.AddNamespace(filePath);

                    // add the nodes to the server
                    nodeManager.AddNodesFromNodesetXml(filePath);

                    Console.WriteLine($"Nodeset {file.FileName} loaded successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("LocalFileOpen: " + ex.Message);

                return View("Index", new BrowserModel() { StatusMessage = ex.Message });
            }

            return View("Index", new BrowserModel() { StatusMessage = "Nodeset loaded successfully." });
        }
    }
}
