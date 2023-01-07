﻿
namespace AdminShell
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class AasxFileServerInterfaceService : IAasxFileServerInterfaceService
    {
        private readonly ILogger _logger;
        private AdminShellPackageEnv[] _packages;
        private string[] _envFileNames;

        public AasxFileServerInterfaceService(ILoggerFactory logger)
        {
            _logger = logger.CreateLogger("AasxFileServerInterfaceService");
            _packages = Program.env.ToArray();
            _envFileNames = Program.envFileName.ToArray();
        }

        public void DeleteAASXByPackageId(string packageId)
        {
            int packageIndex = int.Parse(packageId);
            var requestedPackage = _packages[packageIndex];
            if (requestedPackage != null)
            {
                //Move the file to archive
                var archivePath = Path.Combine(Directory.GetCurrentDirectory(), "AasxFileArchive");
                if (!Directory.Exists(archivePath))
                {
                    Directory.CreateDirectory(archivePath);
                }

                var fileName = Path.Combine(archivePath, Path.GetFileName(_envFileNames[packageIndex]));
                if (System.IO.File.Exists(fileName))
                {
                    System.IO.File.Delete(fileName);
                }

                System.IO.File.Move(_envFileNames[packageIndex], fileName);

                _packages[packageIndex] = null;
                _envFileNames[packageIndex] = null;

                TreeBuilder.SignalNewData(TreeBuilder.TreeUpdateMode.RebuildAndCollapse);
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
            var requestedPackage = _packages[packageIndex];
            var requestedFileName = _envFileNames[packageIndex];
            if (!string.IsNullOrEmpty(requestedFileName) && requestedPackage != null)
            {
                //Create Temp file
                string copyFileName = Path.GetTempFileName().Replace(".tmp", ".aasx");
                System.IO.File.Copy(requestedFileName, copyFileName, true);
                Program.env[packageIndex].SaveAs(copyFileName);

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

            for (int i = 0; i < _packages.Length; i++)
            {
                var package = _packages[i];
                if (package != null)
                {
                    var packageDescription = new PackageDescription();
                    packageDescription.PackageId = i.ToString();
                    var aasIdList = new List<string>();
                    foreach (var aas in _packages[i].AasEnv.AssetAdministrationShells)
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
            //var content = Convert.FromBase64String(body);
            System.IO.File.WriteAllBytes(newFileName, fileContent);
            // open again
            var newAasx = new AdminShellPackageEnv(newFileName, true);
            if (newAasx != null)
            {
                if (EmptyPackageAvailable(out int emptyPackageIndex))
                {
                    _packages[emptyPackageIndex] = newAasx;
                    _envFileNames[emptyPackageIndex] = newFileName;

                    TreeBuilder.SignalNewData(TreeBuilder.TreeUpdateMode.RebuildAndCollapse);

                    return emptyPackageIndex.ToString();
                }
                else
                {
                    throw new Exception($"Could not create the file as the datastructure is completely filled.");
                }
            }
            else
            {
                throw new Exception($"Cannot load new package {fileName}.");
            }
        }

        public void UpdateAASXPackageById(string packageId, byte[] fileContent, string fileName)
        {
            int packageIndex = int.Parse(packageId);
            var package = _packages[packageIndex];
            if (package != null)
            {
                var originalFile = _packages[packageIndex].Filename;

                //Create temporary file
                var tempNewFile = Path.GetTempFileName().Replace(".tmp", ".aasx");
                //Write the received file content to this temp file
                //var content = Convert.FromBase64String(fileContent);
                System.IO.File.WriteAllBytes(tempNewFile, fileContent);

                lock (Program.changeAasxFile)
                {
                    try
                    {
                        _packages[packageIndex].Close();
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
                        //Copy tempFile into originalFile location
                        System.IO.File.Copy(tempNewFile, originalFile, overwrite: true);
                        // open again
                        var newAasx = new AdminShellPackageEnv(originalFile, true);
                        if (newAasx != null)
                        {
                            _packages[packageIndex] = newAasx;
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
                }

                TreeBuilder.SignalNewData(TreeBuilder.TreeUpdateMode.RebuildAndCollapse);
            }
            else
            {
                throw new Exception($"Requested package with packageId {packageId} not found.");
            }
        }

        private bool EmptyPackageAvailable(out int emptyPackageIndex)
        {
            emptyPackageIndex = -1;

            for (int envi = 0; envi < _packages.Length; envi++)
            {
                if (_packages[envi] == null)
                {
                    emptyPackageIndex = envi;
                    _packages[emptyPackageIndex] = new AdminShellPackageEnv();
                    return true;
                }
            }

            return false;
        }
    }
}
