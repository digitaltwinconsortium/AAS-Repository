
namespace AdminShell
{
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Packaging;
    using System.Net.Mime;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml;
    using System.Xml.Serialization;

    public class AASXPackageService
    {
        // we use the AASX filename as the key for packages lookup
        public Dictionary<string, AssetAdministrationShellEnvironment> Packages { get; private set; } = new();

        private readonly IFileStorage _storage;
        private readonly ILogger _logger;
        private List<PackageSupplementaryFile> _files;

        public AASXPackageService(ILoggerFactory logger, IFileStorage storage)
        {
            _storage = storage;
            _logger = logger.CreateLogger("AASXPackageService");
            _files = new();

            Reload();
        }

        public void Reload()
        {
            string[] fileNames = _storage.FindAllFilesAsync(".").GetAwaiter().GetResult();
            if (fileNames != null)
            {
                // load all AASX files
                foreach (string filename in fileNames)
                {
                    try
                    {
                        if (filename.EndsWith(".aasx"))
                        {
                            _logger.LogInformation("Found AASX file " + filename + " in storage.");

                            Load(filename);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Can't load AASX file {filename}: {ex}");
                    }
                }
            }

            // set all parents
            foreach (AssetAdministrationShellEnvironment aasEnv in Packages.Values)
            {
                foreach (var sm in aasEnv.Submodels)
                {
                    SetSMEParent(sm);
                }
            }

            VisualTreeBuilderService.SignalNewData(TreeUpdateMode.RebuildAndCollapse);
        }

        public byte[] ReadFile(string key, out Uri uri)
        {
            foreach(PackageSupplementaryFile file in _files)
            {
                if (file.Key == key)
                {
                    uri = file.Uri;
                    return file.SourceBytes;
                }
            }

            uri = null;
            return null;
        }

        private void SetSMEParent(Submodel sm)
        {
            if (sm.SubmodelElements != null)
            {
                foreach (SubmodelElementWrapper smew in sm.SubmodelElements)
                {
                    SetSMEParent(smew.SubmodelElement, sm);
                }
            }
        }

        private void SetSMEParent(SubmodelElement sme, Referable parent)
        {
            if (sme == null)
            {
                return;
            }

            sme.Parent = parent;

            // recurse if needed
            if (sme is SubmodelElementCollection collection)
            {
                if (collection.Value != null)
                {
                    foreach (SubmodelElementWrapper smew in collection.Value)
                    {
                        SetSMEParent(smew.SubmodelElement, sme);
                    }
                }
            }
        }

        public void Load(string key, string newKey = null)
        {
            Package package;
            if (!string.IsNullOrEmpty(newKey))
            {
                package = Package.Open(GetLocalPackageStream(key), FileMode.Open, FileAccess.Read);
            }
            else
            {
                package = Package.Open(GetPackageStream(key), FileMode.Open, FileAccess.Read);
            }

            System.IO.Packaging.AdminShell.PackageDigitalSignatureManager dsm = new(package);

            // verify the collection of certificates in the package
            foreach (System.IO.Packaging.AdminShell.PackageDigitalSignature signature in dsm.Signatures)
            {
                System.IO.Packaging.AdminShell.PackageDigitalSignatureManager.VerifyCertificate(signature.Signer);
            }

            // verify all signatures in the package
            System.IO.Packaging.AdminShell.VerifyResult vResult = dsm.VerifySignatures(false);
            if ((vResult != System.IO.Packaging.AdminShell.VerifyResult.NotSigned) && (vResult != System.IO.Packaging.AdminShell.VerifyResult.Success))
            {
                _logger.LogWarning("Package signature invalid!");
            }

            // verify that all the parts exist

            // get the origin from the package
            PackagePart originPart = null;
            PackageRelationshipCollection relationships = package.GetRelationshipsByType("http://www.admin-shell.io/aasx/relationships/aasx-origin");
            foreach (var relationship in relationships)
            {
                if (relationship.SourceUri.ToString() == "/")
                {
                    originPart = package.GetPart(relationship.TargetUri);
                    break;
                }
            }

            if (originPart == null)
            {
                throw new Exception("Unable to find AASX origin. Aborting!");
            }

            // get the specs from the package
            PackagePart specPart = null;
            relationships = originPart.GetRelationshipsByType("http://www.admin-shell.io/aasx/relationships/aas-spec");
            foreach (var relationship in relationships)
            {
                // at least one is OK
                specPart = package.GetPart(relationship.TargetUri);
                break;
            }

            if (specPart == null)
            {
                throw new Exception("Unable to find AASX spec(s). Aborting!");
            }

            Stream specStream = specPart.GetStream(FileMode.Open);

            string nsURI = null;
            if ((specPart.ContentType == "text/xml") || (specPart.ContentType == "application/xml"))
            {
                nsURI = TryReadXmlFirstElementNamespaceURI(specStream);
            }

            // reopen spec
            specStream.Close();
            specStream = specPart.GetStream(FileMode.Open, FileAccess.Read);

            // deserialize spec
            AssetAdministrationShellEnvironment aasenv = null;

            if ((specPart.ContentType == "text/xml") || (specPart.ContentType == "application/xml"))
            {
                // read V1.0
                if (nsURI != null && nsURI.Trim() == "http://www.admin-shell.io/aas/1/0")
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(AssetAdministrationShellEnvironment), "http://www.admin-shell.io/aas/1/0");
                    aasenv = serializer.Deserialize(specStream) as AssetAdministrationShellEnvironment;
                }

                // read V2.0
                if (nsURI != null && nsURI.Trim() == "http://www.admin-shell.io/aas/2/0")
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(AssetAdministrationShellEnvironment), "http://www.admin-shell.io/aas/2/0");
                    aasenv = serializer.Deserialize(specStream) as AssetAdministrationShellEnvironment;
                }

