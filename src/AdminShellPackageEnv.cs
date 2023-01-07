
namespace AdminShell
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Packaging;
    using System.Xml.Serialization;

    /// <summary>
    /// This class encapsulates an AdminShellEnvironment and supplementary files into an AASX Package.
    /// Specifically has the capability to load, update and store .XML, .JSON and .AASX packages.
    /// </summary>
    public class AdminShellPackageEnv
    {
        private string _fn = "New Package";
        private string _tempFn = null;

        private AssetAdministrationShellEnvironment _aasenv = new AssetAdministrationShellEnvironment();
        private Package _openPackage = null;
        private List<PackageSupplementaryFile> _pendingFilesToAdd = new List<PackageSupplementaryFile>();
        private List<PackageSupplementaryFile> _pendingFilesToDelete = new List<PackageSupplementaryFile>();

        public AdminShellPackageEnv()
        {
        }

        public AdminShellPackageEnv(string fn, bool indirectLoadSave = false)
        {
            Load(fn, indirectLoadSave);
        }

        public string Filename
        {
            get
            {
                return _fn;
            }
        }

        public AssetAdministrationShellEnvironment AasEnv
        {
            get
            {
                return _aasenv;
            }
        }

        public bool Load(string fn, bool indirectLoadSave = false)
        {
            _fn = fn;
            if (_openPackage != null)
                _openPackage.Close();
            _openPackage = null;

            if (fn.ToLower().EndsWith(".xml"))
            {
                // load only XML
                var reader = new StreamReader(fn);
                // TODO: use _aasenv serialzers here!
                _aasenv = AdminShellSerializationHelper.DeserializeXmlFromStreamWithCompat(reader.BaseStream);
                if (_aasenv == null)
                    throw new Exception("Type error for XML file!");

                reader.Close();

                return true;
            }

            if (fn.ToLower().EndsWith(".aasx"))
            {
                var fnToLoad = fn;
                _tempFn = null;
                if (indirectLoadSave)
                {
                    _tempFn = Path.GetTempFileName().Replace(".tmp", ".aasx");
                    System.IO.File.Copy(fn, _tempFn);
                    fnToLoad = _tempFn;
                }

                string nsuri = "";

                // load package AASX
                Package package = Package.Open(fnToLoad, FileMode.Open, FileAccess.Read, FileShare.Read);

                // get the origin from the package
                PackagePart originPart = null;
                var xs = package.GetRelationshipsByType("http://www.admin-shell.io/aasx/relationships/aasx-origin");
                foreach (var x in xs)
                    if (x.SourceUri.ToString() == "/")
                    {
                        originPart = package.GetPart(x.TargetUri);
                        break;
                    }

                if (originPart == null)
                    throw (new Exception("Unable to find AASX origin. Aborting!"));

                // get the specs from the package
                PackagePart specPart = null;
                xs = originPart.GetRelationshipsByType("http://www.admin-shell.io/aasx/relationships/aas-spec");
                foreach (var x in xs)
                {
                    specPart = package.GetPart(x.TargetUri);
                    break;
                }
                if (specPart == null)
                    throw (new Exception("Unable to find AASX spec(s). Aborting!"));

                // open spec part to read
                var s = specPart.GetStream(FileMode.Open);

                // new
                nsuri = AdminShellSerializationHelper.TryReadXmlFirstElementNamespaceURI(s);

                s.Close();
                s = specPart.GetStream(FileMode.Open, FileAccess.Read);

                // read V1.0
                if (nsuri != null && nsuri.Trim() == "http://www.admin-shell.io/aas/1/0")
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(AssetAdministrationShellEnvironment), "http://www.admin-shell.io/aas/1/0");
                    _aasenv = serializer.Deserialize(s) as AssetAdministrationShellEnvironment;
                }

                // read V2.0
                if (nsuri != null && nsuri.Trim() == "http://www.admin-shell.io/aas/2/0")
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(AssetAdministrationShellEnvironment), "http://www.admin-shell.io/aas/2/0");
                    _aasenv = serializer.Deserialize(s) as AssetAdministrationShellEnvironment;
                }

                // read V3.0
                if (nsuri != null && nsuri.Trim() == "http://www.admin-shell.io/aas/3/0")
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(AssetAdministrationShellEnvironment), "http://www.admin-shell.io/aas/3/0");
                    _aasenv = serializer.Deserialize(s) as AssetAdministrationShellEnvironment;
                }

                _openPackage = package;

                if (_aasenv == null)
                    throw new Exception("Type error for XML file!");

                s.Close();
            }

            return true;
        }

        public enum PreferredFormat
        {
            None,
            Xml,
            Json
        };

        public bool SaveAs(string fn, bool writeFreshly = false, PreferredFormat prefFmt = PreferredFormat.None, MemoryStream useMemoryStream = null)
        {
            Console.WriteLine("SaveAs: " + fn);
            if (fn.ToLower().EndsWith(".xml"))
            {
                // save only XML
                _fn = fn;
                using (var s = new StreamWriter(fn))
                {
                    // TODO: use aasenv serialzers here!
                    var serializer = new XmlSerializer(typeof(AssetAdministrationShellEnvironment));
                    var nss = new XmlSerializerNamespaces();
                    nss.Add("xsi", System.Xml.Schema.XmlSchema.InstanceNamespace);
                    nss.Add("aas", "http://www.admin-shell.io/aas/2/0");
                    nss.Add("IEC61360", "http://www.admin-shell.io/IEC61360/2/0");
                    serializer.Serialize(s, _aasenv, nss);
                }

                return true;
            }

            if (fn.ToLower().EndsWith(".json"))
            {
                // save only JSON
                // this funcitonality is a initial test
                _fn = fn;
                using (var sw = new StreamWriter(fn))
                {
                    // TODO: use _aasenv serialzers here!

                    sw.AutoFlush = true;

                    JsonSerializer serializer = new JsonSerializer()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                        Formatting = Newtonsoft.Json.Formatting.Indented
                    };
                    using (JsonWriter writer = new JsonTextWriter(sw))
                    {
                        serializer.Serialize(writer, _aasenv);
                    }
                }

                return true;
            }

            if (fn.ToLower().EndsWith(".aasx"))
            {
                // save package AASX
                // we want existing contents to be preserved, but no possiblity to change file name
                // therefore: copy file to new name, re-open!
                // _fn could be changed, therefore close "old" package first
                if (_openPackage != null)
                {
                    _openPackage.Close();
                    if (!writeFreshly)
                    {
                        if (_tempFn != null)
                            System.IO.File.Copy(_tempFn, fn);
                        else
                            System.IO.File.Copy(fn, fn);
                    }

                    _openPackage = null;
                }

                // approach is to utilize the existing package, if possible. If not, create from scratch
                Package package = null;
                if (useMemoryStream != null)
                {
                    package = Package.Open(useMemoryStream, (writeFreshly) ? FileMode.Create : FileMode.OpenOrCreate);
                }
                else
                {
                    package = Package.Open((_tempFn != null) ? _tempFn : fn, (writeFreshly) ? FileMode.Create : FileMode.OpenOrCreate);
                }
                _fn = fn;

                // get the origin from the package
                PackagePart originPart = null;
                var xs = package.GetRelationshipsByType("http://www.admin-shell.io/aasx/relationships/aasx-origin");
                foreach (var x in xs)
                    if (x.SourceUri.ToString() == "/")
                    {
                        originPart = package.GetPart(x.TargetUri);
                        break;
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
                    if ((ext == ".json" && prefFmt == PreferredFormat.Xml)
                            || (ext == ".xml" && prefFmt == PreferredFormat.Json)
                            || (name.StartsWith("aasenv-with-no-Id")))
                    {
                        // try kill specpart
                        // ReSharper disable EmptyGeneralCatchClause
                        try
                        {
                            originPart.DeleteRelationship(specRel.Id);
                            package.DeletePart(specPart.Uri);
                        }
                        catch { }
                        finally { specPart = null; specRel = null; }
                        // ReSharper enable EmptyGeneralCatchClause
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
                        serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
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
                        if (x.TargetUri == psfDel.Uri)
                        {
                            // try to delete
                            specPart.DeleteRelationship(x.Id);
                            package.DeletePart(psfDel.Uri);
                            found = true;
                            break;
                        }

                    // thumbnails
                    xs = package.GetRelationshipsByType("http://schemas.openxmlformats.org/package/2006/relationships/metadata/thumbnail");
                    foreach (var x in xs)
                        if (x.TargetUri == psfDel.Uri)
                        {
                            // try to delete
                            package.DeleteRelationship(x.Id);
                            package.DeletePart(psfDel.Uri);
                            found = true;
                            break;
                        }

                    if (!found)
                        throw (new Exception($"Not able to delete pending file {psfDel.Uri} in saving package {fn}"));
                }

                // after this, there are no more pending for delete files
                _pendingFilesToDelete.Clear();

                // write pending supplementary files
                foreach (var psfAdd in _pendingFilesToAdd)
                {
                    // make sure ..
                    if ((psfAdd.SourceLocalPath == null && psfAdd.SourceBytes == null) || psfAdd.Location != PackageSupplementaryFile.LocationType.AddPending)
                        continue;

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
                                mimeType = AdminShellPackageEnv.GuessMimeType(psfAdd.SourceLocalPath);
                            // still null?
                            if (mimeType == null)
                                // see: https://stackoverflow.com/questions/6783921/which-mime-Type-to-use-for-a-binary-file-thats-specific-to-my-program
                                mimeType = "application/octet-stream";

                            // create new part and link
                            filePart = package.CreatePart(psfAdd.Uri, mimeType, CompressionOption.Maximum);
                            if (psfAdd.SpecialHandling == PackageSupplementaryFile.SpecialHandlingType.None)
                                specPart.CreateRelationship(filePart.Uri, TargetMode.Internal, "http://www.admin-shell.io/aasx/relationships/aas-suppl");
                            if (psfAdd.SpecialHandling == PackageSupplementaryFile.SpecialHandlingType.EmbedAsThumbnail)
                                package.CreateRelationship(filePart.Uri, TargetMode.Internal, "http://schemas.openxmlformats.org/package/2006/relationships/metadata/thumbnail");
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
                    System.IO.File.Copy(_tempFn, fn, overwrite: true);
                    _openPackage = Package.Open(_tempFn, FileMode.Open, FileAccess.Read, FileShare.Read);
                }

                return true;
            }

            // Don't know to handle
            throw (new Exception(string.Format($"Not able to handle {fn}.")));
        }

        public Stream GetLocalStreamFromPackage(string uriString)
        {
            // access
            if (_openPackage == null)
                throw (new Exception(string.Format($"AASX Package {_fn} not opened. Aborting!")));

            // gte part
            var part = _openPackage.GetPart(new Uri(uriString, UriKind.RelativeOrAbsolute));
            if (part == null)
                throw (new Exception(string.Format($"Cannot access URI {uriString} in {_fn} not opened. Aborting!")));
            return part.GetStream(FileMode.Open);
        }

        public Stream GetLocalThumbnailStream(ref Uri thumbUri)
        {
            // access
            if (_openPackage == null)
                throw (new Exception(string.Format($"AASX Package {_fn} not opened. Aborting!")));
            // get the thumbnail over the relationship
            PackagePart thumbPart = null;
            var xs = _openPackage.GetRelationshipsByType("http://schemas.openxmlformats.org/package/2006/relationships/metadata/thumbnail");
            foreach (var x in xs)
                if (x.SourceUri.ToString() == "/")
                {
                    thumbPart = _openPackage.GetPart(x.TargetUri);
                    thumbUri = x.TargetUri;
                    break;
                }
            if (thumbPart == null)
                return null;
            // throw (new Exception("Unable to find AASX thumbnail. Aborting!"));
            return thumbPart.GetStream(FileMode.Open);
        }

        public Stream GetLocalThumbnailStream()
        {
            Uri dummy = null;
            return GetLocalThumbnailStream(ref dummy);
        }

        public static string GuessMimeType(string fn)
        {
            var file_ext = System.IO.Path.GetExtension(fn).ToLower().Trim();
            var content_type = System.Net.Mime.MediaTypeNames.Text.Plain;
            if (file_ext == ".pdf") content_type = System.Net.Mime.MediaTypeNames.Application.Pdf;
            if (file_ext == ".xml") content_type = System.Net.Mime.MediaTypeNames.Text.Xml;
            if (file_ext == ".txt") content_type = System.Net.Mime.MediaTypeNames.Text.Plain;
            if (file_ext == ".igs") content_type = "application/iges";
            if (file_ext == ".iges") content_type = "application/iges";
            if (file_ext == ".stp") content_type = "application/step";
            if (file_ext == ".step") content_type = "application/step";
            if (file_ext == ".jpg") content_type = System.Net.Mime.MediaTypeNames.Image.Jpeg;
            if (file_ext == ".jpeg") content_type = System.Net.Mime.MediaTypeNames.Image.Jpeg;
            if (file_ext == ".png") content_type = "image/png";
            if (file_ext == ".gif") content_type = System.Net.Mime.MediaTypeNames.Image.Gif;
            return content_type;
        }

        public void ReplaceSupplementaryFileInPackageAsync(string sourceUri, string targetFile, string targetContentType, Stream fileContent)
        {
            // access
            if (_openPackage == null)
                throw new Exception(string.Format($"AASX Package not opened. Aborting!"));

            _openPackage.DeletePart(new Uri(sourceUri, UriKind.RelativeOrAbsolute));
            var targetUri = PackUriHelper.CreatePartUri(new Uri(targetFile, UriKind.RelativeOrAbsolute));
            PackagePart packagePart = _openPackage.CreatePart(targetUri, targetContentType);
            fileContent.Position = 0;
            using (Stream dest = packagePart.GetStream())
            {
                fileContent.CopyTo(dest);
            }
        }

        public void Close()
        {
            if (_openPackage != null)
                _openPackage.Close();
            _openPackage = null;
            _fn = "";
            _aasenv = null;
        }
    }
}
