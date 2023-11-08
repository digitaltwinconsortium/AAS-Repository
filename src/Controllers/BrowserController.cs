using AdminShell;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace UANodesetWebViewer.Controllers
{
    public class BrowserController : Controller
    {
        private readonly AASXPackageService _packageService;
        private readonly IFileStorage _storage;

        public BrowserController(AASXPackageService packageService, IFileStorage storage)
        {
            _packageService = packageService;
            _storage = storage;
        }

        public ActionResult Index()
        {
            return View("Index");
        }

        public void GenerateAAS(string name)
        {
            try
            {
                MemoryStream aasxStream = new MemoryStream();
                Package package = Package.Open(aasxStream, FileMode.Create);

                // add package origin part
                PackagePart origin = package.CreatePart(new Uri("/aasx/aasx-origin", UriKind.Relative), MediaTypeNames.Text.Plain);
                using (Stream fileStream = origin.GetStream(FileMode.Create))
                {
                    var bytes = Encoding.ASCII.GetBytes("Intentionally empty.");
                    fileStream.Write(bytes, 0, bytes.Length);
                }
                package.CreateRelationship(origin.Uri, TargetMode.Internal, "http://www.admin-shell.io/aasx/relationships/aasx-origin");

                // add package spec part
                PackagePart spec = package.CreatePart(new Uri("/aasx/" + name + "/" + name, UriKind.Relative), MediaTypeNames.Text.Xml);
                using (FileStream fileStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "NodeSets", name), FileMode.Open, FileAccess.Read))
                {
                    CopyStream(fileStream, spec.GetStream());
                }
                origin.CreateRelationship(spec.Uri, TargetMode.Internal, "http://www.admin-shell.io/aasx/relationships/aas-spec");

                _packageService.AddFileToSpec(package, spec, "/OPCUA.png", new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "OPCUA.png"), FileMode.Open), true);

                package.Flush();
                package.Close();

                _storage.SaveFileAsync(name + ".aasx", aasxStream.ToArray());
                aasxStream.Close();

                _packageService.Reload();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }
        }

        private void CopyStream(Stream source, Stream target)
        {
            const int bufSize = 0x1000;
            byte[] buf = new byte[bufSize];
            int bytesRead = 0;
            while ((bytesRead = source.Read(buf, 0, bufSize)) > 0)
            {
                target.Write(buf, 0, bytesRead);
            }
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

                    // directory validation
                    if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "NodeSets")))
                    {
                        Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "NodeSets"));
                    }

                    // store the file on the webserver
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "NodeSets", file.FileName);
                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream).ConfigureAwait(false);
                    }

                    GenerateAAS(file.FileName);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }

            return View("Index");
        }
    }
}