                // read V3.0
                if (nsURI != null && nsURI.Trim() == "http://www.admin-shell.io/aas/3/0")
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(AssetAdministrationShellEnvironment), "http://www.admin-shell.io/aas/3/0");
                    aasenv = serializer.Deserialize(specStream) as AssetAdministrationShellEnvironment;
                }

                // read OPC UA nodeset
                if (nsURI != null && nsURI.Trim() == "http://opcfoundation.org/UA/2011/03/UANodeSet.xsd")
                {
                    aasenv = new AssetAdministrationShellEnvironment();
                    aasenv.AssetAdministrationShells.Add(new AssetAdministrationShell() { IdShort = specPart.Uri.ToString().Substring(specPart.Uri.ToString().LastIndexOf('/') + 1)});

                    PackageSupplementaryFile file = new()
                    {
                        Uri = specPart.Uri,
                        Location = LocationType.InPackage,
                        Key = key
                    };

                    using (MemoryStream partStream = new())
                    {
                        specPart.GetStream(FileMode.Open).CopyToAsync(partStream).GetAwaiter().GetResult();
                        file.SourceBytes = partStream.ToArray();
                    }

                    _files.Add(file);
                }
            }
            else
            {
                byte[] bytes = new byte[specStream.Length];
                specStream.Read(bytes);
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
                aasenv = JsonConvert.DeserializeObject<AssetAdministrationShellEnvironment>(Encoding.UTF8.GetString(bytes), settings);
            }

            ReadFilesFromPackage(key, package);

            if (aasenv == null)
            {
                throw new Exception("Error parsing AAS spec!");
            }

            if (newKey != null)
            {
                Save(newKey, System.IO.File.ReadAllBytes(key));
            }
            else
            {
                Packages.Add(key, aasenv);
            }

            specStream.Close();

            package.Close();
        }

        private string TryReadXmlFirstElementNamespaceURI(Stream specStream)
        {
            string nsURI = null;

            try
            {
                var reader = XmlReader.Create(specStream);

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        nsURI = reader.NamespaceURI;
                        break;
                    }
                }

                reader.Close();
            }
            catch (Exception)
            {
                // do nothing
            }

            return nsURI;
        }

        public Stream GetPackageStream(string key)
        {
            try
            {
                MemoryStream newStream = new();
                using (MemoryStream contents = new(_storage.LoadFileAsync(key).GetAwaiter().GetResult()))
                {
                    contents.CopyToAsync(newStream).GetAwaiter().GetResult();
                }

                return newStream;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private Stream GetLocalPackageStream(string key)
        {
            try
            {
                if (!Path.IsPathRooted(key))
                {
                    key = Path.Combine(Directory.GetCurrentDirectory(), key);
                }

                MemoryStream newStream = new();

                using (MemoryStream contents = new(System.IO.File.ReadAllBytes(key)))
                {
                    contents.CopyToAsync(newStream).GetAwaiter().GetResult();
                }

                return newStream;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public byte[] GetFileContentsFromPackagePart(string key, string uriString)
        {
            Package package = Package.Open(GetPackageStream(key), FileMode.Open, FileAccess.Read);

            var part = package.GetPart(new Uri(uriString, UriKind.RelativeOrAbsolute));
            if (part == null)
            {
                return null;
            }

            using (MemoryStream partStream = new())
            {
                part.GetStream(FileMode.Open).CopyToAsync(partStream).GetAwaiter().GetResult();
                package.Close();

                return partStream.ToArray();
            }
        }

        public byte[] GetThumbnailFromPackage(string key)
        {
            Package package = Package.Open(GetPackageStream(key), FileMode.Open, FileAccess.Read);

            PackagePart thumbPart = null;

            PackageRelationshipCollection collection = package.GetRelationshipsByType("http://schemas.openxmlformats.org/package/2006/relationships/metadata/thumbnail");

            foreach (PackageRelationship relationship in collection)
            {
                if (relationship.SourceUri.ToString() == "/")
                {
                    thumbPart = package.GetPart(relationship.TargetUri);
                    break;
                }
            }

            if (thumbPart == null)
            {
                return null;
            }

            using (MemoryStream partStream = new())
            {
                thumbPart.GetStream(FileMode.Open).CopyToAsync(partStream).GetAwaiter().GetResult();
                package.Close();

                return partStream.ToArray();
            }
        }

        public void Delete(string filename)
        {
            Packages.Remove(filename);

            _storage.DeleteFileAsync(filename).GetAwaiter().GetResult();
        }

        public string GetAASXFileName(string key)
        {
            return Path.GetFileNameWithoutExtension(key);
        }

        public void Save(string filename, byte[] fileContent)
        {
            _storage.SaveFileAsync(filename, fileContent).GetAwaiter().GetResult();

            Load(filename);
        }

        public void Save(string key)
        {
            SaveAs(key, Packages[key]);
        }

        public string GetPackageID(string filename)
        {
            return filename;
        }

        public void ReplaceSupplementaryFileInPackage(string key, string targetFile, string targetContentType, Stream fileContent)
        {
            MemoryStream packageStream = (MemoryStream)GetPackageStream(key);
            Package package = Package.Open(packageStream, FileMode.Open, FileAccess.ReadWrite);

            package.DeletePart(new Uri(targetFile, UriKind.RelativeOrAbsolute));

            Uri targetUri = PackUriHelper.CreatePartUri(new Uri(targetFile, UriKind.RelativeOrAbsolute));
            PackagePart packagePart = package.CreatePart(targetUri, targetContentType);
            fileContent.Position = 0;

            using (Stream dest = packagePart.GetStream())
            {
                fileContent.CopyToAsync(dest).GetAwaiter().GetResult();
            }

            package.Flush();
            package.Close();

            _storage.SaveFileAsync(key, packageStream.ToArray()).GetAwaiter().GetResult();
            packageStream.Close();
        }

        public void ReadFilesFromPackage(string key, Package package)
        {
            // get the thumbnail(s) from the package
            PackagePart thumbPart;
            foreach (PackageRelationship rel in package.GetRelationshipsByType("http://schemas.openxmlformats.org/package/2006/relationships/metadata/thumbnail"))
            {
                _logger.LogInformation("Package relationship for thumbnail in package " + key + ": " + rel.TargetUri);

                if (rel.SourceUri.ToString() == "/")
                {
                    thumbPart = package.GetPart(rel.TargetUri);

                    PackageSupplementaryFile file = new()
                    {
                        Uri = rel.TargetUri,
                        Location = LocationType.InPackage,
                        SpecialHandling = SpecialHandlingType.EmbedAsThumbnail,
                        Key = key
                    };

                    using (MemoryStream partStream = new())
                    {
                        thumbPart.GetStream(FileMode.Open).CopyToAsync(partStream).GetAwaiter().GetResult();
                        file.SourceBytes = partStream.ToArray();
                    }

                    _files.Add(file);
                }
            }

            // get the origin from the package
            PackagePart originPart = null;
            foreach (PackageRelationship rel in package.GetRelationshipsByType("http://www.admin-shell.io/aasx/relationships/aasx-origin"))
            {
                if (rel.SourceUri.ToString() == "/")
                {
                    originPart = package.GetPart(rel.TargetUri);
                    break;
                }
            }

            if (originPart != null)
            {
                // get the specs from the origin
                PackagePart specPart = null;
                foreach (PackageRelationship rel in originPart.GetRelationshipsByType("http://www.admin-shell.io/aasx/relationships/aas-spec"))
                {
                    specPart = package.GetPart(rel.TargetUri);
                    break;
                }

                if (specPart != null)
                {
                    // get the supplemental files from the package, derived from spec
                    foreach (PackageRelationship rel in specPart.GetRelationshipsByType("http://www.admin-shell.io/aasx/relationships/aas-suppl"))
                    {
                        _logger.LogInformation("Package relationship for supplemental file in package " + key + ": " + rel.TargetUri);

                        // try to find the part for the file
                        foreach (PackagePart packagePart in package.GetParts())
                        {
                            if (packagePart.Uri == rel.TargetUri)
                            {
                                PackageSupplementaryFile file = new()
                                {
                                    Uri = rel.TargetUri,
                                    Location = LocationType.InPackage,
                                    Key = key
                                };

                                using (MemoryStream partStream = new())
                                {
                                    packagePart.GetStream(FileMode.Open).CopyToAsync(partStream).GetAwaiter().GetResult();
                                    file.SourceBytes = partStream.ToArray();
                                }

                                _files.Add(file);
                                break;
                            }
                        }
                    }
                }
            }
        }

        public void AddSupplementaryFileToPackage(string key, string targetFile, string targetContentType, Stream fileContent)
        {
            MemoryStream packageStream = (MemoryStream)GetPackageStream(key);
            Package package = Package.Open(packageStream, FileMode.Open, FileAccess.ReadWrite);

            // get the origin from the package
            PackagePart originPart = null;
            PackageRelationshipCollection relationships = package.GetRelationshipsByType("http://www.admin-shell.io/aasx/relationships/aasx-origin");
            foreach (var relationship in relationships)
            {
                if (relationship.SourceUri.ToString() == "/")
                {
                    originPart = package.GetPart(relationship.TargetUri);
                    break;
                }
            }

            if (originPart == null)
            {
                throw new Exception("Origin part missing in package!");
            }

            // get the specs from the package
            PackagePart specPart = null;
            PackageRelationship specRel = null;
            relationships = originPart.GetRelationshipsByType("http://www.admin-shell.io/aasx/relationships/aas-spec");
            foreach (var relationship in relationships)
            {
                specRel = relationship;
                specPart = package.GetPart(relationship.TargetUri);
                break;
            }

            if (specPart == null)
            {
                throw new Exception("Spec part missing in package!");
            }

            AddFileToSpec(package, specPart, targetFile, fileContent);

            package.Flush();
            package.Close();

            _storage.SaveFileAsync(key, packageStream.ToArray()).GetAwaiter().GetResult();
            packageStream.Close();
        }

        public void AddFileToSpec(Package package, PackagePart specPart, string targetFile, Stream fileContent, bool addAsThumbnail = false)
        {
            Uri targetUri = PackUriHelper.CreatePartUri(new Uri(targetFile, UriKind.RelativeOrAbsolute));

            // try to find an existing relationship for the file
            PackageRelationship existingRelationship = null;

            PackageRelationshipCollection relationships;
            if (addAsThumbnail)
            {
                relationships = package.GetRelationshipsByType("http://schemas.openxmlformats.org/package/2006/relationships/metadata/thumbnail");

            }
            else
            {
                relationships = specPart.GetRelationshipsByType("http://www.admin-shell.io/aasx/relationships/aas-suppl");
            }

            foreach (PackageRelationship relationship in relationships)
            {
                if (relationship.TargetUri == targetUri)
                {
                    // relatinship already exists
                    existingRelationship = relationship;
                    break;
                }
            }

            if (existingRelationship == null)
            {
                if (addAsThumbnail)
                {
                    specPart.CreateRelationship(targetUri, TargetMode.Internal, "http://schemas.openxmlformats.org/package/2006/relationships/metadata/thumbnail");
                }
                else
                {
                    specPart.CreateRelationship(targetUri, TargetMode.Internal, "http://www.admin-shell.io/aasx/relationships/aas-suppl");
                }
            }

            // try to find an existing part for the file
            PackagePart filePart = null;
            PackagePartCollection packages = package.GetParts();
            foreach (PackagePart packagePart in packages)
            {
                if (packagePart.Uri == targetUri)
                {
                    // part already exists
                    filePart = packagePart;
                    break;
                }
            }

            if (filePart == null)
            {
                filePart = package.CreatePart(targetUri, MediaTypeNames.Application.Octet, CompressionOption.Maximum);
            }

            using (Stream partStream = filePart.GetStream(FileMode.Create))
            {
                fileContent.CopyToAsync(partStream).GetAwaiter().GetResult();
            }
        }

        public void SaveAs(string key, AssetAdministrationShellEnvironment env)
        {
            // utilize an existing package, if possible. If not, create from scratch

            MemoryStream packageStream = (MemoryStream)GetPackageStream(key);
            Package package = Package.Open(packageStream, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            // get the origin from the package
            PackagePart originPart = null;
            foreach (PackageRelationship relationship in package.GetRelationshipsByType("http://www.admin-shell.io/aasx/relationships/aasx-origin"))
            {
                if (relationship.SourceUri.ToString() == "/")
                {
                    originPart = package.GetPart(relationship.TargetUri);
                    break;
                }
            }

            if (originPart == null)
            {
                // create, as not existing
                originPart = package.CreatePart(new Uri("/aasx/aasx-origin", UriKind.RelativeOrAbsolute), MediaTypeNames.Text.Plain, CompressionOption.Maximum);
                using (var s = originPart.GetStream(FileMode.Create))
                {
                    var bytes = Encoding.ASCII.GetBytes("Intentionally empty.");
                    s.Write(bytes, 0, bytes.Length);
                }

                package.CreateRelationship(originPart.Uri, TargetMode.Internal, "http://www.admin-shell.io/aasx/relationships/aasx-origin");
            }

            // get the specs from the package
            PackagePart specPart = null;
            PackageRelationship specRel = null;
            foreach (PackageRelationship relationship in originPart.GetRelationshipsByType("http://www.admin-shell.io/aasx/relationships/aas-spec"))
            {
                specRel = relationship;
                specPart = package.GetPart(relationship.TargetUri);
                break;
            }

            // check if we have to change the spec part
            if (specPart != null && specRel != null)
            {
                var name = Path.GetFileNameWithoutExtension(specPart.Uri.ToString()).ToLower().Trim();
                var ext = Path.GetExtension(specPart.Uri.ToString()).ToLower().Trim();
                if (ext == ".json" || name.StartsWith("aasenv-with-no-Id") || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("USE_JSON_SERIALIZATION")))
                {
                    try
                    {
                        originPart.DeleteRelationship(specRel.Id);
                        package.DeletePart(specPart.Uri);
                    }
                    catch (Exception)
                    {
                        // do nothing
                    }
                    finally
                    {
                        specPart = null;
                    }
                }
            }

            if (specPart == null)
            {
                // create, as not existing
                string frn = "aasenv-with-no-Id";

                if (env.AssetAdministrationShells.Count > 0)
                {
                    frn = Regex.Replace(env.AssetAdministrationShells[0].IdShort ?? frn, @"[^a-zA-Z0-9\-_]", "_");
                }

                if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("USE_JSON_SERIALIZATION")))
                {
                    string aas_spec_fn = "/aasx/#/#.aas.json".Replace("#", frn);
                    specPart = package.CreatePart(new Uri(aas_spec_fn, UriKind.RelativeOrAbsolute), MediaTypeNames.Text.Plain, CompressionOption.Maximum);
                }
                else
                {
                    string aas_spec_fn = "/aasx/#/#.aas.xml".Replace("#", frn);
                    specPart = package.CreatePart(new Uri(aas_spec_fn, UriKind.RelativeOrAbsolute), MediaTypeNames.Text.Xml, CompressionOption.Maximum);
                }

                originPart.CreateRelationship(specPart.Uri, TargetMode.Internal, "http://www.admin-shell.io/aasx/relationships/aas-spec");
            }

            using (var s = specPart.GetStream(FileMode.Create))
            {
                if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("USE_JSON_SERIALIZATION")))
                {
                    JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
                    string json = JsonConvert.SerializeObject(env, settings);
                    s.Write(Encoding.UTF8.GetBytes(json));
                }
                else
                {
                    var serializer = new XmlSerializer(typeof(AssetAdministrationShellEnvironment));
                    var nss = new XmlSerializerNamespaces();
                    nss.Add("xsi", System.Xml.Schema.XmlSchema.InstanceNamespace);
                    nss.Add("aas", "http://www.admin-shell.io/aas/3/0");
                    nss.Add("IEC", "http://www.admin-shell.io/IEC61360/3/0");
                    nss.Add("abac", "http://www.admin-shell.io/aas/abac/3/0");
                    serializer.Serialize(s, env, nss);
                }

                foreach (PackageSupplementaryFile file in _files)
                {
                    if (file.Key == key)
                    {
                        using (MemoryStream contents = new(file.SourceBytes))
                        {
                            if (file.SpecialHandling == SpecialHandlingType.EmbedAsThumbnail)
                            {
                                AddFileToSpec(package, specPart, file.Uri.ToString(), contents, true);
                            }
                            else
                            {
                                AddFileToSpec(package, specPart, file.Uri.ToString(), contents);
                            }
                        }
                    }
                }

                s.Flush();
                s.Close();
            }

            package.Flush();
            package.Close();

            _storage.SaveFileAsync(key, packageStream.ToArray()).GetAwaiter().GetResult();
            packageStream.Close();
        }
    }
}
