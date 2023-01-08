
namespace AdminShell
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Packaging;
    using System.Net.Mime;
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
                    sm.SetSMEParent();
                }
            }

            VisualTreeBuilderService.SignalNewData(VisualTreeBuilderService.TreeUpdateMode.RebuildAndCollapse);
        }

        private void Load(string filename)
        {
            Package package = Package.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read);

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

        private string GuessMimeType(string filename)
        {
            string fileExtension = Path.GetExtension(filename).ToLower();

            string contentType = MediaTypeNames.Text.Plain;

            if (fileExtension == ".pdf") contentType = MediaTypeNames.Application.Pdf;
            if (fileExtension == ".xml") contentType = MediaTypeNames.Text.Xml;
            if (fileExtension == ".txt") contentType = MediaTypeNames.Text.Plain;
            if (fileExtension == ".igs") contentType = "application/iges";
            if (fileExtension == ".iges") contentType = "application/iges";
            if (fileExtension == ".stp") contentType = "application/step";
            if (fileExtension == ".step") contentType = "application/step";
            if (fileExtension == ".jpg") contentType = MediaTypeNames.Image.Jpeg;
            if (fileExtension == ".jpeg") contentType = MediaTypeNames.Image.Jpeg;
            if (fileExtension == ".png") contentType = "image/png";
            if (fileExtension == ".gif") contentType = MediaTypeNames.Image.Gif;

            return contentType;
        }

        public Stream GetPackageStream(string key)
        {
            return System.IO.File.OpenRead(key);
        }

        public Stream GetStreamFromPackagePart(string key, string uriString)
        {
            Package package = Package.Open(key, FileMode.Open, FileAccess.Read, FileShare.Read);
 
            var part = package.GetPart(new Uri(uriString, UriKind.RelativeOrAbsolute));
            if (part == null)
            {
                return null;
            }

            return part.GetStream(FileMode.Open);
        }

        public Stream GetLocalThumbnailStream(string key)
        {
            Package package = Package.Open(key, FileMode.Open, FileAccess.Read, FileShare.Read);
            
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

            return thumbPart.GetStream(FileMode.Open);
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

        public void SaveAs(string key, AssetAdministrationShellEnvironment env)
        {
            throw new NotImplementedException();

            /*TODO
            // approach is to utilize the existing package, if possible. If not, create from scratch
            Package package = Package.Open(filename, FileMode.OpenOrCreate);

            // get the origin from the package
            PackagePart originPart = null;
            var xs = package.GetRelationshipsByType("http://www.admin-shell.io/aasx/relationships/aasx-origin");
            foreach (var x in xs)
            {
                if (x.SourceUri.ToString() == "/")
                {
                    originPart = package.GetPart(x.TargetUri);
                    break;
                }
            }

            if (originPart == null)
            {
                // create, as not existing
                originPart = package.CreatePart(new Uri("/aasx/aasx-origin", UriKind.RelativeOrAbsolute), System.Net.Mime.MediaTypeNames.Text.Plain, CompressionOption.Maximum);
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
            xs = originPart.GetRelationshipsByType("http://www.admin-shell.io/aasx/relationships/aas-spec");
            foreach (var x in xs)
            {
                specRel = x;
                specPart = package.GetPart(x.TargetUri);
                break;
            }

            // check, if we have to change the spec part
            if (specPart != null && specRel != null)
            {
                var name = Path.GetFileNameWithoutExtension(specPart.Uri.ToString()).ToLower().Trim();
                var ext = Path.GetExtension(specPart.Uri.ToString()).ToLower().Trim();
                if (ext == ".json" && prefFmt == PreferredFormat.Xml
                        || ext == ".xml" && prefFmt == PreferredFormat.Json
                        || name.StartsWith("aasenv-with-no-Id"))
                {
                    // try kill specpart
                    try
                    {
                        originPart.DeleteRelationship(specRel.Id);
                        package.DeletePart(specPart.Uri);
                    }
                    catch { }
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
                var frn = "aasenv-with-no-Id";

                if (_aasenv.AssetAdministrationShells.Count > 0)
                    frn = _aasenv.AssetAdministrationShells[0].GetFriendlyName() ?? frn;

                var aas_spec_fn = "/aasx/#/#.aas";

                if (prefFmt == PreferredFormat.Json)
                    aas_spec_fn += ".json";
                else
                    aas_spec_fn += ".xml";

                aas_spec_fn = aas_spec_fn.Replace("#", "" + frn);

                specPart = package.CreatePart(new Uri(aas_spec_fn, UriKind.RelativeOrAbsolute), System.Net.Mime.MediaTypeNames.Text.Xml, CompressionOption.Maximum);

                originPart.CreateRelationship(specPart.Uri, TargetMode.Internal, "http://www.admin-shell.io/aasx/relationships/aas-spec");
            }

            // now, specPart shall be != null!
            if (specPart.Uri.ToString().ToLower().Trim().EndsWith("json"))
            {
                using (var s = specPart.GetStream(FileMode.Create))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.NullValueHandling = NullValueHandling.Ignore;
                    serializer.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
                    serializer.Formatting = Formatting.Indented;
                    using (var sw = new StreamWriter(s))
                    {
                        using (JsonWriter writer = new JsonTextWriter(sw))
                        {
                            serializer.Serialize(writer, _aasenv);
                        }
                    }
                }
            }
            else
            {
                using (var s = specPart.GetStream(FileMode.Create))
                {
                    var serializer = new XmlSerializer(typeof(AssetAdministrationShellEnvironment));
                    var nss = new XmlSerializerNamespaces();
                    nss.Add("xsi", System.Xml.Schema.XmlSchema.InstanceNamespace);
                    nss.Add("aas", "http://www.admin-shell.io/aas/2/0");
                    nss.Add("IEC61360", "http://www.admin-shell.io/IEC61360/2/0");
                    serializer.Serialize(s, _aasenv, nss);
                }
            }

            // there might be pending files to be deleted (first delete, then add, in case of identical files in both categories)
            foreach (var psfDel in _pendingFilesToDelete)
            {
                // try find an existing part for that file ..
                var found = false;

                // normal files
                xs = specPart.GetRelationshipsByType("http://www.admin-shell.io/aasx/relationships/aas-suppl");
                foreach (var x in xs)
                {
                    if (x.TargetUri == psfDel.Uri)
                    {
                        // try to delete
                        specPart.DeleteRelationship(x.Id);
                        package.DeletePart(psfDel.Uri);
                        found = true;
                        break;
                    }
                }

                // thumbnails
                xs = package.GetRelationshipsByType("http://schemas.openxmlformats.org/package/2006/relationships/metadata/thumbnail");
                foreach (var x in xs)
                {
                    if (x.TargetUri == psfDel.Uri)
                    {
                        // try to delete
                        package.DeleteRelationship(x.Id);
                        package.DeletePart(psfDel.Uri);
                        found = true;
                        break;
                    }
                }

                if (!found)
                    throw new Exception($"Not able to delete pending file {psfDel.Uri} in saving package {filename}");
            }

            // after this, there are no more pending for delete files
            _pendingFilesToDelete.Clear();

            // write pending supplementary files
            foreach (var psfAdd in _pendingFilesToAdd)
            {
                // make sure ..
                if (psfAdd.SourceLocalPath == null && psfAdd.SourceBytes == null || psfAdd.Location != PackageSupplementaryFile.LocationType.AddPending)
                {
                    continue;
                }

                // normal file?
                if (psfAdd.SpecialHandling == PackageSupplementaryFile.SpecialHandlingType.None
                    || psfAdd.SpecialHandling == PackageSupplementaryFile.SpecialHandlingType.EmbedAsThumbnail)
                {

                    // try find an existing part for that file ..
                    PackagePart filePart = null;
                    if (psfAdd.SpecialHandling == PackageSupplementaryFile.SpecialHandlingType.None)
                    {
                        xs = specPart.GetRelationshipsByType("http://www.admin-shell.io/aasx/relationships/aas-suppl");
                        foreach (var x in xs)
                            if (x.TargetUri == psfAdd.Uri)
                            {
                                filePart = package.GetPart(x.TargetUri);
                                break;
                            }
                    }

                    if (psfAdd.SpecialHandling == PackageSupplementaryFile.SpecialHandlingType.EmbedAsThumbnail)
                    {
                        xs = package.GetRelationshipsByType("http://schemas.openxmlformats.org/package/2006/relationships/metadata/thumbnail");
                        foreach (var x in xs)
                            if (x.SourceUri.ToString() == "/" && x.TargetUri == psfAdd.Uri)
                            {
                                filePart = package.GetPart(x.TargetUri);
                                break;
                            }
                    }

                    if (filePart == null)
                    {
                        // determine mimeType
                        var mimeType = psfAdd.UseMimeType;

                        // reconcile mime
                        if (mimeType == null && psfAdd.SourceLocalPath != null)
                        {
                            mimeType = GuessMimeType(psfAdd.SourceLocalPath);
                        }

                        // still null?
                        if (mimeType == null)
                        {
                            // see: https://stackoverflow.com/questions/6783921/which-mime-Type-to-use-for-a-binary-file-thats-specific-to-my-program
                            mimeType = "application/octet-stream";
                        }

                        // create new part and link
                        filePart = package.CreatePart(psfAdd.Uri, mimeType, CompressionOption.Maximum);

                        if (psfAdd.SpecialHandling == PackageSupplementaryFile.SpecialHandlingType.None)
                        {
                            specPart.CreateRelationship(filePart.Uri, TargetMode.Internal, "http://www.admin-shell.io/aasx/relationships/aas-suppl");
                        }

                        if (psfAdd.SpecialHandling == PackageSupplementaryFile.SpecialHandlingType.EmbedAsThumbnail)
                        {
                            package.CreateRelationship(filePart.Uri, TargetMode.Internal, "http://schemas.openxmlformats.org/package/2006/relationships/metadata/thumbnail");
                        }
                    }

                    // now should be able to write
                    using (var s = filePart.GetStream(FileMode.Create))
                    {
                        if (psfAdd.SourceLocalPath != null)
                        {
                            var bytes = System.IO.File.ReadAllBytes(psfAdd.SourceLocalPath);
                            s.Write(bytes, 0, bytes.Length);
                        }

                        if (psfAdd.SourceBytes != null)
                        {
                            var bytes = psfAdd.SourceBytes;
                            if (bytes != null)
                                s.Write(bytes, 0, bytes.Length);
                        }
                    }
                }


            }

            // after this, there are no more pending for add files
            _pendingFilesToAdd.Clear();

            // flush and close
            package.Flush();
            _openPackage = null;
            package.Close();

            // if in temp _fn, close the package, copy to original _fn, re-open the package
            if (_tempFn != null)
            {
                package.Close();
                System.IO.File.Copy(_tempFn, filename, overwrite: true);
                _openPackage = Package.Open(_tempFn, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
*/
        }
    }
}
