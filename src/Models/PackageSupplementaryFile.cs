
namespace AdminShell
{
    using System;

    /// <summary>
    /// This class lets an outer functionality keep track on the supplementary files, which are in or
    /// are pending to be added or deleted to an Package.
    /// </summary>
    public class PackageSupplementaryFile : Referable
    {
        public enum LocationType { InPackage, AddPending, DeletePending }

        public enum SpecialHandlingType { None, EmbedAsThumbnail }

        public Uri uri = null;
        public string sourcePath = null;
        public LocationType location = LocationType.InPackage;
        public SpecialHandlingType specialHandling = SpecialHandlingType.None;

        public PackageSupplementaryFile(Uri uri, string sourcePath = null, LocationType location = LocationType.InPackage, SpecialHandlingType specialHandling = SpecialHandlingType.None)
        {
            this.uri = uri;
            this.sourcePath = sourcePath;
            this.location = location;
            this.specialHandling = specialHandling;
        }

        // class derives from Referable in order to provide GetElementName
        public override string GetElementName()
        {
            return "File";
        }
    }
}
