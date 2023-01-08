
namespace AdminShell
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class AasxFileServerInterfaceService
    {
        private readonly ILogger _logger;
        private AASXPackageService _packageService;
 
        public AasxFileServerInterfaceService(ILoggerFactory logger, AASXPackageService packageService)
        {
            _logger = logger.CreateLogger("AasxFileServerInterfaceService");
            _packageService = packageService;
        }

        public void DeleteAASXByPackageId(string packageId)
        {
            if (_packageService.Packages.ContainsKey(packageId))
            {
                _packageService.Delete(packageId);

                VisualTreeBuilderService.SignalNewData(VisualTreeBuilderService.TreeUpdateMode.RebuildAndCollapse);
            }
            else
            {
                throw new Exception($"Package with packageId {packageId} not found.");
            }
        }

        public string GetAASXByPackageId(string packageId, out byte[] content, out long fileSize)
        {
            content = null;
            fileSize = 0;
            int packageIndex = int.Parse(packageId);
            var requestedPackage = _packageService[packageIndex];
            var requestedFileName = _envFileNames[packageIndex];
            if (!string.IsNullOrEmpty(requestedFileName) && requestedPackage != null)
            {
                //Create Temp file
                string copyFileName = Path.GetTempFileName().Replace(".tmp", ".aasx");
                System.IO.File.Copy(requestedFileName, copyFileName, true);
                _packageService[packageIndex].SaveAs(copyFileName);

                content = System.IO.File.ReadAllBytes(copyFileName);
                string fileName = Path.GetFileName(requestedFileName);
                fileSize = content.Length;

                //Delete Temp file
                System.IO.File.Delete(copyFileName);
                return fileName;
            }

            return null;
        }

        public List<PackageDescription> GetAllAASXPackageIds(string aasId = null)
        {
            var output = new List<PackageDescription>();

            for (int i = 0; i < _packageService.Count; i++)
            {
                var package = _packageService[i];
                if (package != null)
                {
                    var packageDescription = new PackageDescription();
                    packageDescription.PackageId = i.ToString();
                    var aasIdList = new List<string>();
                    foreach (var aas in _packageService[i].AasEnv.AssetAdministrationShells)
                    {
                        aasIdList.Add(aas.IdShort);
                    }

                    packageDescription.AasIds = aasIdList;
                    output.Add(packageDescription);
                }
            }

            //Filter w..r.t aasId
            if (output.Any())
            {
                if (!string.IsNullOrEmpty(aasId))
                {
                    output = output.Where(x => x.AasIds.Contains(aasId)).ToList();
                }
            }

            return output;
        }

        public string PostAASXPackage(byte[] fileContent, string fileName)
        {
            var newFileName = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            //Check if file already exists
            if (System.IO.File.Exists(newFileName))
            {
                throw new Exception($"File already exists");
            }

            //TODO:Check file extentsion ".aasx"
     
            //Write the received file content to this temp file
            System.IO.File.WriteAllBytes(newFileName, fileContent);

            // open again
            var newAasx = new AASXPackageService(newFileName, true);
            if (newAasx != null)
            {
                _packageService.Add(newAasx);
                _envFileNames[emptyPackageIndex] = newFileName;

                VisualTreeBuilderService.SignalNewData(VisualTreeBuilderService.TreeUpdateMode.RebuildAndCollapse);

                return emptyPackageIndex.ToString();
            }
            else
            {
                throw new Exception($"Cannot load new package {fileName}.");
            }
        }

        public void UpdateAASXPackageById(string packageId, byte[] fileContent, string fileName)
        {
            int packageIndex = int.Parse(packageId);
            var package = _packageService[packageIndex];
            if (package != null)
            {
                var originalFile = _packageService[packageIndex].Filename;

                //Create temporary file
                var tempNewFile = Path.GetTempFileName().Replace(".tmp", ".aasx");
  
                //Write the received file content to this temp file
                System.IO.File.WriteAllBytes(tempNewFile, fileContent);

                try
                {
                    _packageService[packageIndex].Close();
                    //Create back up of existing file
                    System.IO.File.Copy(originalFile, originalFile + ".bak", overwrite: true);
                }
                catch (Exception e)
                {
                    throw new Exception($"Cannot close/ backup old AASX {originalFile}. Aborting. Exception: {e.Message}");
                }
                try
                {
                    //Replace existing file with temp file
                    originalFile = fileName;
                    //Copy tempFile into originalFile Location
                    System.IO.File.Copy(tempNewFile, originalFile, overwrite: true);
                    // open again
                    var newAasx = new AdminShellPackageEnvironment(originalFile, true);
                    if (newAasx != null)
                    {
                        _packageService[packageIndex] = newAasx;
                    }
                    else
                    {
                        throw new Exception($"Cannot load new package {originalFile} for replacing via PUT. Aborting.");
                    }
                    //now delete tempFile
                    System.IO.File.Delete(tempNewFile);
                }
                catch (Exception e)
                {
                    throw new Exception($"Cannot replace AASX {originalFile} with new {tempNewFile}. Aborting. Exception: {e.Message}");
                }
  
                VisualTreeBuilderService.SignalNewData(VisualTreeBuilderService.TreeUpdateMode.RebuildAndCollapse);
            }
            else
            {
                throw new Exception($"Requested package with packageId {packageId} not found.");
            }
        }
    }
}
