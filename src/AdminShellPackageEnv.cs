
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
        private List<AdminShellPackageSupplementaryFile> _pendingFilesToAdd = new List<AdminShellPackageSupplementaryFile>();
        private List<AdminShellPackageSupplementaryFile> _pendingFilesToDelete = new List<AdminShellPackageSupplementaryFile>();

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
                try
                {
                    var reader = new StreamReader(fn);
                    // TODO: use _aasenv serialzers here!
                    _aasenv = AdminShellSerializationHelper.DeserializeXmlFromStreamWithCompat(reader.BaseStream);
                    if (_aasenv == null)
                        throw (new Exception("Type error for XML file!"));
                    reader.Close();
                }
                catch (Exception ex)
                {
                    throw (new Exception(string.Format("While reading AAS {0} at {1} gave: {2}", fn, Program.ShortLocation(ex), ex.Message)));
                }
                return true;
            }

            if (fn.ToLower().EndsWith(".aasx"))
            {
                var fnToLoad = fn;
                _tempFn = null;
                if (indirectLoadSave)
                    try
                    {
                        _tempFn = System.IO.Path.GetTempFileName().Replace(".tmp", ".aasx");
                        System.IO.File.Copy(fn, _tempFn);
                        fnToLoad = _tempFn;


                    }
                    catch (Exception ex)
                    {
                        throw (new Exception(string.Format("While copying AASX {0} for indirect load at {1} gave: {2}", fn, Program.ShortLocation(ex), ex.Message)));
                    }

                string nsuri = "";

                // for (int loop = 0; loop < 2; loop++)
                for (int loop = 1; loop < 2; loop++)
                {
                    // load package AASX
                    try
                    {
                        Package package = null;
                        switch (loop)
                        {
                            case 0:
                                package = Package.Open(fnToLoad, FileMode.Open);
                                break;
                            case 1:
                                package = Package.Open(fnToLoad, FileMode.Open, FileAccess.Read, FileShare.Read);
                                break;
                        }

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
                        try
                        {
                            var s = specPart.GetStream(FileMode.Open);

                            {
                                // own catch loop to be more specific

                                if (loop == 0)
                                {
                                    nsuri = AdminShellSerializationHelper.TryReadXmlFirstElementNamespaceURI(s);
                                    s.Close();
                                    package.Close();
                                    continue;
                                }

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

                        }
                        catch (Exception ex)
                        {
                            throw (new Exception(string.Format("While reading AAS {0} spec at {1} gave: {2}", fn, Program.ShortLocation(ex), ex.Message)));
                        }
                        // Package must not close to read e.g. thumbnails
                        //// package.Close();
                    }
                    catch (Exception ex)
                    {
                        throw (new Exception(string.Format("While reading AASX {0} at {1} gave: {2}", fn, Program.ShortLocation(ex), ex.Message)));
                    }
                }
                return true;
            }

            // Don't know to handle
            throw (new Exception(string.Format($"Not able to handle {fn}.")));
        }

        public enum PreferredFormat { None, Xml, Json };

        public bool SaveAs(string fn, bool writeFreshly = false, PreferredFormat prefFmt = PreferredFormat.None, MemoryStream useMemoryStream = null)
        {
            Console.WriteLine("SaveAs: " + fn);
            if (fn.ToLower().EndsWith(".xml"))
            {
                // save only XML
                _fn = fn;
                try
                {
                    using (var s = new StreamWriter(fn))
                    {
                        // TODO: use _aasenv serialzers here!
                        var serializer = new XmlSerializer(typeof(AssetAdministrationShellEnvironment));
                        var nss = new XmlSerializerNamespaces();
                        nss.Add("xsi", System.Xml.Schema.XmlSchema.InstanceNamespace);
                        nss.Add("aas", "http://www.admin-shell.io/aas/2/0");
                        nss.Add("IEC61360", "http://www.admin-shell.io/IEC61360/2/0");
                        serializer.Serialize(s, _aasenv, nss);
                    }
                }
                catch (Exception ex)
                {
                    throw (new Exception(string.Format("While writing AAS {0} at {1} gave: {2}", fn, Program.ShortLocation(ex), ex.Message)));
                }
                return true;
            }

            if (fn.ToLower().EndsWith(".json"))
            {
                // save only JSON
                // this funcitonality is a initial test
                _fn = fn;
                try
                {
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
                }
                catch (Exception ex)
                {
                    throw (new Exception(string.Format("While writing AAS {0} at {1} gave: {2}", fn, Program.ShortLocation(ex), ex.Message)));
                }
                return true;
            }

            if (fn.ToLower().EndsWith(".aasx"))
            {
                // save package AASX
                try
                {
                    // we want existing contents to be preserved, but no possiblity to change file name
                    // therefore: copy file to new name, re-open!
                    // _fn could be changed, therefore close "old" package first
                    if (_openPackage != null)
                    {
                        // ReSharper disable EmptyGeneralCatchClause
                        try
                        {
                            _openPackage.Close();
                            if (!writeFreshly)
                            {
                                if (_tempFn != null)
                                    System.IO.File.Copy(_tempFn, fn);
                                else
                                    System.IO.File.Copy(fn, fn);
                            }
                        }
                        catch { }
                        // ReSharper enable EmptyGeneralCatchClause
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
                        var name = System.IO.Path.GetFileNameWithoutExtension(specPart.Uri.ToString()).ToLower().Trim();
                        var ext = System.IO.Path.GetExtension(specPart.Uri.ToString()).ToLower().Trim();
                        if ((ext == ".json" && prefFmt == PreferredFormat.Xml)
                             || (ext == ".xml" && prefFmt == PreferredFormat.Json)
                             || (name.StartsWith("_aasenv-with-no-Id")))
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
                        var frn = "_aasenv-with-no-Id";
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
                            if (x.TargetUri == psfDel.uri)
                            {
                                // try to delete
                                specPart.DeleteRelationship(x.Id);
                                package.DeletePart(psfDel.uri);
                                found = true;
                                break;
                            }

                        // thumbnails
                        xs = package.GetRelationshipsByType("http://schemas.openxmlformats.org/package/2006/relationships/metadata/thumbnail");
                        foreach (var x in xs)
                            if (x.TargetUri == psfDel.uri)
                            {
                                // try to delete
                                package.DeleteRelationship(x.Id);
                                package.DeletePart(psfDel.uri);
                                found = true;
                                break;
                            }

                        if (!found)
                            throw (new Exception($"Not able to delete pending file {psfDel.uri} in saving package {fn}"));
                    }

                    // after this, there are no more pending for delete files
                    _pendingFilesToDelete.Clear();

                    // write pending supplementary files
                    foreach (var psfAdd in _pendingFilesToAdd)
                    {
                        // make sure ..
                        if ((psfAdd.sourceLocalPath == null && psfAdd.sourceGetBytesDel == null) || psfAdd.location != AdminShellPackageSupplementaryFile.LocationType.AddPending)
                            continue;

                        // normal file?
                        if (psfAdd.specialHandling == AdminShellPackageSupplementaryFile.SpecialHandlingType.None
                            || psfAdd.specialHandling == AdminShellPackageSupplementaryFile.SpecialHandlingType.EmbedAsThumbnail)
                        {

                            // try find an existing part for that file ..
                            PackagePart filePart = null;
                            if (psfAdd.specialHandling == AdminShellPackageSupplementaryFile.SpecialHandlingType.None)
                            {
                                xs = specPart.GetRelationshipsByType("http://www.admin-shell.io/aasx/relationships/aas-suppl");
                                foreach (var x in xs)
                                    if (x.TargetUri == psfAdd.uri)
                                    {
                                        filePart = package.GetPart(x.TargetUri);
                                        break;
                                    }
                            }
                            if (psfAdd.specialHandling == AdminShellPackageSupplementaryFile.SpecialHandlingType.EmbedAsThumbnail)
                            {
                                xs = package.GetRelationshipsByType("http://schemas.openxmlformats.org/package/2006/relationships/metadata/thumbnail");
                                foreach (var x in xs)
                                    if (x.SourceUri.ToString() == "/" && x.TargetUri == psfAdd.uri)
                                    {
                                        filePart = package.GetPart(x.TargetUri);
                                        break;
                                    }
                            }

                            if (filePart == null)
                            {
                                // determine mimeType
                                var mimeType = psfAdd.useMimeType;
                                // reconcile mime
                                if (mimeType == null && psfAdd.sourceLocalPath != null)
                                    mimeType = AdminShellPackageEnv.GuessMimeType(psfAdd.sourceLocalPath);
                                // still null?
                                if (mimeType == null)
                                    // see: https://stackoverflow.com/questions/6783921/which-mime-Type-to-use-for-a-binary-file-thats-specific-to-my-program
                                    mimeType = "application/octet-stream";

                                // create new part and link
                                filePart = package.CreatePart(psfAdd.uri, mimeType, CompressionOption.Maximum);
                                if (psfAdd.specialHandling == AdminShellPackageSupplementaryFile.SpecialHandlingType.None)
                                    specPart.CreateRelationship(filePart.Uri, TargetMode.Internal, "http://www.admin-shell.io/aasx/relationships/aas-suppl");
                                if (psfAdd.specialHandling == AdminShellPackageSupplementaryFile.SpecialHandlingType.EmbedAsThumbnail)
                                    package.CreateRelationship(filePart.Uri, TargetMode.Internal, "http://schemas.openxmlformats.org/package/2006/relationships/metadata/thumbnail");
                            }

                            // now should be able to write
                            using (var s = filePart.GetStream(FileMode.Create))
                            {
                                if (psfAdd.sourceLocalPath != null)
                                {
                                    var bytes = System.IO.File.ReadAllBytes(psfAdd.sourceLocalPath);
                                    s.Write(bytes, 0, bytes.Length);
                                }

                                if (psfAdd.sourceGetBytesDel != null)
                                {
                                    var bytes = psfAdd.sourceGetBytesDel();
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
                        try
                        {
                            package.Close();
                            System.IO.File.Copy(_tempFn, fn, overwrite: true);
                            _openPackage = Package.Open(_tempFn, FileMode.Open, FileAccess.Read, FileShare.Read);
                        }
                        catch (Exception ex)
                        {
                            throw (new Exception(string.Format("While write AASX {0} indirectly at {1} gave: {2}", fn, Program.ShortLocation(ex), ex.Message)));
                        }
                }
                catch (Exception ex)
                {
                    throw (new Exception(string.Format("While write AASX {0} at {1} gave: {2}", fn, Program.ShortLocation(ex), ex.Message)));
                }
                return true;
            }

            // Don't know to handle
            throw (new Exception(string.Format($"Not able to handle {fn}.")));
        }

        private int BackupIndex = 0;

        public void BackupInDir(string backupDir, int maxFiles)
        {
            // access
            if (backupDir == null || maxFiles < 1)
                return;

            // we do it not caring on any errors
            // ReSharper disable EmptyGeneralCatchClause
            try
            {
                // get index in form
                if (BackupIndex == 0)
                {
                    // do not always start at 0!!
                    var rnd = new Random();
                    BackupIndex = rnd.Next(maxFiles);
                }
                var ndx = BackupIndex % maxFiles;
                BackupIndex += 1;

                // build a filename
                var bdfn = Path.Combine(backupDir, $"backup{ndx:000}.xml");

                // raw save
                using (var s = new StreamWriter(bdfn))
                {
                    var serializer = new XmlSerializer(typeof(AssetAdministrationShellEnvironment));
                    var nss = new XmlSerializerNamespaces();
                    nss.Add("xsi", System.Xml.Schema.XmlSchema.InstanceNamespace);
                    nss.Add("aas", "http://www.admin-shell.io/aas/2/0");
                    nss.Add("IEC61360", "http://www.admin-shell.io/IEC61360/2/0");
                    serializer.Serialize(s, _aasenv, nss);
                }
            }
            catch { }
            // ReSharper enable EmptyGeneralCatchClause
        }

        public bool IsLocalFile(string uriString)
        {
            // access
            if (_openPackage == null)
                throw (new Exception(string.Format($"AASX Package {_fn} not opened. Aborting!")));
            if (uriString == null || uriString == "" || !uriString.StartsWith("/"))
                return false;

            // check
            var isLocal = _openPackage.PartExists(new Uri(uriString, UriKind.RelativeOrAbsolute));
            return isLocal;
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

        public long GetStreamSizeFromPackage(string uriString)
        {
            long res = 0;
            try
            {
                if (_openPackage == null)
                    return 0;
                var part = _openPackage.GetPart(new Uri(uriString, UriKind.RelativeOrAbsolute));
                using (var s = part.GetStream(FileMode.Open))
                {
                    res = s.Length;
                }
            }
            catch { return 0; }
            return res;
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

        public List<AdminShellPackageSupplementaryFile> GetListOfSupplementaryFiles()
        {
            // new result
            var result = new List<AdminShellPackageSupplementaryFile>();

            // access
            if (_openPackage != null)
            {

                // get the thumbnail(s) from the package
                var xs = _openPackage.GetRelationshipsByType("http://schemas.openxmlformats.org/package/2006/relationships/metadata/thumbnail");
                foreach (var x in xs)
                    if (x.SourceUri.ToString() == "/")
                    {
                        result.Add(new AdminShellPackageSupplementaryFile(
                            x.TargetUri,
                            location: AdminShellPackageSupplementaryFile.LocationType.InPackage,
                            specialHandling: AdminShellPackageSupplementaryFile.SpecialHandlingType.EmbedAsThumbnail));
                    }

                // get the origin from the package
                PackagePart originPart = null;
                xs = _openPackage.GetRelationshipsByType("http://www.admin-shell.io/aasx/relationships/aasx-origin");
                foreach (var x in xs)
                    if (x.SourceUri.ToString() == "/")
                    {
                        originPart = _openPackage.GetPart(x.TargetUri);
                        break;
                    }

                if (originPart != null)
                {
                    // get the specs from the origin
                    PackagePart specPart = null;
                    xs = originPart.GetRelationshipsByType("http://www.admin-shell.io/aasx/relationships/aas-spec");
                    foreach (var x in xs)
                    {
                        specPart = _openPackage.GetPart(x.TargetUri);
                        break;
                    }

                    if (specPart != null)
                    {
                        // get the supplementaries from the package, derived from spec
                        xs = specPart.GetRelationshipsByType("http://www.admin-shell.io/aasx/relationships/aas-suppl");
                        foreach (var x in xs)
                        {
                            result.Add(new AdminShellPackageSupplementaryFile(x.TargetUri, location: AdminShellPackageSupplementaryFile.LocationType.InPackage));
                        }
                    }
                }
            }

            // add or modify the files to delete
            foreach (var psfDel in _pendingFilesToDelete)
            {
                // already in
                var found = result.Find(x => { return x.uri == psfDel.uri; });
                if (found != null)
                    found.location = AdminShellPackageSupplementaryFile.LocationType.DeletePending;
                else
                {
                    psfDel.location = AdminShellPackageSupplementaryFile.LocationType.DeletePending;
                    result.Add(psfDel);
                }
            }

            // add the files to store as well
            foreach (var psfAdd in _pendingFilesToAdd)
            {
                // already in (should not happen ?!)
                var found = result.Find(x => { return x.uri == psfAdd.uri; });
                if (found != null)
                    found.location = AdminShellPackageSupplementaryFile.LocationType.AddPending;
                else
                {
                    psfAdd.location = AdminShellPackageSupplementaryFile.LocationType.AddPending;
                    result.Add(psfAdd);
                }
            }

            // done
            return result;
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

        public void AddSupplementaryFileToStore(string sourcePath, string targetDir, string targetFn, bool embedAsThumb,
            AdminShellPackageSupplementaryFile.SourceGetByteChunk sourceGetBytesDel = null, string useMimeType = null)
        {
            // beautify parameters
            if ((sourcePath == null && sourceGetBytesDel == null) || targetDir == null || targetFn == null)
                return;

            // build target path
            targetDir = targetDir.Trim();
            if (!targetDir.EndsWith("/"))
                targetDir += "/";
            targetDir = targetDir.Replace(@"\", "/");
            targetFn = targetFn.Trim();
            if (sourcePath == "" || targetDir == "" || targetFn == "")
                throw (new Exception("Trying add supplementary file with empty name or path!"));

            var targetPath = "" + targetDir.Trim() + targetFn.Trim();

            // base funciton
            AddSupplementaryFileToStore(sourcePath, targetPath, embedAsThumb, sourceGetBytesDel, useMimeType);
        }

        public void AddSupplementaryFileToStore(string sourcePath, string targetPath, bool embedAsThumb,
            AdminShellPackageSupplementaryFile.SourceGetByteChunk sourceGetBytesDel = null, string useMimeType = null)
        {
            // beautify parameters
            if ((sourcePath == null && sourceGetBytesDel == null) || targetPath == null)
                return;

            sourcePath = sourcePath?.Trim();
            targetPath = targetPath.Trim();

            // add record
            _pendingFilesToAdd.Add(
                new AdminShellPackageSupplementaryFile(
                    new Uri(targetPath, UriKind.RelativeOrAbsolute),
                    sourcePath,
                    location: AdminShellPackageSupplementaryFile.LocationType.AddPending,
                    specialHandling: (embedAsThumb ? AdminShellPackageSupplementaryFile.SpecialHandlingType.EmbedAsThumbnail : AdminShellPackageSupplementaryFile.SpecialHandlingType.None),
                    sourceGetBytesDel: sourceGetBytesDel,
                    useMimeType: useMimeType)
                );

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

        public void DeleteSupplementaryFile(AdminShellPackageSupplementaryFile psf)
        {
            if (psf == null)
                throw (new Exception("No supplementary file given!"));

            if (psf.location == AdminShellPackageSupplementaryFile.LocationType.AddPending)
            {
                // is still pending in add list -> remove
                _pendingFilesToAdd.RemoveAll((x) => { return x.uri == psf.uri; });
            }

            if (psf.location == AdminShellPackageSupplementaryFile.LocationType.InPackage)
            {
                // add to pending delete list
                _pendingFilesToDelete.Add(psf);
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

        public string MakePackageFileAvailableAsTempFile(string packageUri)
        {
            // get input stream
            var input = GetLocalStreamFromPackage(packageUri);
            // copy to temp file
            string tempext = Path.GetExtension(packageUri);
            string temppath = Path.GetTempFileName().Replace(".tmp", tempext);
            var temp = System.IO.File.OpenWrite(temppath);
            input.CopyTo(temp);
            temp.Close();
            return temppath;
        }
    }
}
