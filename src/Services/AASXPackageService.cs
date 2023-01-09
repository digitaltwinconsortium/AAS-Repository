
namespace AdminShell
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Packaging;
    using System.Net.Mime;
    using System.Text.RegularExpressions;
    using System.Xml;
    using System.Xml.Serialization;

    public class AASXPackageService
    {
        // we use the AASX filename as the key for packages lookup
        public Dictionary<string, AssetAdministrationShellEnvironment> Packages { get; private set; } = new();

        private readonly IFileStorage _storage;

        public AASXPackageService(IFileStorage storage)
        {
            _storage = storage;

            string[] fileNames = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.aasx");
            Console.WriteLine("Found " + fileNames.Length.ToString() + " AAS in directory " + Directory.GetCurrentDirectory());

            // load all AASX files
            foreach (string filename in fileNames)
            {
                try
                {
                    Load(filename);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Can't load AASX file {filename}: {ex}");
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

        private void Load(string filename)
        {
            Package package = Package.Open(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            // verify all the parts exist

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
            string nsURI = TryReadXmlFirstElementNamespaceURI(specStream);

            // reopen spec
            specStream.Close();
            specStream = specPart.GetStream(FileMode.Open, FileAccess.Read);

            // deserialize spec
            AssetAdministrationShellEnvironment aasenv = null;

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

            if (aasenv == null)
            {
                throw new Exception("Type error XML spec file!");
            }

            Packages.Add(filename, aasenv);

            specStream.Close();

            package.Close();
        }

        private string TryReadXmlFirstElementNamespaceURI(Stream specStream)
        {
            string nsURI = null;

            try
            {
                var reader = XmlReader.Create(specStream);

                for (int i = 0; i < 100; i++)
                {
                    reader.Read();
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
            return System.IO.File.OpenRead(key);
        }

        public byte[] GetFileContentsFromPackagePart(string key, string uriString)
        {
            Package package = Package.Open(key, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            var part = package.GetPart(new Uri(uriString, UriKind.RelativeOrAbsolute));
            if (part == null)
            {
                return null;
            }

            using (MemoryStream partStream = new())
            {
                part.GetStream(FileMode.Open).CopyTo(partStream);
                package.Close();

                return partStream.ToArray();
            }
        }

        public byte[] GetLocalThumbnailBytes(string key)
        {
            Package package = Package.Open(key, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

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
                thumbPart.GetStream(FileMode.Open).CopyTo(partStream);
                package.Close();

                return partStream.ToArray();
            }
        }

        public void Delete(string filename)
        {
            Packages.Remove(filename);

            System.IO.File.Delete(filename);
        }

        public byte[] GetAASXBytes(string key)
        {
            return System.IO.File.ReadAllBytes(key);
        }

        public string GetAASXFileName(string key)
        {
            return key;
        }

        public void Save(string filename, byte[] fileContent)
        {
            System.IO.File.WriteAllBytes(filename, fileContent);
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
            Package package = Package.Open(key, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

            package.DeletePart(new Uri(targetFile, UriKind.RelativeOrAbsolute));

            Uri targetUri = PackUriHelper.CreatePartUri(new Uri(targetFile, UriKind.RelativeOrAbsolute));
            PackagePart packagePart = package.CreatePart(targetUri, targetContentType);
            fileContent.Position = 0;

            using (Stream dest = packagePart.GetStream())
            {
                fileContent.CopyTo(dest);
            }

            package.Flush();
            package.Close();
        }

        public void AddSupplementaryFileToPackage(string key, string targetFile, string targetContentType, Stream fileContent)
        {
            Package package = Package.Open(key, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

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

            // try find an existing part for that file ..
            PackagePart filePart = null;
            Uri targetUri = PackUriHelper.CreatePartUri(new Uri(targetFile, UriKind.RelativeOrAbsolute));
            relationships = specPart.GetRelationshipsByType("http://www.admin-shell.io/aasx/relationships/aas-suppl");
            foreach (var relationship in relationships)
            {
                if (relationship.TargetUri == targetUri)
                {
                    filePart = package.GetPart(relationship.TargetUri);
                    break;
                }
            }

            if (filePart == null)
            {
                filePart = package.CreatePart(targetUri, targetContentType, CompressionOption.Maximum);
                specPart.CreateRelationship(filePart.Uri, TargetMode.Internal, "http://www.admin-shell.io/aasx/relationships/aas-suppl");
            }

            using (Stream partStream = filePart.GetStream(FileMode.Create))
            {
                fileContent.CopyTo(partStream);
            }

            package.Flush();
            package.Close();
        }

        public void SaveAs(string key, AssetAdministrationShellEnvironment env)
        {
            // approach is to utilize an existing package, if possible. If not, create from scratch
            Package package = Package.Open(key, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

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
                // create, as not existing
                originPart = package.CreatePart(new Uri("/aasx/aasx-origin", UriKind.RelativeOrAbsolute), MediaTypeNames.Text.Plain, CompressionOption.Maximum);
                using (var s = originPart.GetStream(FileMode.Create))
                {
                    var bytes = System.Text.Encoding.ASCII.GetBytes("Intentionally empty.");
                    s.Write(bytes, 0, bytes.Length);
                }

                package.CreateRelationship(originPart.Uri, TargetMode.Internal, "http://www.admin-shell.io/aasx/relationships/aasx-origin");
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

            // check, if we have to change the spec part
            if (specPart != null && specRel != null)
            {
                var name = Path.GetFileNameWithoutExtension(specPart.Uri.ToString()).ToLower().Trim();
                var ext = Path.GetExtension(specPart.Uri.ToString()).ToLower().Trim();
                if (ext == ".json" || name.StartsWith("aasenv-with-no-Id"))
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
                        specRel = null;
                    }
                }
            }

            if (specPart == null)
            {
                // create, as not existing
                string frn = "aasenv-with-no-Id";

                if (env.AssetAdministrationShells.Count > 0)
                {
                    frn = Regex.Replace(env.AssetAdministrationShells[0].Identification ?? frn, @"[^a-zA-Z0-9\-_]", "_");
                }

                string aas_spec_fn = "/aasx/#/#.aas.xml".Replace("#", frn);

                specPart = package.CreatePart(new Uri(aas_spec_fn, UriKind.RelativeOrAbsolute), MediaTypeNames.Text.Xml, CompressionOption.Maximum);

                originPart.CreateRelationship(specPart.Uri, TargetMode.Internal, "http://www.admin-shell.io/aasx/relationships/aas-spec");
            }

            using (var s = specPart.GetStream(FileMode.Create))
            {
                var serializer = new XmlSerializer(typeof(AssetAdministrationShellEnvironment));
                var nss = new XmlSerializerNamespaces();
                nss.Add("xsi", System.Xml.Schema.XmlSchema.InstanceNamespace);
                nss.Add("aas", "http://www.admin-shell.io/aas/3/0");
                nss.Add("IEC61360", "http://www.admin-shell.io/IEC61360/3/0");
                serializer.Serialize(s, env, nss);
            }

            package.Flush();
            package.Close();
        }
    }
}
