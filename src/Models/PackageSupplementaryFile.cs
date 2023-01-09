
namespace AdminShell
{
    using System;

    /// <summary>
    /// This class lets an outer functionality keep track on the supplementary files, which are in or
    /// are pending to be added to or deleted from a Package.
    /// </summary>
    public class PackageSupplementaryFile : Referable
    {
        public Uri Uri { get; set; }

        public string UseMimeType { get; set; }

        public string SourceLocalPath { get; set; }

        public byte[] SourceBytes { get; set; }

        public LocationType Location { get; set; }

        public SpecialHandlingType SpecialHandling { get; set; }
    }
}
