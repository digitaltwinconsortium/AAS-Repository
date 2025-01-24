using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;
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
        public async Task<ActionResult> LocalFileOpen(IFormFile[] files, bool autodownloadreferences)
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
