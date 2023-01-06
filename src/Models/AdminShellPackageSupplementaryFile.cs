
namespace AdminShell
{
    using System;

    /// <summary>
    /// This class lets an outer functionality keep track on the supplementary files, which are in or
    /// are pending to be added or deleted to an Package.
    /// </summary>
    public class AdminShellPackageSupplementaryFile : Referable
    {
        public delegate byte[] SourceGetByteChunk();

        public enum LocationType
        {
            InPackage,
            AddPending,
            DeletePending
        }

        public enum SpecialHandlingType
        {
            None,
            EmbedAsThumbnail
        }

        public Uri uri = null;

        public string useMimeType = null;

        public string sourceLocalPath = null;

        public SourceGetByteChunk sourceGetBytesDel = null;

        public LocationType location;

        public SpecialHandlingType specialHandling;

        public AdminShellPackageSupplementaryFile(
            Uri uri,
            string sourceLocalPath = null,
            LocationType location = LocationType.InPackage,
            SpecialHandlingType specialHandling = SpecialHandlingType.None,
            SourceGetByteChunk sourceGetBytesDel = null,
            string useMimeType = null)
        {
            this.uri = uri;
            this.useMimeType = useMimeType;
            this.sourceLocalPath = sourceLocalPath;
            this.sourceGetBytesDel = sourceGetBytesDel;
            this.location = location;
            this.specialHandling = specialHandling;
        }
    }
}
