#define UseAasxCompatibilityModels

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Xml.Serialization;
using Newtonsoft.Json;

/* Copyright (c) 2018-2019 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>, author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/


#if UseAasxCompatibilityModels

namespace AasxCompatibilityModels
{

    #region Utils

    #endregion


    #region AdminShell_V1_0

    public partial class AdminShellV10
    {
        /// <summary>
        /// This class encapsulates an AdminShellEnvironment and supplementary files into an AASX Package.
        /// Specifically has the capability to load, update and store .XML, .JSON and .AASX packages.
        /// </summary>
        public class PackageEnv
        {
            private string fn = "New Package";
            private AdministrationShellEnv aasenv = new AdministrationShellEnv();
            private Package openPackage = null;
            private List<PackageSupplementaryFile> pendingFilesToAdd = new List<PackageSupplementaryFile>();
            private List<PackageSupplementaryFile> pendingFilesToDelete = new List<PackageSupplementaryFile>();

            public PackageEnv()
            {
            }

            public PackageEnv(AdministrationShellEnv env)
            {
                if (env != null)
                    this.aasenv = env;
            }

            public PackageEnv(string fn)
            {
                Load(fn);
            }

            public bool IsOpen
            {
                get
                {
                    return openPackage != null;
                }
            }

            public string Filename
            {
                get
                {
                    return fn;
                }
            }

            public AdminShellV10.AdministrationShellEnv AasEnv
            {
                get
                {
                    return aasenv;
                }
            }

            public bool Load(string fn)
            {
                this.fn = fn;
                if (this.openPackage != null)
                    this.openPackage.Close();
                this.openPackage = null;

                if (fn.ToLower().EndsWith(".xml"))
                {
                    // load only XML
                    try
                    {
                        // TODO: use aasenv serialzers here!
                        XmlSerializer serializer = new XmlSerializer(typeof(AdminShellV10.AdministrationShellEnv), "http://www.admin-shell.io/aas/1/0");
                        TextReader reader = new StreamReader(fn);
                        this.aasenv = serializer.Deserialize(reader) as AdminShellV10.AdministrationShellEnv;
                        if (this.aasenv == null)
                            throw (new Exception("Type error for XML file!"));
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        throw (new Exception(string.Format("While reading AAS {0} at {1} gave: {2}", fn, AdminShellUtil.ShortLocation(ex), ex.Message)));
                    }
                    return true;
                }

                if (fn.ToLower().EndsWith(".json"))
                {
                    // load only JSON
                    try
                    {
                        using (StreamReader file = System.IO.File.OpenText(fn))
                        {
                            // TODO: use aasenv serialzers here!
                            JsonSerializer serializer = new JsonSerializer();
                            serializer.Converters.Add(new AdminShellV10.JsonAasxConverter("modelType", "name"));
                            this.aasenv = (AdministrationShellEnv)serializer.Deserialize(file, typeof(AdministrationShellEnv));
                        }
                    }
                    catch (Exception ex)
                    {
                        throw (new Exception(string.Format("While reading AAS {0} at {1} gave: {2}", fn, AdminShellUtil.ShortLocation(ex), ex.Message)));
                    }
                    return true;
                }

                if (fn.ToLower().EndsWith(".aasx"))
                {
                    // load package AASX
                    try
                    {
                        var package = Package.Open(fn, FileMode.Open);

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
                            throw (new Exception(string.Format("Unable to find AASX origin. Aborting!")));

                        // get the specs from the package
                        PackagePart specPart = null;
                        xs = originPart.GetRelationshipsByType("http://www.admin-shell.io/aasx/relationships/aas-spec");
                        foreach (var x in xs)
                        {
                            specPart = package.GetPart(x.TargetUri);
                            break;
                        }
                        if (specPart == null)
                            throw (new Exception(string.Format("Unable to find AASX spec(s). Aborting!")));

                        // open spec part to read
                        try
                        {
                            if (specPart.Uri.ToString().ToLower().Trim().EndsWith("json"))
                            {
                                using (var s = specPart.GetStream(FileMode.Open))
                                {
                                    using (StreamReader file = new StreamReader(s))
                                    {
                                        JsonSerializer serializer = new JsonSerializer();
                                        serializer.Converters.Add(new AdminShellV10.JsonAasxConverter("modelType", "name"));
                                        this.aasenv = (AdministrationShellEnv)serializer.Deserialize(file, typeof(AdministrationShellEnv));
                                    }
                                }
                            }
                            else
                            {
                                using (var s = specPart.GetStream(FileMode.Open))
                                {
                                    // own catch loop to be more specific
                                    XmlSerializer serializer = new XmlSerializer(typeof(AdminShellV10.AdministrationShellEnv), "http://www.admin-shell.io/aas/1/0");
                                    this.aasenv = serializer.Deserialize(s) as AdminShellV10.AdministrationShellEnv;
                                    this.openPackage = package;
                                    if (this.aasenv == null)
                                        throw (new Exception("Type error for XML file!"));
                                    s.Close();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw (new Exception(string.Format("While reading AAS {0} spec at {1} gave: {2}", fn, AdminShellUtil.ShortLocation(ex), ex.Message)));
                        }
                    }
                    catch (Exception ex)
                    {
                        throw (new Exception(string.Format("While reading AASX {0} at {1} gave: {2}", fn, AdminShellUtil.ShortLocation(ex), ex.Message)));
                    }
                    return true;
                }

                // Don't know to handle
                throw (new Exception(string.Format($"Not able to handle {fn}.")));
            }

            public bool LoadFromAasEnvString(string content)
            {
                try
                {
                    using (var file = new StringReader(content))
                    {
                        // TODO: use aasenv serialzers here!
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Converters.Add(new AdminShellV10.JsonAasxConverter("modelType", "name"));
                        this.aasenv = (AdministrationShellEnv)serializer.Deserialize(file, typeof(AdministrationShellEnv));
                    }
                }
                catch (Exception ex)
                {
                    throw (new Exception(string.Format("While reading AASENV string {0} gave: {1}", AdminShellUtil.ShortLocation(ex), ex.Message)));
                }
                return true;
            }

            public enum PreferredFormat { None, Xml, Json };

            public bool SaveAs(string fn, bool writeFreshly = false, PreferredFormat prefFmt = PreferredFormat.None)
            {

                if (fn.ToLower().EndsWith(".xml"))
                {
                    // save only XML
                    this.fn = fn;
                    try
                    {
                        using (var s = new StreamWriter(this.fn))
                        {
                            // TODO: use aasenv serialzers here!
                            var serializer = new XmlSerializer(typeof(AdminShellV10.AdministrationShellEnv));
                            var nss = new XmlSerializerNamespaces();
                            nss.Add("xsi", System.Xml.Schema.XmlSchema.InstanceNamespace);
                            nss.Add("aas", "http://www.admin-shell.io/aas/1/0");
                            nss.Add("IEC61360", "http://www.admin-shell.io/IEC61360/1/0");
                            serializer.Serialize(s, this.aasenv, nss);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw (new Exception(string.Format("While writing AAS {0} at {1} gave: {2}", fn, AdminShellUtil.ShortLocation(ex), ex.Message)));
                    }
                    return true;
                }

                if (fn.ToLower().EndsWith(".json"))
                {
                    // save only JSON
                    // this funcitonality is a initial test
                    this.fn = fn;
                    try
                    {
                        using (var sw = new StreamWriter(fn))
                        {
                            // TODO: use aasenv serialzers here!

                            sw.AutoFlush = true;

                            JsonSerializer serializer = new JsonSerializer();
                            serializer.NullValueHandling = NullValueHandling.Ignore;
                            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
                            serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
                            using (JsonWriter writer = new JsonTextWriter(sw))
                            {
                                serializer.Serialize(writer, this.aasenv);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw (new Exception(string.Format("While writing AAS {0} at {1} gave: {2}", fn, AdminShellUtil.ShortLocation(ex), ex.Message)));
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
                        // fn could be changed, therefore close "old" package first
                        if (this.openPackage != null)
                        {
                            try
                            {
                                this.openPackage.Close();
                                if (!writeFreshly)
                                    System.IO.File.Copy(this.fn, fn);
                            }
                            catch { }
                            this.openPackage = null;
                        }

                        // approach is to utilize the existing package, if possible. If not, create from scratch
                        var package = Package.Open(fn, (writeFreshly) ? FileMode.Create : FileMode.OpenOrCreate);
                        this.fn = fn;

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
                                 || (name.StartsWith("aasenv-with-no-id")))
                            {
                                // try kill specpart
                                try
                                {
                                    originPart.DeleteRelationship(specRel.Id);
                                    package.DeletePart(specPart.Uri);
                                }
                                catch { }
                                finally { specPart = null; specRel = null; }
                            }
                        }

                        if (specPart == null)
                        {
                            // create, as not existing
                            var frn = "aasenv-with-no-id";
                            if (this.AasEnv.AdministrationShells.Count > 0)
                                frn = this.AasEnv.AdministrationShells[0].GetFriendlyName() ?? frn;
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
                                        serializer.Serialize(writer, this.aasenv);
                                    }
                                }
                            }
                        }
                        else
                        {
                            using (var s = specPart.GetStream(FileMode.Create))
                            {
                                var serializer = new XmlSerializer(typeof(AdminShellV10.AdministrationShellEnv));
                                var nss = new XmlSerializerNamespaces();
                                nss.Add("xsi", System.Xml.Schema.XmlSchema.InstanceNamespace);
                                nss.Add("aas", "http://www.admin-shell.io/aas/1/0");
                                nss.Add("IEC61360", "http://www.admin-shell.io/IEC61360/1/0");
                                serializer.Serialize(s, this.aasenv, nss);
                            }
                        }

                        // there might be pending files to be deleted (first delete, then add, in case of identical files in both categories)
                        foreach (var psfDel in pendingFilesToDelete)
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
                        pendingFilesToDelete.Clear();

                        // write pending supplementary files
                        foreach (var psfAdd in pendingFilesToAdd)
                        {
                            // make sure ..
                            if (psfAdd.sourcePath == null || psfAdd.location != PackageSupplementaryFile.LocationType.AddPending)
                                continue;

                            // normal file?
                            if (psfAdd.specialHandling == PackageSupplementaryFile.SpecialHandlingType.None)
                            {

                                // try find an existing part for that file ..
                                PackagePart filePart = null;
                                xs = specPart.GetRelationshipsByType("http://www.admin-shell.io/aasx/relationships/aas-suppl");
                                foreach (var x in xs)
                                    if (x.TargetUri == psfAdd.uri)
                                    {
                                        filePart = package.GetPart(x.TargetUri);
                                        break;
                                    }

                                if (filePart == null)
                                {
                                    // create new part and link
                                    filePart = package.CreatePart(psfAdd.uri, AdminShellV10.PackageEnv.GuessMimeType(psfAdd.sourcePath), CompressionOption.Maximum);
                                    specPart.CreateRelationship(filePart.Uri, TargetMode.Internal, "http://www.admin-shell.io/aasx/relationships/aas-suppl");
                                }

                                // now should be able to write
                                using (var s = filePart.GetStream(FileMode.Create))
                                {
                                    var bytes = System.IO.File.ReadAllBytes(psfAdd.sourcePath);
                                    s.Write(bytes, 0, bytes.Length);
                                }
                            }

                            // thumbnail file?
                            if (psfAdd.specialHandling == PackageSupplementaryFile.SpecialHandlingType.EmbedAsThumbnail)
                            {
                                // try find an existing part for that file ..
                                PackagePart filePart = null;
                                xs = package.GetRelationshipsByType("http://schemas.openxmlformats.org/package/2006/relationships/metadata/thumbnail");
                                foreach (var x in xs)
                                    if (x.SourceUri.ToString() == "/" && x.TargetUri == psfAdd.uri)
                                    {
                                        filePart = package.GetPart(x.TargetUri);
                                        break;
                                    }

                                if (filePart == null)
                                {
                                    // create new part and link
                                    filePart = package.CreatePart(psfAdd.uri, AdminShellV10.PackageEnv.GuessMimeType(psfAdd.sourcePath), CompressionOption.Maximum);
                                    package.CreateRelationship(filePart.Uri, TargetMode.Internal, "http://schemas.openxmlformats.org/package/2006/relationships/metadata/thumbnail");
                                }

                                // now should be able to write
                                using (var s = filePart.GetStream(FileMode.Create))
                                {
                                    var bytes = System.IO.File.ReadAllBytes(psfAdd.sourcePath);
                                    s.Write(bytes, 0, bytes.Length);
                                }
                            }
                        }

                        // after this, there are no more pending for add files
                        pendingFilesToAdd.Clear();

                        // flush, but leave open
                        package.Flush();
                        this.openPackage = package;
                    }
                    catch (Exception ex)
                    {
                        throw (new Exception(string.Format("While write AASX {0} at {1} gave: {2}", fn, AdminShellUtil.ShortLocation(ex), ex.Message)));
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
                    var fn = Path.Combine(backupDir, $"backup{ndx:000}.xml");

                    // raw save
                    using (var s = new StreamWriter(fn))
                    {
                        var serializer = new XmlSerializer(typeof(AdminShellV10.AdministrationShellEnv));
                        var nss = new XmlSerializerNamespaces();
                        nss.Add("xsi", System.Xml.Schema.XmlSchema.InstanceNamespace);
                        nss.Add("aas", "http://www.admin-shell.io/aas/1/0");
                        nss.Add("IEC61360", "http://www.admin-shell.io/IEC61360/1/0");
                        serializer.Serialize(s, this.aasenv, nss);
                    }
                }
                catch { }
            }

            public Stream GetLocalStreamFromPackage(string uriString)
            {
                // access
                if (this.openPackage == null)
                    throw (new Exception(string.Format($"AASX Package {this.fn} not opened. Aborting!")));
                var part = this.openPackage.GetPart(new Uri(uriString, UriKind.RelativeOrAbsolute));
                if (part == null)
                    throw (new Exception(string.Format($"Cannot access URI {uriString} in {this.fn} not opened. Aborting!")));
                return part.GetStream(FileMode.Open);
            }

            public long GetStreamSizeFromPackage(string uriString)
            {
                long res = 0;
                try
                {
                    if (this.openPackage == null)
                        return 0;
                    var part = this.openPackage.GetPart(new Uri(uriString, UriKind.RelativeOrAbsolute));
                    if (part == null)
                        return 0;
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
                if (this.openPackage == null)
                    throw (new Exception(string.Format($"AASX Package {this.fn} not opened. Aborting!")));
                // get the thumbnail over the relationship
                PackagePart thumbPart = null;
                var xs = this.openPackage.GetRelationshipsByType("http://schemas.openxmlformats.org/package/2006/relationships/metadata/thumbnail");
                foreach (var x in xs)
                    if (x.SourceUri.ToString() == "/")
                    {
                        thumbPart = this.openPackage.GetPart(x.TargetUri);
                        thumbUri = x.TargetUri;
                        break;
                    }
                if (thumbPart == null)
                    throw (new Exception(string.Format("Unable to find AASX thumbnail. Aborting!")));
                return thumbPart.GetStream(FileMode.Open);
            }

            public Stream GetLocalThumbnailStream()
            {
                Uri dummy = null;
                return GetLocalThumbnailStream(ref dummy);
            }

            public List<PackageSupplementaryFile> GetListOfSupplementaryFiles()
            {
                // new result
                var result = new List<PackageSupplementaryFile>();

                // access
                if (this.openPackage != null)
                {

                    // get the thumbnail(s) from the package
                    var xs = this.openPackage.GetRelationshipsByType("http://schemas.openxmlformats.org/package/2006/relationships/metadata/thumbnail");
                    foreach (var x in xs)
                        if (x.SourceUri.ToString() == "/")
                        {
                            result.Add(new PackageSupplementaryFile(
                                x.TargetUri,
                                location: PackageSupplementaryFile.LocationType.InPackage,
                                specialHandling: PackageSupplementaryFile.SpecialHandlingType.EmbedAsThumbnail));
                        }

                    // get the origin from the package
                    PackagePart originPart = null;
                    xs = this.openPackage.GetRelationshipsByType("http://www.admin-shell.io/aasx/relationships/aasx-origin");
                    foreach (var x in xs)
                        if (x.SourceUri.ToString() == "/")
                        {
                            originPart = this.openPackage.GetPart(x.TargetUri);
                            break;
                        }

                    if (originPart != null)
                    {
                        // get the specs from the origin
                        PackagePart specPart = null;
                        xs = originPart.GetRelationshipsByType("http://www.admin-shell.io/aasx/relationships/aas-spec");
                        foreach (var x in xs)
                        {
                            specPart = this.openPackage.GetPart(x.TargetUri);
                            break;
                        }

                        if (specPart != null)
                        {
                            // get the supplementaries from the package, derived from spec
                            xs = specPart.GetRelationshipsByType("http://www.admin-shell.io/aasx/relationships/aas-suppl");
                            foreach (var x in xs)
                            {
                                result.Add(new PackageSupplementaryFile(x.TargetUri, location: PackageSupplementaryFile.LocationType.InPackage));
                            }
                        }
                    }
                }

                // add or modify the files to delete
                foreach (var psfDel in pendingFilesToDelete)
                {
                    // already in
                    var found = result.Find(x => { return x.uri == psfDel.uri; });
                    if (found != null)
                        found.location = PackageSupplementaryFile.LocationType.DeletePending;
                    else
                    {
                        psfDel.location = PackageSupplementaryFile.LocationType.DeletePending;
                        result.Add(psfDel);
                    }
                }

                // add the files to store as well
                foreach (var psfAdd in pendingFilesToAdd)
                {
                    // already in (should not happen ?!)
                    var found = result.Find(x => { return x.uri == psfAdd.uri; });
                    if (found != null)
                        found.location = PackageSupplementaryFile.LocationType.AddPending;
                    else
                    {
                        psfAdd.location = PackageSupplementaryFile.LocationType.AddPending;
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

            public void AddSupplementaryFileToStore(string sourcePath, string targetDir, string targetFn, bool embedAsThumb)
            {
                // beautify parameters
                sourcePath = sourcePath.Trim();
                targetDir = targetDir.Trim();
                if (!targetDir.EndsWith("/"))
                    targetDir += "/";
                targetDir = targetDir.Replace(@"\", "/");
                targetFn = targetFn.Trim();
                if (sourcePath == "" || targetDir == "" || targetFn == "")
                    throw (new Exception(string.Format("Trying add supplementary file with empty name or path!")));

                var file_fn = "" + targetDir.Trim() + targetFn.Trim();

                // add record
                pendingFilesToAdd.Add(
                    new PackageSupplementaryFile(
                        new Uri(file_fn, UriKind.RelativeOrAbsolute),
                        sourcePath,
                        location: PackageSupplementaryFile.LocationType.AddPending,
                        specialHandling: (embedAsThumb ? PackageSupplementaryFile.SpecialHandlingType.EmbedAsThumbnail : PackageSupplementaryFile.SpecialHandlingType.None)
                    ));
            }

            public void DeleteSupplementaryFile(PackageSupplementaryFile psf)
            {
                if (psf == null)
                    throw (new Exception(string.Format("No supplementary file given!")));

                if (psf.location == PackageSupplementaryFile.LocationType.AddPending)
                {
                    // is still pending in add list -> remove
                    pendingFilesToAdd.RemoveAll((x) => { return x.uri == psf.uri; });
                }

                if (psf.location == PackageSupplementaryFile.LocationType.InPackage)
                {
                    // add to pending delete list
                    pendingFilesToDelete.Add(psf);
                }
            }

            /*
            private void AddSupplementaryFileDirect(string srcFn, string targetDir, string targetFn)
            {
                // access
                if (this.openPackage == null)
                    throw (new Exception(string.Format($"AASX Package {this.fn} not opened. Aborting!")));

                srcFn = srcFn.Trim();
                targetDir = targetDir.Trim();
                if (!targetDir.EndsWith("/"))
                    targetDir += "/";
                targetDir = targetDir.Replace(@"\", "/");
                targetFn = targetFn.Trim();
                if (srcFn == "" || targetDir == "" || targetFn == "")
                    throw (new Exception(string.Format("Trying add supplementary file with empty name or path!")));

                // get the origin from the package
                PackagePart originPart = null;
                var xs = this.openPackage.GetRelationshipsByType("http://www.admin-shell.io/aasx/relationships/aasx-origin");
                foreach (var x in xs)
                    if (x.SourceUri.ToString() == "/")
                    {
                        originPart = this.openPackage.GetPart(x.TargetUri);
                        break;
                    }
                if (originPart == null)
                    throw (new Exception(string.Format("Unable to find AASX origin. Aborting!")));

                // get the specs from the package
                PackagePart specPart = null;
                xs = originPart.GetRelationshipsByType("http://www.admin-shell.io/aasx/relationships/aas-spec");
                foreach (var x in xs)
                {
                    specPart = this.openPackage.GetPart(x.TargetUri);
                    break;
                }
                if (specPart == null)
                    throw (new Exception(string.Format("Unable to find AASX spec(s). Aborting!")));

                // add a supplementary file
                var file_fn = "" + targetDir.Trim() + targetFn.Trim();
                var file_part = this.openPackage.CreatePart(new Uri(file_fn, UriKind.RelativeOrAbsolute), AdminShellV10.PackageEnv.GuessMimeType(srcFn), CompressionOption.Maximum);
                using (var s = file_part.GetStream(FileMode.Create))
                {
                    var bytes = System.IO.File.ReadAllBytes(srcFn);
                    s.Write(bytes, 0, bytes.Length);
                }
                specPart.CreateRelationship(file_part.Uri, TargetMode.Internal, "http://www.admin-shell.io/aasx/relationships/aas-suppl");
            }
            */

            public void Close()
            {
                if (this.openPackage != null)
                    this.openPackage.Close();
                this.openPackage = null;
                this.fn = "";
                this.aasenv = null;
            }
        }

    }

    #endregion
}

#endif